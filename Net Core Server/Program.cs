using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net_Core_Server.Controllers;
using Net_Core_Server.Data;
using Net_Core_Server.Filters;
using Net_Core_Server.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<UserContext>(opt =>
   opt.UseInMemoryDatabase("UserList"));

services.AddScoped<IUserDataAccess, UserDataAccess>();

services.AddSingleton<IAuthorizationFilter, AuthFilter>();

services.AddControllers();
services.AddMvcCore()
    .AddNewtonsoftJson();
services.AddHttpContextAccessor();

services.AddAuthentication("Base").AddScheme<AuthenticationSchemeOptions, AuthMiddleware>("Base", null);
services.AddAuthorization();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapTalkBackEndpoints();
app.MapUserEndpoint();
app.MapProtectedEndpoints();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public partial class Program
{ }