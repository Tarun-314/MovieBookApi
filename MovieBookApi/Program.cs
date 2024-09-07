
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieBookApi.Models.Db;
using MovieBookApi.Services;
using System.Net;
using WebApi2.Infra;
using WebApi2.Models;

namespace MovieBookApi
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost4200",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            var Aud = builder.Configuration.GetValue<string>("Audience");
            var iss = builder.Configuration.GetValue<string>("Issuer");
            var sec = builder.Configuration.GetValue<string>("Secret");

            var keybytes = System.Text.Encoding.UTF8.GetBytes(sec);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(c =>
            {
                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = iss,
                    ValidAudience = Aud,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keybytes)
                };
            });

            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MovieBookApi",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header (Bearer scheme). \n Enter 'Bearer' <JWTTOKEN>.\r\n\r\nExample: \"Bearer JWTTOKEN\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            builder.Services.AddAuthorization(c =>
            {
               
                c.AddPolicy(SecurityPolicy.Admin, SecurityPolicy.AdminPolicy());
                c.AddPolicy(SecurityPolicy.User, SecurityPolicy.UserPolicy());
            });



            builder.Services.AddDbContext<MovieBookDbContext>(config =>
            {
                string cnstr = builder.Configuration.GetConnectionString("cstr")!;
                config.UseSqlServer(cnstr);
            });
            builder.Services.AddTransient(typeof(IAdminService), typeof(AdminService));
            builder.Services.AddTransient(typeof(IJWTService), typeof(JWTService));
            builder.Services.AddTransient(typeof(IUserService), typeof(UserService));
            builder.Services.AddTransient(typeof(IMovieStatisticsService), typeof(MovieStatisticsService));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var path =$"{Environment.CurrentDirectory}\\log4net.config";
            builder.Logging.ClearProviders();
            builder.Logging.AddLog4Net(path);


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("AllowLocalhost4200");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
