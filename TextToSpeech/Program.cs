﻿using Microsoft.AspNetCore.HttpOverrides;
using TextToSpeech.Options;
using TextToSpeech.TextToSpeechService;

var builder = WebApplication.CreateBuilder(args);

var environment = builder.Environment.EnvironmentName;

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddJsonFile("appsettings.local.json", optional: true);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = 
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Options
builder.Services
    .AddOptions<TTSOptions>()
    .Bind(builder.Configuration.GetSection("TTSOptions"))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<TTSConverter>();

// Set port number
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseForwardedHeaders();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextToSpeech API V1");
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.Run();