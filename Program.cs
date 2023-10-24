using DualStock.DAL;
using DualStock.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<StockCacheService>();
builder.Services.AddScoped<IStockRepo, StockRepo>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
var s = app.Services.GetRequiredService<StockCacheService>();

//StockRepo stockRepo = new StockRepo(s);
//stockRepo.test();

app.MapGet("/", () => "Hello World!");

app.UseCors();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Enable API calls to controllers    
});

app.Run();

