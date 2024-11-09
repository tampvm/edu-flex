using EduFlex.API;
using EduFlex.Service.Common;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

//builder.Services.AddHealthChecks();

var configuration = builder.Configuration.Get<AppConfiguration>();
if (configuration?.DatabaseConnection == null || configuration.JWT == null)
{
    throw new Exception("Cấu hình không hợp lệ hoặc không được tải. Kiểm tra lại cấu hình hoặc biến môi trường.");
}
builder.Services.AddInfrastructuresService(configuration.DatabaseConnection);
builder.Services.AddWebAPIService(configuration.JWT);
builder.Services.AddSingleton(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EduFlex API");
    options.RoutePrefix = string.Empty;
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

app.Run();
