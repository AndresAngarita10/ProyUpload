using System.Text;
//using ApiFarmacia.Helpers;
//using ApiFarmacia.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiFarmacia.Extension;

public static class ApplicationServiceExtension
{
    public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()    //WithOrigins("https://domain.com")
                        .AllowAnyMethod()       //WithMethods("GET","POST)
                        .AllowAnyHeader());     //WithHeaders("accept","content-type")
            });
}