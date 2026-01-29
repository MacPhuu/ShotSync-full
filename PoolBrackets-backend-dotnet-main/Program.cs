using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.Interfaces;
using PoolBrackets_backend_dotnet.Repositories;
using PoolBrackets_backend_dotnet.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION CHECK (Clean Code: Fail Fast) ---
// Lấy chuỗi kết nối và JWT Config, nếu thiếu thì báo lỗi ngay lập tức
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key is not configured in appsettings.json.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException("JWT Issuer is not configured.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException("JWT Audience is not configured.");

// --- 2. DATABASE CONTEXT ---
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        )
    );
});

// --- 3. REGISTER SERVICES & REPOSITORIES (Dependency Injection) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IPlayerHistoryRepository, PlayerHistoryRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMatchService, MatchService>();

// --- 4. CONFIGURATION SWAGGER (With JWT Support) ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PoolBrackets API", Version = "v1" });

    // Add JWT Authentication Button to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token in the text input below.\n\nExample: 12345abcdef",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --- 5. CONFIGURATION CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// --- 6. CONFIGURATION AUTHENTICATION (JWT) ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        // Fix cảnh báo CS8604 bằng cách dùng biến jwtKey đã check null ở trên
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// --- 7. BUILD & RUN APP ---
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication(); // Phải đứng trước Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();