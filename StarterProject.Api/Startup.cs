using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterProject.Api.Common;  
using StarterProject.Api.Data;
using StarterProject.Api.Features.Users;
using StarterProject.Api.Features.Users.Dtos;
using StarterProject.Api.Helpers;
using StarterProject.Api.Infrastructure;
using StarterProject.Api.Infrastructure.ServiceCollectionExtensions;
using StarterProject.Api.Security.Policies;
using StarterProject.Api.Services;
using FluentValidation;
using StarterProject.Api.Dtos.Users;
using StarterProject.Api.Features.Exercises;
using StarterProject.Api.Features.Routines;
using StarterProject.Api.Features.Equipments;
using StarterProject.Api.Features.Exercises.Dtos;
using StarterProject.Api.Features.Routines.Dtos;
using StarterProject.Api.Features.Equipments.Dtos;
using StarterProject.Api.Features.Eqiupments.Dtos;

using StarterProject.Api.Features.Exercises.Dtos;

namespace StarterProject.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            SetupDatabase(services);
            SetupAuthentication(services);
            SetupAuthorization(services);
            SetupCustomDependencies(services);
        }

        private void SetupDatabase(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DataContext")));
        }

        private void SetupAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddJwtAuthentication(key);
            services.AddSwaggerDocumentation();
        }

        private void SetupAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.Policies.CanChangeUserRole, policy =>
                    policy.Requirements.Add(new CanChangeUserRoleRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, CanChangeUserRoleHandler>();
        }

        private void SetupCustomDependencies(IServiceCollection services)
        {
            services.AddScoped<IRoutineRepository, RoutineRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IValidator<ExerciseEditDto>, ExerciseEditDtoValidator>();
            services.AddScoped<IValidator<ExerciseGetDto>, ExerciseGetDtoValidator>();
            services.AddScoped<IValidator<ExerciseDeleteDto>, ExerciseDeleteDtoValidator>();
            services.AddScoped<IValidator<EquipmentEditDto>, EquipmentEditDtoValidator>();
            services.AddScoped<IValidator<EquipmentCreateDto>, EquipmentCreateDtoValidator>();
            services.AddScoped<IValidator<ExerciseEditDto>, ExerciseEditDtoValidator>();
            services.AddScoped<IValidator<RoutineEditDto>, RoutineEditDtoValidator>();
            services.AddScoped<IValidator<RoutineCreateDto>, RoutineCreateDtoValidator>();
            services.AddScoped<IValidator<RoutineGetDto>, RoutineGetDtoValidator>();
            services.AddScoped<IValidator<RoutineDeleteDto>, RoutineDeleteDtoValidator>();
            services.AddScoped<IValidator<EquipmentGetDto>, EquipmentGetDtoValidator>();
            services.AddScoped<IValidator<EquipmentDeleteDto>, EquipmentDeleteDtoValidator>();
            services.AddScoped<IValidator<ExerciseCreateDto>, ExerciseCreateDtoValidator>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}