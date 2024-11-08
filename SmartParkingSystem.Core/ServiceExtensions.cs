using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SmartParkingSystem.Core.AutoMappers;
using SmartParkingSystem.Core.Interfaces;
using SmartParkingSystem.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingSystem.Core
{
    public static class ServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IParkingSpotService, ParkingSpotService>();
            services.AddScoped<IParkingLotService, ParkingLotService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddValidator(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();
            service.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static void AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        }

       
    }
}
