using AriNaturals.DataAccess;
using AriNaturals.Interfaces;
using AriNaturals.Profiles;
using AriNaturals.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AnDbConnection")));

// Add services to the container.
builder.Services.AddScoped<IEmailService, EmailService>();

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins!)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    var requestKey = context.Request.Headers["ari-na-api-key"];
//    var expectedKey = builder.Configuration["AppSettings:ApiKey"];

//    if (string.IsNullOrWhiteSpace(requestKey) || requestKey != expectedKey)
//    {
//        context.Response.StatusCode = 401;
//        await context.Response.WriteAsync("Unauthorized");
//        return;
//    }

//    await next();
//});



app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
