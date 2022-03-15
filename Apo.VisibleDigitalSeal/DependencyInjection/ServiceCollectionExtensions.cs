/* 
ServiceCollectionExtensions.cs
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
using Apo.VisibleDigitalSeal.Models.Icao;
using Apo.VisibleDigitalSeal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Apo.VisibleDigitalSeal.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddVdsBarcodeOnlyDocumentService(this IServiceCollection services)
        {
            services.AddScoped<IDocumentService<PovVdsMessage>, BarcodeOnlyDocumentService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsStringEncodingService(this IServiceCollection services)
        {
            services.AddScoped<IStringEncodingService, JsonStringEncodingService>();

            return services;
        }

        public static IServiceCollection AddVdsWorkflow(this IServiceCollection services)
        {
            services.AddScoped<IVisibleDigitalSealGenerator<PovVdsMessage>, VisibleDigitalSealGenerator<PovVdsMessage>>();
           
            return services;
        }

        public static IServiceCollection AddVdsPdfWorkflow(this IServiceCollection services)
        {
            services.AddVdsValidationService();
            services.AddVdsQrCodeService();
            services.AddVdsHttpSigningService();
            services.AddVdsPdfDocumentService();
            services.AddVdsStringEncodingService();
            services.AddScoped<IVisibleDigitalSealGenerator<PovVdsMessage>, VisibleDigitalSealGenerator<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsHtmlWorkflow(this IServiceCollection services)
        {
            services.AddVdsValidationService();
            services.AddVdsQrCodeService();
            services.AddVdsHttpSigningService();
            services.AddVdsHtmlDocumentService();
            services.AddVdsStringEncodingService();
            services.AddScoped<IVisibleDigitalSealGenerator<PovVdsMessage>, VisibleDigitalSealGenerator<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsBarcodeWorkflow(this IServiceCollection services)
        {
            services.AddVdsValidationService();
            services.AddVdsQrCodeService();
            services.AddVdsHttpSigningService();
            services.AddVdsBarcodeOnlyDocumentService();
            services.AddVdsStringEncodingService();
            services.AddScoped<IVisibleDigitalSealGenerator<PovVdsMessage>, VisibleDigitalSealGenerator<PovVdsMessage>>();

            return services;
        }
    }
}
