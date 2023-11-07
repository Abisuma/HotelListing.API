using HotelListing.API.Configurations;
using HotelListing.API.Data;
using HotelListing.API.Repository.IRepository;
using HotelListing.API.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Microsoft.AspNetCore.Identity;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentityCore<APIUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//removing Cors
builder.Services.AddCors(options=>options.AddPolicy("AllowAll", c=>c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
//registering serilog
builder.Host.UseSerilog((ctx, lc)=> lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));//ctx(instance of the builder) and lc(loggerconfiguration)

//registering automapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

//adding JWT for login purposes.
builder.Services.AddAuthentication(options =>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//JwtBearerDefaults.AuthenticationScheme is the equivalent of the magic string "Bearer" and csan be used in its place so as reduce spelling error.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,  
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
}); 
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();//Start logging the type of logging coming via httprequests
app.UseHttpsRedirection();
app.UseCors("AllowAll");  
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
