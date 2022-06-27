using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net_Core_Server.Controllers;
using Net_Core_Server.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<UserContext>(opt =>
   opt.UseInMemoryDatabase("UserList"));

services.AddScoped<IUserDataAccess, UserDataAccess>();
services.AddControllers();
services.AddMvcCore()
    .AddNewtonsoftJson();

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
app.UseMiddleware<Net_Core_Server.Middleware.AuthMiddleware>();
app.UseRouting();

app.UseAuthorization();

app.MapTalkBackEnpoints();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();