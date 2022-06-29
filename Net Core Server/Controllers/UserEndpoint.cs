using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Net_Core_Server.Data;
using Net_Core_Server.Models;
using Newtonsoft.Json;
using System;
using System.Security.Claims;

namespace Net_Core_Server.Controllers;

public static class UserEndpoint
{
    public static IEndpointRouteBuilder MapUserEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/user/new", async (string username, IUserDataAccess dataAccess) =>
        {
            string output = await dataAccess.ContainsUsername(username)
          ? "True - User Does Exist!"
          : "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
            return Results.Ok(output);
        }).WithTags("User");

        builder.MapPost("api/user/new", async (string username, IUserDataAccess dataAccess) =>
        {
            if (username is null)
            {
                return Results.BadRequest("Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
            }

            if (await dataAccess.ContainsUsername(username))
            {
                return Results.BadRequest("Oops. This username is already in use. Please try again with a new username.");
            }

            return Results.Ok(await dataAccess.Add(username));
        }).WithTags("User");

        builder.MapDelete("api/user/removeuser", [Authorize] async (string username, [FromHeader(Name = "ApiKey")] Guid apiKey, IUserDataAccess dataAccess, ClaimsPrincipal user) =>
        {
            var actualUsername = user?.FindFirstValue(ClaimTypes.Name);

            if (actualUsername is null || username != actualUsername)
            {
                return Results.Ok(false);
            }

            var removeSuccess = await dataAccess.Remove(apiKey);

            return Results.Ok(removeSuccess);
        }).WithTags("User");

        builder.MapPost("api/user/changerole", [Authorize(Roles = Role.Admin)] async ([FromBody] ChangeRoleRequest request, IUserDataAccess dataAccess) =>
        {
            if (request.Role is not Role.Admin or Role.User)
            {
                return Results.BadRequest("NOT DONE: Role does not exist");
            }

            var changeRoleSuccess = await dataAccess.ChangeRole(request.Username, request.Role);
            if (changeRoleSuccess is false)
            {
                return Results.BadRequest("NOT DONE: Username does not exist");
            }

            return Results.Ok("DONE");
        }).WithTags("User");

        return builder;
    }
}

public record ChangeRoleRequest
{
    [JsonProperty("username")]
    public string Username { get; init; } = null!;

    [JsonProperty("role")]
    public string Role { get; init; } = null!;
}