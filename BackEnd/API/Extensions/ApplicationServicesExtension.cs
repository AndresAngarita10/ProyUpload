
using Aplicacion.UnitOfWork;
using Domain.Interfaces;

namespace API.Extensions;

public static class ApplicationServicesExtension
{

    public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()    //WithOrigins("https://domain.com")
                        .AllowAnyMethod()       //WithMethods("GET","POST)
                        .AllowAnyHeader()   //WithHeaders("accept","content-type")
                        /* .WithOrigins("http://127.0.0.1:5500") */);     //Revisar que se agrego esto
                        
            });
    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
