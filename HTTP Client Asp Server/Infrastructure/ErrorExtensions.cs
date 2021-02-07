﻿using CSharpx;
using RailwaySharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class ErrorExtensions
    {
        /// <summary>
        /// Builds a Maybe type instance from a Result one.
        /// </summary>
        public static Maybe<TSuccess> ToMaybe<TSuccess, TMessage>(this Result<TSuccess, TMessage> result)
        {
            if (result.Tag == ResultType.Ok)
            {
                var ok = (Ok<TSuccess, TMessage>)result;
                return Maybe.Just(ok.Success);
            }
            return Maybe.Nothing<TSuccess>();
        }

        public static void RethrowWhenAbsentIn(this Exception exception, IEnumerable<Type> validExceptions)
        {
            if (!validExceptions.Contains(exception.GetType()))
            {
                throw exception;
            }
        }

        public static Result<TSuccess, Exception> ResultTry<TSuccess>(this Func<TSuccess> func)
        {
            if (func == null) throw new ArgumentException(null, nameof(func));

            try
            {
                return new Ok<TSuccess, Exception>(
                        func(), Enumerable.Empty<Exception>());
            }
            catch (Exception ex)
            {
                return new Bad<TSuccess, Exception>(
                    new[] { ex });
            }
        }
    }
}