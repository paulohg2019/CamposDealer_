using CamposDealer.DB;
using CamposDealer.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Antes de executar o projeto, é necessário aplicar as migrações para criar o modelo na base de dados.
// Comando para rodar no terminal:
// cd CamposDealer
// dotnet ef migrations add InitialCreate


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Contexto>(options => 
{
    options.UseSqlServer("Server=DESKTOP-9HDSN48;Database=DbCamposDealer;Trusted_Connection=True;TrustServerCertificate=True;");
});


builder.Services.AddScoped<DataSeeder>(); // Adiciona o seeder como um serviço
builder.Services.AddHttpClient<ApiService>(); // Adiciona o seeder como um serviço

var app = builder.Build();

// Executar migrações e popular o banco de dados
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Contexto>();
    dbContext.Database.Migrate(); // Aplica as migrações

    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedData(); // Popula o banco
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
