using System.Globalization;
using System.Security.Authentication;
using AnonymizationApi.Data;
using AnonymizationApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = SslProtocols.Tls12;
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddHttpClient<IChatGptService, ChatGptService>(client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/chat/completions");
});


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<ICpfService, CpfService>();
builder.Services.AddScoped<IBirthDateService, BirthDateService>();
builder.Services.AddScoped<IHashService, HashService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
