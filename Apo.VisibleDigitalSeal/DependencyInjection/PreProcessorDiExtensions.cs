/* 
PreProcessorDiExtensions.cs
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
    /// Extensions for IServiceCollection which configure pre-processor services.
    /// </summary>
    public static class PreProcessorDiExtensions
    {
        private static Action<PreProcessorServiceOptions> SetDefaultPreProcessorServiceOptions => (PreProcessorServiceOptions options) =>
        {
            options.TruncateDosesByCount = false;
            options.MaxDoses = default(int);
        };

        public static IServiceCollection AddVdsPreProcessorService(this IServiceCollection services)
        {
            // Apply defaults
            services.Configure<PreProcessorServiceOptions>(SetDefaultPreProcessorServiceOptions);

            // Override from configuration
            services.AddOptions<PreProcessorServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(PreProcessorServiceOptions)}");

            services.AddScoped<IPreProcessorService<PovVdsMessage>, PovPreProcessorService<PovVdsMessage>>();

            return services;
        }
        
        public static IServiceCollection AddVdsPreProcessorService(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            // Apply defaults
            services.Configure<PreProcessorServiceOptions>(SetDefaultPreProcessorServiceOptions);

            // Override from configuration
            services.Configure<PreProcessorServiceOptions>(namedConfigurationSection);

            services.AddScoped<IPreProcessorService<PovVdsMessage>, PovPreProcessorService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsPreProcessorService(this IServiceCollection services, Action<PreProcessorServiceOptions> configureOptions)
        {
            // Apply defaults
            services.Configure<PreProcessorServiceOptions>(SetDefaultPreProcessorServiceOptions);

            // Override from configuration
            services.Configure<PreProcessorServiceOptions>(configureOptions);

            services.AddScoped<IPreProcessorService<PovVdsMessage>, PovPreProcessorService<PovVdsMessage>>();

            return services;
        }

    }
}
