using EarthApi.Caches;
using EarthApi.Filters;
using EarthApi.Repositories;
using EarthApi.Servicies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPlayerService, PlayerService>();
builder.Services.AddSingleton<OnlinePlayerCache>();
builder.Services.AddSingleton<PlayerBalanceCache>();
builder.Services.AddSingleton<GameInfoCache>();
builder.Services.AddSingleton<IEarthRepository, EarthRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders();
    });
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
