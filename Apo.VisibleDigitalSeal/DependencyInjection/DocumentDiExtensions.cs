/* 
DocumentDiExtensions.cs
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Apo.VisibleDigitalSeal.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for IServiceCollection which configure document services.
    /// </summary>
    public static class DocumentDiExtensions
    {
        private static Action<HtmlDocumentServiceOptions> SetDefaultHtmlDocumentServiceOptions => (HtmlDocumentServiceOptions options) =>
        {
            options.TemplateFilePath = null;
        };

        private static Action<PdfDocumentServiceOptions> SetDefaultPdfDocumentServiceOptions => (PdfDocumentServiceOptions options) =>
        {
            options.MarginTop = 30;
            options.MarginRight = 38;
            options.MarginBottom = 40;
            options.MarginLeft = 38;
            options.PageWidth = 800;
        };

        public static IServiceCollection AddVdsHtmlDocumentService(this IServiceCollection services)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);

            // Override from configuration
            services.AddOptions<HtmlDocumentServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(HtmlDocumentServiceOptions)}");

            services.AddScoped<IDocumentService<PovVdsMessage>, HtmlDocumentService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsHtmlDocumentService(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);

            // Override from configuration
            services.Configure<HtmlDocumentServiceOptions>(namedConfigurationSection);

            services.AddScoped<IDocumentService<PovVdsMessage>, HtmlDocumentService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsHtmlDocumentService(this IServiceCollection services, Action<HtmlDocumentServiceOptions> configureOptions)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);

            // Override from configuration
            services.Configure<HtmlDocumentServiceOptions>(configureOptions);

            services.AddScoped<IDocumentService<PovVdsMessage>, HtmlDocumentService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsPdfDocumentService(this IServiceCollection services)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);
            services.Configure<PdfDocumentServiceOptions>(SetDefaultPdfDocumentServiceOptions);

            // Override from configuration
            services.AddOptions<HtmlDocumentServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(HtmlDocumentServiceOptions)}");
            services.AddOptions<PdfDocumentServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(PdfDocumentServiceOptions)}");

            services.AddScoped<IDocumentService<PovVdsMessage>, PdfDocumentService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsPdfDocumentService(this IServiceCollection services, IConfiguration pdfConfigSection, IConfiguration htmlConfigSection)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);
            services.Configure<PdfDocumentServiceOptions>(SetDefaultPdfDocumentServiceOptions);

            // Override from configuration
            services.Configure<HtmlDocumentServiceOptions>(htmlConfigSection);
            services.Configure<PdfDocumentServiceOptions>(pdfConfigSection);

            services.AddScoped<IDocumentService<PovVdsMessage>, PdfDocumentService<PovVdsMessage>>();

            return services;
        }

        public class PdfDocumentServiceConfig
        {
            public Action<PdfDocumentServiceOptions> PdfConfigurator { get; set; }
            public Action<HtmlDocumentServiceOptions> HtmlConfigurator { get; set; }
        }

        public static IServiceCollection AddVdsPdfDocumentService(this IServiceCollection services, PdfDocumentServiceConfig configure)
        {
            // Apply defaults
            services.Configure<HtmlDocumentServiceOptions>(SetDefaultHtmlDocumentServiceOptions);
            services.Configure<PdfDocumentServiceOptions>(SetDefaultPdfDocumentServiceOptions);

            // Override from configuration
            services.Configure<HtmlDocumentServiceOptions>(configure.HtmlConfigurator);
            services.Configure<PdfDocumentServiceOptions>(configure.PdfConfigurator);

            services.AddScoped<IDocumentService<PovVdsMessage>, PdfDocumentService<PovVdsMessage>>();

            return services;
        }
    }
}
