var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte ao CORS para permitir qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Adiciona outros serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<RoutesService>();
builder.Services.AddTransient<GeoCodingService>();
builder.Services.AddTransient<RotasExibicaoController>();
builder.Services.AddTransient<BaseRoutesController>();
builder.Services.AddTransient<ConvertAndressSaveController>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplica a política de CORS criada
app.UseCors("PermitirTudo");

app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();