using TESTE_API_GOOGLE.Controllers;
using TESTE_API_GOOGLE.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de expiração da sessão
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); // Adiciona suporte a Session


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<RoutesService>();
builder.Services.AddTransient<GeoCodingService>();
builder.Services.AddTransient<RotasExibicaoController>();
builder.Services.AddSwaggerGen();


builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.Http2.MaxRequestHeaderFieldSize = 8192;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adiciona o middleware de sessões antes do Authorization
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();