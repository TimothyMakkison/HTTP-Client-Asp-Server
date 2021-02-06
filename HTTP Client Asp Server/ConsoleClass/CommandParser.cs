using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using RailwaySharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public static class CommandParser
    {
        public static Result<IEnumerable<object>, string> Parse(string input, CommandModel command)
        {
            var args = input.Replace(command.Data.CommandKey, "")
                            .Trim()
                            .Split()
                            .Where(arg => arg != default && arg != "")
                            .ToArray();
            var specificationsResult = Validate(args, command);

            if (Trial.Failed(specificationsResult))
                return Result<IEnumerable<object>, string>.FailWith(specificationsResult.FailedWith());

            var pairs = MatchValues(args, specificationsResult.SucceededWith());

            var outVal = pairs.SucceededWith().Select((pair) => TypeConverter.ChangeType(pair.Item2,
                                    pair.Item1.ConversionType,
                                    pair.Item1.TargetType == TargetType.Scalar,
                                    CultureInfo.InvariantCulture,
                                    true));

            var vals = outVal.Select(x => x.FromJust());

            return Result<IEnumerable<object>, string>.Succeed(vals);
        }

        public static Result<Specification[], string> Validate(IEnumerable<string> args, CommandModel command)
        {
            var specifications = command.Operation.Method.GetParameters()
                                                         .Select(Specification.FromPropertyInfo)
                                                         .ToArray();
            var name = command.Operation.Method.Name;
            var collectionCount = specifications.Where(spec => spec.TargetType != TargetType.Scalar).Count();

            return collectionCount > 1
                ? Result<Specification[], string>.FailWith("Ambiguous conversion.\nCommand " + name + " cannot have more than one collection parameter type")
                : specifications.Length > args.Count()
                ? Result<Specification[], string>.FailWith("Not enough input arguments. \nCommand " + name + " requires at least " + specifications.Length + " arguments.")
                : collectionCount == 0 && args.Count() > specifications.Length
                ? Result<Specification[], string>.FailWith("Too many input arguments. \nCommand " + name + " requires " + specifications.Length + " arguments.")
                : Result<Specification[], string>.Succeed(specifications);
        }

        /// <summary>
        /// Pairs types to its corresponding types.
        /// </summary>
        /// <param name="args">String array of user input.</param>
        /// <param name="specification">Collection of type data.</param>
        /// <returns>Tuple pairs of types and corresponding strings.</returns>
        private static Result<List<Tuple<Specification, IEnumerable<string>>>, string> MatchValues(
            IEnumerable<string> args,
            IEnumerable<Specification> specification)
        {
            var vals = args.ToArray();
            int index = 0;
            var diff = args.Count() - specification.Count();

            var outList = new List<Tuple<Specification, IEnumerable<string>>>();

            foreach (var spec in specification)
            {
                if (spec.TargetType == TargetType.Scalar)
                {
                    outList.Add(Tuple.Create(spec, vals[index].Yield()));
                    index++;
                }
                else
                {
                    outList.Add(Tuple.Create(spec, vals.Skip(index).Take(diff + 1)));
                    index += diff + 1;
                }
            }
            return Result<List<Tuple<Specification, IEnumerable<string>>>, string>.Succeed(outList);
        }
    }
}