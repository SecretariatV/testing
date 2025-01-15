using Microsoft.EntityFrameworkCore;
using Test2Server.Data;
using Test2Server.Middlewares;
using Test2Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")))
);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITodoService, todoService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors();
app.UseMiddleware<AuthMiddleware>();

app.MapGet("/health", () => Results.Json(new { msg = "Hello Get Zell" }));
app.MapControllers();

var port = builder.Configuration.GetValue<int>("Port");
app.Run($"http://0.0.0.0:{port}");