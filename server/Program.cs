using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Interfaces;
using server.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON enum configuration
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// OpenAPI
builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<InvoiceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Services
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMerchantCustomerService, MerchantCustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMerchantUserService, MerchantUserService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IInvoiceItemService, InvoiceItemService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Development OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

// Test endpoint
app.MapGet("/db-test", async (InvoiceDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();

    return canConnect
        ? Results.Ok("Database connection successful.")
        : Results.Problem("Database connection failed.");
});

// Controllers
app.MapControllers();

app.Run();