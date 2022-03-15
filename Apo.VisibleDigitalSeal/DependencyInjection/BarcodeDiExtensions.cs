/* 
BarcodeDiExtensions.cs
Copyright (c) 2021, Commonwealth of Australia. vds.support@dfat.gov.au

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using Apo.VisibleDigitalSeal.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QRCoder;
using System;

namespace Apo.VisibleDigitalSeal.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for IServiceCollection which configure barcode services.
    /// </summary>
    public static class BarcodeDiExtensions
    {
        private static Action<QrCodeServiceOptions> SetDefaultQrCodeServiceOptions => (QrCodeServiceOptions options) =>
        {
            options.PixelsPerModule = 3;
            options.EccLevel = QRCodeGenerator.ECCLevel.M;
            options.DarkColor = "#000000";
            options.LightColor = "#FFFFFF";
            options.DrawBorder = false;
            options.ImageFormat = "PNG";
        };

        public static IServiceCollection AddVdsQrCodeService(this IServiceCollection services)
        {
            // Apply defaults
            services.Configure<QrCodeServiceOptions>(SetDefaultQrCodeServiceOptions);

            // Override from configuration
            services.AddOptions<QrCodeServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(QrCodeServiceOptions)}");

            services.AddScoped<IBarcodeService, QrCodeService>();

            return services;
        }

        public static IServiceCollection AddVdsQrCodeService(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            // Apply defaults
            services.Configure<QrCodeServiceOptions>(SetDefaultQrCodeServiceOptions);

            // Override from configuration
            services.Configure<QrCodeServiceOptions>(namedConfigurationSection);

            services.AddScoped<IBarcodeService, QrCodeService>();

            return services;
        }

        public static IServiceCollection AddVdsQrCodeService(this IServiceCollection services, Action<QrCodeServiceOptions> configureOptions)
        {
            // Apply defaults
            services.Configure<QrCodeServiceOptions>(SetDefaultQrCodeServiceOptions);

            // Override from configuration
            services.Configure<QrCodeServiceOptions>(configureOptions);

            services.AddScoped<IBarcodeService, QrCodeService>();

            return services;
        }


    }
}
