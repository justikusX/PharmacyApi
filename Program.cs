using PharmacyApi.Data;
using PharmacyApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Pharmacy API", Version = "v1" });
});

var app = builder.Build();


try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbService = scope.ServiceProvider.GetRequiredService<MongoDbService>();
        await SeedData.InitializeAsync(dbService);
        Console.WriteLine("База данных успешно инициализирована");
    }
}
catch (Exception ex)
{
    Console.WriteLine($" Ошибка при инициализации БД: {ex.Message}");
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger"))
    {
        context.Request.Scheme = "https";
    }
    await next();
});

app.MapGet("/", () => Results.Ok(new
{
    Message = "Pharmacy API is running!",
    Endpoints = new
    {
        Swagger = "/swagger/index.html",
        AllPharmacies = "/api/pharmacy",
        PharmacyByCode = "/api/pharmacy/{code}",
        AllDrugs = "/api/pharmacy/drugs",
        DrugByCode = "/api/pharmacy/drugs/{drugCode}",
        DrugsByPharmacy = "/api/pharmacy/pharmacy/{pharmacyCode}/drugs",
        PharmaciesWithDrug = "/api/pharmacy/drug/{drugCode}/pharmacies",
        DeleteExpired = "/api/pharmacy/expired (DELETE)"
    }
}));

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();