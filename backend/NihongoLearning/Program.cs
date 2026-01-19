using Microsoft.EntityFrameworkCore; // Thêm dòng này
using NihongoLearning.Models;      // Thêm dòng này (Thay bằng namespace của bạn)

var builder = WebApplication.CreateBuilder(args);

// --- ĐĂNG KÝ DATABASE TẠI ĐÂY ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// --------------------------------

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Thêm 2 dòng này để có giao diện Swagger test API cho dễ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // Thêm dòng này
    app.UseSwaggerUI(); // Thêm dòng này
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();