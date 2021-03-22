using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using RailwaySharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public static class CommandParser
    {
        public static Result<IEnumerable<object>, string> Parse(string input, CommandModel command)
        {
            var args = input.ToArgs(command.Data.CommandKey);

            // Validate input args and command arg types, pair args with types and then convert into objects.
            return Validate(args, command).Map(spec => MatchValues(args, spec))
                                          .Map(TypeConverter.ConvertTuples)
                                          .Flatten();
        }

        public static Result<Specification[], string> Validate(IEnumerable<string> args, CommandModel command)
        {
            var specifications = command.Operation.Method.GetParameters()
                                                         .Select(Specification.FromPropertyInfo)
                                                         .ToArray();
            var name = command.Operation.Method.Name;
            var collectionCount = specifications.Where(spec => spec.TargetType != TargetType.Scalar).Count();

            //TODO clean up return
            return collectionCount > 1
                ? Result<Specification[], string>.FailWith("Ambiguous conversion.\nCommand " + name + " cannot have more than one collection parameter type")
                : specifications.Length > args.Count()
                ? Result<Specification[], string>.FailWith("Not enough input arguments. \nCommand " + name + " requires at least " + specifications.Length + " arguments.")
                : collectionCount == 0 && args.Count() > specifications.Length
                ? Result<Specification[], string>.FailWith("Too many input arguments. \nCommand " + name + " requires " + specifications.Length + " arguments.")
                : Result<Specification[], string>.Succeed(specifications);
        }

        /// <summary>
        /// Pairs strings to their corresponding types.
        /// </summary>
        /// <param name="args">String array of user input.</param>
        /// <param name="specification">Collection of type data.</param>
        /// <returns>Tuple pairs of types and corresponding strings.</returns>
        private static List<Tuple<Specification, IEnumerable<string>>> MatchValues(
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
            return outList;
        }
    }
}