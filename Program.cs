using GerenciadorTarefas.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços da aplicação: MVC (controllers + views)
builder.Services.AddControllersWithViews(options =>
{
    // Mensagem em português quando um campo obrigatório chega vazio (ex.: data em branco)
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Este campo é obrigatório.");
});

// Entity Framework Core com SQL Server, usando a connection string "DefaultConnection".
// EnableRetryOnFailure tenta de novo em falhas temporárias, como o Azure SQL gratuito
// "acordando" depois de um tempo sem uso.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure()));

var app = builder.Build();

// Cria o banco e aplica as migrations pendentes ao iniciar,
// para o projeto rodar com um simples "dotnet run".
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Fora do ambiente de desenvolvimento, erros vão para uma página amigável
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// Rota padrão: /{controller}/{action}/{id}, começando em Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
