using Microsoft.EntityFrameworkCore;
using Shop_Duolingo.Models;
using Shop_Duolingo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Đăng ký DbContext
builder.Services.AddDbContext<JapaneseLearningShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

// Đăng ký Services
builder.Services.AddScoped<IShopService, ShopService>();

// Cấu hình Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Japanese Learning Shop API",
        Description = "API cho hệ thống Shop học tiếng Nhật - Dựa trên Duolingo",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Shop Team",
            Email = "support@japaneseshop.com"
        }
    });

    // Tùy chọn: Thêm XML comments nếu có
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Bật Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Japanese Learning Shop API v1");
        options.RoutePrefix = "swagger"; // Truy cập tại: https://localhost:5001/swagger
        options.DocumentTitle = "Japanese Learning Shop API";
        options.DefaultModelsExpandDepth(-1); // Ẩn schema models mặc định
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();