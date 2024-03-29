﻿using Client.Infrastructure;
using Client.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Senders;

public class UserSender
{
    private readonly IAuthenticatedSender _sender;
    private readonly ILogger _output;

    public UserSender(IAuthenticatedSender sender, ILogger output)
    {
        _sender = sender;
        _output = output;
    }

    [Command("User Get")]
    public async Task<string> GetUser(string line)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"user/new?username={line}");
        var response = await _sender.SendAsync(request);
        var product = await _sender.GetResponseString(response);
        return product;
    }

    [Command("User Post")]
    public async Task<string> NewUser(string name)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "user/new")
        {
            Content = _sender.ToHttpContent(name)
        };

        HttpResponseMessage response = await _sender.SendAsync(request);
        var product = await _sender.GetResponseString(response);

        if (response.StatusCode is not HttpStatusCode.OK)
        {
            return product;
        }

        var jObject = JObject.Parse(product);

        var user = new User()
        {
            Username = jObject["userName"].Value<string>(),
            ApiKey = Guid.Parse(jObject["apiKey"].Value<string>())
        };

        _sender.UserHandler.Set(user);
        _output.Log("Got API Key");
        return user.ToString();
    }

    [Command("User Set")]
    public string UserSet(string name, Guid guid)
    {
        var user = new User() { Username = name, ApiKey = guid };
        _sender.UserHandler.Set(user);
        return $"Stored {user}";
    }

    [Command("User Delete")]
    public async Task DeleteUser()
    {
        if (!_sender.UserCheck())
        {
            return;
        }

        var request = new HttpRequestMessage(HttpMethod.Delete, $"user/removeuser?username={_sender.UserHandler.Value.Username}");
        var response = await _sender.SendAuthenticatedAsync(request);
        var product = await _sender.GetResponseString(response);
        var success = Convert.ToBoolean(product);
        _output.Log(success.ToString());
    }

    [Command("User Role")]
    public async Task ChangeRole(string username, string role)
    {
        if (!_sender.UserCheck())
        {
            return;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "user/changerole")
        {
            Content = _sender.ToHttpContent(new { username, role })
        };

        var response = await _sender.SendAuthenticatedAsync(request);
        _output.Log(await _sender.GetResponseString(response));
    }
}
