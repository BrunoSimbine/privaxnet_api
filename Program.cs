using privaxnet_api.Dtos;
using privaxnet_api.Services.AuthService;
using privaxnet_api.Services.UserService;
using privaxnet_api.Services.ProductService;
using privaxnet_api.Services.VoucherService;
using privaxnet_api.Services.MessageService;
using privaxnet_api.Services.CurrencyService;
using privaxnet_api.Services.WalletService;
using privaxnet_api.Services.PayAgentService;
using privaxnet_api.Services.PaymentService;
using privaxnet_api.Repository.UserRepository;
using privaxnet_api.Repository.ProductRepository;
using privaxnet_api.Repository.MessageRepository;
using privaxnet_api.Repository.VoucherRepository;
using privaxnet_api.Repository.CurrencyRepository;
using privaxnet_api.Repository.WalletRepository;
using privaxnet_api.Repository.PayAgentRepository;
using privaxnet_api.Repository.PaymentRepository;
using privaxnet_api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddCors(options => {
    options.AddPolicy(name: "MyPolicy",
        policy => {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("WhatsappClient", client => {
    client.BaseAddress = new Uri("https://whatsapp.api.privaxnet.com/");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Some Description",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidIssuer = "bruno.com",
        ValidAudience = "bruno.com",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jwfhgfhjsgdvjhdsg837483hf8743tfg8734gfyegf7634gf38734"))
    };
});
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPayAgentService, PayAgentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IPayAgentRepository, PayAgentRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();
app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("MyPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
