using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using EventPlus.WebAPI.Services;
using EventPlus.WebAPI.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

// ============================================================
// PROGRAM.CS - Ponto de Entrada e Configuração da Web API
// ============================================================
// Este arquivo configura todos os serviços da aplicação antes
// de iniciá-la. Pense nele como o "painel de controle" da API.
// ============================================================

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------
// 1. BANCO DE DADOS - Configuração do Entity Framework Core
// ----------------------------------------------------------
// Registra o EventContext no container de DI, lendo a string
// de conexão "DefaultConnection" definida no appsettings.json
builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------------------------------------
// 2. REPOSITÓRIOS - Injeção de Dependência (DI)
// ----------------------------------------------------------
// AddScoped significa que uma instância nova é criada por requisição HTTP.
// Isso garante que cada requisição tenha seu próprio contexto isolado.
builder.Services.AddScoped<ITipoEventoRepository, TipoEventoRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();
builder.Services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<IPresencaRepository, PresencaRepository>();

// ----------------------------------------------------------
// 3. SERVIÇOS DE IMAGEM E MODERAÇÃO
// ----------------------------------------------------------
builder.Services.AddOptions<CloudinarySettings>()
    .Bind(builder.Configuration.GetSection(CloudinarySettings.SectionName))
    .Validate(settings =>
        !string.IsNullOrWhiteSpace(settings.CloudName) &&
        !string.IsNullOrWhiteSpace(settings.ApiKey) &&
        !string.IsNullOrWhiteSpace(settings.ApiSecret),
        "Configure Cloudinary:CloudName, Cloudinary:ApiKey e Cloudinary:ApiSecret.")
    .ValidateOnStart();
builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();

// Moderação de Conteúdo:
// Registramos o MockContentModerationService como implementação inicial.
// Substitua por AzureContentSafetyService ou GooglePerspectiveService quando definir o provedor.
builder.Services.AddScoped<IContentModerationService, MockContentModerationService>();

// ----------------------------------------------------------
// 4. OPENAPI (nativo .NET 10) + Scalar UI
// ----------------------------------------------------------
// O .NET 10 usa o Microsoft.AspNetCore.OpenApi nativo para geração
// do documento OpenAPI e o Scalar como interface visual moderna.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Version = "v1",
            Title = "Event+ WebAPI",
            Description = "API para gerenciamento de eventos, usuários, presenças e comentários.",
            Contact = new OpenApiContact
            {
                Name = "Desenvolvimento de Sistemas",
                Url = new Uri("https://example.com")
            }
        };
        return Task.CompletedTask;
    });

    // Configuração do esquema de autenticação Bearer Token
    // Permite testar endpoints protegidos diretamente pelo Scalar UI
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

// ----------------------------------------------------------
// 5. AUTENTICAÇÃO JWT (JSON Web Token)
// ----------------------------------------------------------
// Configura como a API vai validar os tokens recebidos nas requisições.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida quem emitiu o token (Issuer)
        ValidateIssuer = true,
        ValidIssuer = "webapi.event+",

        // Valida para quem o token foi emitido (Audience)
        ValidateAudience = true,
        ValidAudience = "webapi.event+",

        // Valida se o token ainda está dentro do prazo de validade
        ValidateLifetime = true,

        // Define a tolerância de clock entre servidores (5 minutos)
        ClockSkew = TimeSpan.FromMinutes(5),

        // Chave secreta utilizada para validar a assinatura do token
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes("eventos-chave-autenticacao-webapi-dev")
        )
    };
});

// Registra o serviço de autorização (necessário para [Authorize] funcionar)
builder.Services.AddAuthorization();

// Registra o serviço de controllers (mapeia automaticamente os controllers da pasta /Controllers)
builder.Services.AddControllers();

//var allowedOrigins = builder.Configuration
//    .GetSection("Cors:AllowedOrigins")
//    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ----------------------------------------------------------
// Constrói a aplicação com todas as configurações acima
// ----------------------------------------------------------
var app = builder.Build();

// ----------------------------------------------------------
// 6. PIPELINE DE MIDDLEWARES (apenas em desenvolvimento)
// ----------------------------------------------------------

var scalarEnabled = builder.Configuration.GetValue<bool>("Scalar:Enabled");
if (app.Environment.IsDevelopment() || scalarEnabled)
{
    // Ativa o endpoint nativo OpenAPI do .NET 10 (gera o documento JSON)
    app.MapOpenApi();

    // Ativa o Scalar UI - interface visual moderna para testar a API
    // Acesse em: /scalar/v1
    app.MapScalarApiReference(options =>
    {
        options.Title = "Event+ WebAPI";
        options.Theme = ScalarTheme.DeepSpace;
        options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// Redireciona HTTP para HTTPS automaticamente
app.UseHttpsRedirection();

// Ativa o middleware de Autenticação (deve vir ANTES de UseAuthorization)
app.UseAuthentication();

// Ativa o middleware de Autorização
app.UseAuthorization();

// Mapeia as rotas definidas nos Controllers com os atributos [Route] e [HttpGet/Post/...]
app.MapControllers();

app.UseCors("Frontend");

// Inicia o servidor web e começa a ouvir requisições
app.Run();

// ----------------------------------------------------------
// TRANSFORMER: Adiciona o esquema de segurança Bearer ao OpenAPI
// ----------------------------------------------------------
/// <summary>
/// Transformer que injeta o esquema de autenticação Bearer JWT
/// no documento OpenAPI gerado nativamente pelo .NET 10.
/// </summary>
internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(scheme => scheme.Name == JwtBearerDefaults.AuthenticationScheme))
        {
            var requirements = new Dictionary<string, IOpenApiSecurityScheme>
            {
                [JwtBearerDefaults.AuthenticationScheme] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Insira o token JWT abaixo (sem precisar digitar 'Bearer '):"
                }
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;
        }
    }
}
