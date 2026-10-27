using Calculator.Data;
using Microsoft.EntityFrameworkCore;
using Calculator.Services;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<KafkaProducerHandler>();

builder.Services.AddSingleton<
    KafkaProducerService<Null, string>
>();

builder.Services.AddHostedService<KafkaConsumerService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Не задана строка подключения DefaultConnection.");

builder.Services.AddDbContext<CalculatorContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 5, 15))));

builder.Services.AddRazorPages();

var app = builder.Build();

// Создаёт базу/таблицы при первом запуске, если их ещё нет.
// Сама база данных должна быть предварительно создана на сервере.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CalculatorContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Index}/{id?}");

app.Run();
