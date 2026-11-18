using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Em hospedagens como o Render, a porta vem na váriavel de ambiente PORT
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Aplica as migrations pendentes ao iniciar, apenas se a configuração permitir
if (app.Configuration.GetValue<bool>("RunMigrationsOnStartup"))
{
    app.Logger.LogInformation("Verificando e aplicando migrations pendentes...");

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    app.Logger.LogInformation("Banco de dados atualizado.");
}

// Captura exceções não tratadas e responde com um erro pradronizado.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "TaskManager API v1");
    });

    // Localmente a API pode usar HTTPS; em produção o TLS é tratado pela plataforma
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Logger.LogInformation("Task Manager API iniciada no ambiente {Environment}", app.Environment.EnvironmentName);

app.Run();
