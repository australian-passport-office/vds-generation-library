/* 
SigningDiExtensions.cs
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
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Apo.VisibleDigitalSeal.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for IServiceCollection which configure signing services.
    /// </summary>
    public static class SigningDiExtensions
    {
        private static Action<HttpSigningServiceOptions> SetDefaultHttpSigningServiceOptions => (HttpSigningServiceOptions options) =>
        {
            options.SigningEndpoint = null;
            options.SigningServiceBaseUrl = null;
        };
        
        public static IServiceCollection AddVdsHttpSigningService(this IServiceCollection services, string namedConfigurationSection = "VisibleDigitalSeal:" + nameof(HttpSigningServiceOptions))
        {
            // Apply defaults
            services.Configure<HttpSigningServiceOptions>(SetDefaultHttpSigningServiceOptions);

            // Override from configuration
            services.AddOptions<HttpSigningServiceOptions>().BindConfiguration(namedConfigurationSection);

            // Inner HTTP Client
            services.AddHttpClient<ISigningServiceApiClient<PovVdsMessage>, SigningServiceApiClient<PovVdsMessage>>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var config = sp.GetRequiredService<IOptions<HttpSigningServiceOptions>>();
                    httpClient.BaseAddress = new Uri(config.Value.SigningServiceBaseUrl);
                });

            // Signing Service
            services.AddScoped<ISigningService<PovVdsMessage>, HttpSigningService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsHttpSigningService(this IServiceCollection services, Func<IHttpClientBuilder, IHttpClientBuilder> configureClient, string namedConfigurationSection = "VisibleDigitalSeal:" + nameof(HttpSigningServiceOptions))
        {
            // Apply defaults
            services.Configure<HttpSigningServiceOptions>(SetDefaultHttpSigningServiceOptions);

            // Override from configuration
            services.AddOptions<HttpSigningServiceOptions>().BindConfiguration(namedConfigurationSection);

            // Inner HTTP Client
            var builder = services.AddHttpClient<ISigningServiceApiClient<PovVdsMessage>, SigningServiceApiClient<PovVdsMessage>>();
                configureClient(builder);

            // Signing Service
            services.AddScoped<ISigningService<PovVdsMessage>, HttpSigningService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsHttpSigningService(this IServiceCollection services, Func<IHttpClientBuilder, IHttpClientBuilder> configureClient, Action<HttpSigningServiceOptions> configureOptions)
        {
            // Apply defaults
            services.Configure<HttpSigningServiceOptions>(SetDefaultHttpSigningServiceOptions);

            // Override from configuration
            services.Configure<HttpSigningServiceOptions>(configureOptions);

            // Inner HTTP Client
            var builder = services.AddHttpClient<ISigningServiceApiClient<PovVdsMessage>, SigningServiceApiClient<PovVdsMessage>>();
                configureClient(builder);

            // Signing Service
            services.AddScoped<ISigningService<PovVdsMessage>, HttpSigningService<PovVdsMessage>>();

            return services;
        }

        [Obsolete("The DemoSigningService class should not be used in production", error: false)]
        public static IServiceCollection AddVdsDemoSigningService_ForNonProduction(this IServiceCollection services)
        {
            // Signing Service
            services.AddScoped<ISigningService<PovVdsMessage>, DemoSigningService<PovVdsMessage>>();

            return services;
        }

    }
}
