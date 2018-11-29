using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyIdeaPool.Data;
using Microsoft.EntityFrameworkCore;
using MyIdeaPool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MyIdeaPool.Validators;
using MyIdeaPool.ViewModels;

namespace MyIdeaPool
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
            services.Configure<AuthenticationSettings>(Configuration.GetSection("AuthenticationSettings"));
            
            services.AddDbContext<IdeaPoolContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("IdeaPoolDb"));
            });
            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<IdeaPoolContext>();

            services.Configure<IdentityOptions>(options =>{
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            });

            ConfigureDependencies(services);
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void ConfigureDependencies(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AuthenticationSettings").GetValue<string>("SymmetricSecurityKey"))),
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    };
                });
            
            services.AddSingleton(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserSignupViewModel, User>()
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.email))
                        .ForMember(dest => dest.Fullname, opt => opt.MapFrom(src => src.name));
                }).CreateMapper());
            
            services.AddTransient<IValidator<UserSignupViewModel>, UserSignupViewModelValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
/*            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = ,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = "http://localhost:8000",
            }*/
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
        }
    }
}
