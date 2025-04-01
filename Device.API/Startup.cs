using AspNetCore.Authentication.Basic;
using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repository;

namespace Device.API
{
    public class Startup
    {
        private IConfiguration configuration { get; set; }

        public Startup(IConfiguration cfg) {
            configuration = cfg;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            services.AddDbContext<ApplicationDbContext>(cfg => {
                cfg.UseSqlite(configuration["ConnectionStrings:DatabaseConnection"], b => b.MigrationsAssembly("Device.API"));
            });

            services.AddSwaggerGen(cfg => {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Device api"
                });

                cfg.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Description = "Paste only basic token or skip",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "basic"
                });

                cfg.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="basic"
                        },
                    },
                    new List<string>()}
                });

                var xmlFilename = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
                cfg.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            services
                .AddAuthentication(BasicDefaults.AuthenticationScheme)
                .AddBasic<BasicUserValidationService>(options => { options.Realm = "device api"; });
            services.AddAuthorization(options => {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(cfg => {
                cfg.RoutePrefix = "swagger";
                cfg.SwaggerEndpoint("v1/swagger.json", "device Api");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
