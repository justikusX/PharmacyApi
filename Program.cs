using PharmacyApi.Data;
using PharmacyApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();
    await SeedData.InitializeAsync(dbService);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();