using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net_Core_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public static class TalkBackEndpoint
{
    public static IEndpointRouteBuilder MapTalkBackEnpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/talkback/hello", () => "Hello World").WithTags("TalkBack");
        builder.MapGet("api/talkback/sort", (QueryInteger integers) =>
        {
            if (integers is null)
            {
                return Results.BadRequest($"Please input parameters");
            }

            return Results.Ok(integers.Values?.OrderBy(x => x));
        }).WithTags("TalkBack");

        return builder;
    }
}

public class QueryInteger
{
    public List<int>? Values { get; init; }

    public static bool TryParse(string? value, IFormatProvider? provider,
                                out QueryInteger? queryInteger)
    {
        if (string.IsNullOrEmpty(value))
        {
            queryInteger = null;
            return false;
        }

        var list = value.Split(',').Select(int.Parse).ToList();

        queryInteger = new QueryInteger() { Values = list };
        return true;
    }
}