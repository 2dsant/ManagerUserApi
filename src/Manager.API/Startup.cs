using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Manager.Services.Interfaces;
using Isopoh.Cryptography.Argon2;
using Manager.Infra.Repositories;
using Manager.Services.Services;
using Microsoft.OpenApi.Models;
using Manager.Infra.Interfaces;
using Manager.Domain.Entities;
using EscNet.IoC.Cryptography;
using Manager.API.ViewModels;
using Manager.Infra.Context;
using Manager.Services.DTO;
using EscNet.IoC.Hashers;
using Manager.API.Token;
using System.Text;
using AutoMapper;
using System;

namespace Manager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            #region JWT
            var secretKey = Configuration["Jwt:Key"];
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion
            #region Automapper
            var autoMapConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<CreateUserViewModel, UserDTO>().ReverseMap();
                cfg.CreateMap<UpdateUserViewModel, UserDTO>().ReverseMap();
            });
            
            services.AddSingleton(autoMapConfig.CreateMapper());
            #endregion 
            #region DI
            services.AddSingleton(d => Configuration);
            services.AddDbContext<ManagerContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:USER_MANAGER"]));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            #endregion
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Manager API",
                    Version = "v1",
                    Description = "API construída na serie de vídeos no canal Lucas Eschechola.",
                    Contact = new OpenApiContact
                    {
                        Name = "David Santos",
                        Email = "2dsant3@gmail.com",
                        Url = new Uri("https://linkedin.com/in/2dsant")
                    },
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor utilize Bearer <TOKEN>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });
            });
            #endregion
            #region Hash
            var config = new Argon2Config{
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                Threads = Environment.ProcessorCount,
                TimeCost = int.Parse(Configuration["Hash:TimeCost"]),
                MemoryCost = int.Parse(Configuration["Hash:MemoryCost"]),
                Lanes = int.Parse(Configuration["Hash:Lanes"]),
                HashLength = int.Parse(Configuration["Hash:HashLength"]),
                Salt = Encoding.UTF8.GetBytes(Configuration["Hash:Salt"])
            };

            services.AddArgon2IdHasher(config);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manager.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
