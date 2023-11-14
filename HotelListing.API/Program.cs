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
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentityCore<APIUser>().AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<APIUser>>("HotelListingAPI")
    .AddEntityFrameworkStores<AppDbContext>(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//removing Cors
builder.Services.AddCors(options=>options.AddPolicy("AllowAll", c=>c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

//implementing and registering versioning
//builder.Services.AddApiVersioning(options =>
//{
//    options.AssumeDefaultVersionWhenUnspecified = true;//default version if no other versions are needed.
//    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);//specifying the default version
//    options.ReportApiVersions = true;
//    options.ApiVersionReader = ApiVersionReader.Combine(
//         new QueryStringApiVersionReader("api-version"),
//         new HeaderApiVersionReader("X-Version"),
//         new MediaTypeApiVersionReader("ver")
//    );
//});

//builder.Services.AddVersionedApiExplorer(
//    options =>
//    {
//        options.GroupNameFormat = "'v'VVV";//how we want the version name to look.    
//        options.SubstituteApiVersionInUrl = true;
//    });

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

//adding response cache

builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024; //cache size allow at a particular time
    options.UseCaseSensitivePaths = true;
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

app.UseResponseCaching();

app.Use(async (context,  next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
