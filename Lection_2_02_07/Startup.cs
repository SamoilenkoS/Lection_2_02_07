using Lection_2_BL;
using Lection_2_BL.Auth;
using Lection_2_BL.Services.BooksService;
using Lection_2_DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lection_2_BL.Services.AuthService;
using Lection_2_BL.Services.LibraryService;
using Lection_2_BL.Services;
using Lection_2_BL.Options;
using System.Text;
using Lection_2_BL.Services.HashService;
using Lection_2_BL.Services.SMTPService;
using System.Collections.Generic;
using Lection_2_BL.Services.EncryptionService;
using Hangfire;
using Hangfire.SqlServer;
using System;
using Lection_2_BL.Jobs;
using Lection_2_DAL.CachingSystem;
using StackExchange.Redis;

namespace Lection_2_02_07
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
            services.AddDbContext<EFCoreDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });

            services.Configure<AuthOptions>(options =>
                Configuration.GetSection(nameof(AuthOptions)).Bind(options));

            services.Configure<SmtpConfiguration>(options =>
                Configuration.GetSection(nameof(SmtpConfiguration)).Bind(options));

            services.Configure<EncryptionConfiguration>(options =>
                Configuration.GetSection(nameof(EncryptionConfiguration)).Bind(options));

            var authOptions = Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>();

            services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
                  ConnectionMultiplexer.Connect(new ConfigurationOptions
                  {
                      EndPoints =
                      {
                          $"127.0.0.1:5002"
                      },
                      AbortOnConnectFail = false,
                  }));

            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authOptions.Issuer,
                            ValidateAudience = true,
                            ValidAudience = authOptions.Audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Key)),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddScoped
                (typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheRepository, CacheMock>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<ISendingBlueSmtpService, MockSMTPService>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<CountMonitorJob>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lesson1", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });
            });
            //services.AddHostedService<TimedHostedService>();
            services.AddHangfire(configuration => configuration
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseSimpleAssemblyNameTypeSerializer()
               .UseRecommendedSerializerSettings()
               .UseSqlServerStorage(Configuration.GetConnectionString("Default"),
               new SqlServerStorageOptions
               {
                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                   QueuePollInterval = TimeSpan.Zero,
                   UseRecommendedIsolationLevel = true,
                   DisableGlobalLocks = true
               }));

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lection_2_02_07 v1"));
            }

            app.UseHttpsRedirection();
            app.UseHangfireDashboard();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
