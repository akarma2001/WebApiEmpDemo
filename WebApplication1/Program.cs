using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myPolicy",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://akangularempui.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
                
            });
});
      
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
// allow cors
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

//app.UseStaticFiles(
//    new StaticFileOptions
//    {
//        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
//        RequestPath = "/Photos"
//    });

app.UseCors("myPolicy");

app.Run();
