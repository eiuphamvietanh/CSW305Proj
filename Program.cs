using CSW305Proj.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

   
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: \"Bearer eyJhbGciOi...\""
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
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<CSW306DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
builder.Services.AddAuthorization(options =>
{
    // 1. OwnerOnly
    options.AddPolicy("OwnerOnly", policy =>
        policy.RequireAssertion(context =>
        {
            var userIdClaim = context.User.FindFirst("UserId")?.Value;
            var resourceOwnerId = context.Resource as string; 
            return userIdClaim == resourceOwnerId;
        }));

    // 2. OwnerOrAdmin
    options.AddPolicy("OwnerOrAdmin", policy =>
        policy.RequireAssertion(context =>
        {
            var userIdClaim = context.User.FindFirst("UserId")?.Value;
            var isAdmin = context.User.IsInRole("Admin");
            var resourceOwnerId = context.Resource as string;
            return userIdClaim == resourceOwnerId || isAdmin;
        }));

    // 3. AdminOwnerCustomer
    options.AddPolicy("AdminOwnerCustomer", policy =>
        policy.RequireRole("Admin", "Owner", "Customer"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
