using Microsoft.EntityFrameworkCore;
using Sln.Estoque.Application.Service.SQLServerServices;
using Sln.Estoque.Domain.IRepositories;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Infra.Data.Context;
using Sln.Estoque.Infra.Data.Repositories;
using System.Globalization;
using Sln.Estoque.Web.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sln.Estoque.Domain.Entities;

var cultureInfo = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configuration
builder.Configuration.AddEnvironmentVariables();

#region JWT
var key = Encoding.ASCII.GetBytes("a9fa81bfe8bf9e80e746de648a063144d2878b2841a12bef6f4d84025bdd2c59");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
	{
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "JWTdoSistemaYF",
        ValidAudience = "Aplicacao-YF",
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["userJwt"];
            return Task.CompletedTask;
        }
    };
});
#endregion

#region Conection String
// Criar o ConfigurationBuilder e adicionar o arquivo appsettings.json
var configurationBuilder = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

// Obtém a configuração do appsettings.json
var configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddConfiguration(configurationBuilder)
	.Build();

// Configurar a classe de configuração
var connectionStringsConfig = new ConnectionStringsConfig();
configuration.GetSection("ConnectionStrings").Bind(connectionStringsConfig);


// Configurar o contexto SQL Server
builder.Services.AddDbContext<SQLServerContext>(options =>
	options.UseSqlServer(connectionStringsConfig.SqlConnectionString));
#endregion

#region Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILayoutRepository, LayoutRepository>();
builder.Services.AddScoped<IPcpRepository, PcpRepository>();
#endregion

#region Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<IPcpService, PcpService>();
#endregion

#region Other Services
builder.Services.AddSingleton(connectionStringsConfig);
builder.Services.AddScoped<TokenGenerator>();
builder.Services.AddScoped<UserRoleService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#endregion

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();