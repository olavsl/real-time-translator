using Microsoft.AspNetCore.HttpOverrides;
using SpeechToSpeechTranslator.SpeechToSpeechTranslatorService;
using SpeechToSpeechTranslator.SpeechToTextTranslationService;
using SpeechToSpeechTranslator.TextToSpeechService;
using SpeechToSpeechTranslator.Options;

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
    .AddOptions<STTTOptions>()
    .Bind(builder.Configuration.GetSection("STTTOptions"))
    .ValidateDataAnnotations();

builder.Services
    .AddOptions<TTSOptions>()
    .Bind(builder.Configuration.GetSection("TTSOptions"))
    .ValidateDataAnnotations();

builder.Services.AddSingleton<RTT>();
builder.Services.AddSingleton<STTTConverter>();
builder.Services.AddSingleton<TTSConverter>();

// Set port number
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(4000);
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpeechToSpeechTranslator API V1");
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.Run();