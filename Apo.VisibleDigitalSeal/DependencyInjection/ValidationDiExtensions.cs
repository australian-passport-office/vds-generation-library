/* 
ValidationDiExtensions.cs
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
*/using Apo.VisibleDigitalSeal.Models.Icao;
using Apo.VisibleDigitalSeal.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Apo.VisibleDigitalSeal.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for IServiceCollection which configure validation services.
    /// </summary>
    public static class ValidationDiExtensions
    {
        private static Action<FluentValidationServiceOptions> SetDefaultFluentValidationServiceOptions => (FluentValidationServiceOptions options) =>
        {
            options.RequireSignature = false;
            options.RequireUniqueIdentifier = false;
            options.CheckAdditionalIdentifier = Optionality.Optional;
            options.RequireDateOfBirth = false;
            options.DateOfBirthFiller = null;
            options.CheckDueDateOfNextDose = Optionality.Optional;
            options.SigningAlgorithms = new List<string> { };
            options.IssuingCountries = new List<string> { };
            options.VdsTypes = new List<string> { };
            options.VaccinesOrProphylaxes = new List<string> { };
            options.VaccineBrands = new List<string> { };
            options.DiseaseOrAgentTargeted = new List<string> { };
            options.VaccinationCountries = new List<string> { };
            options.ThrowExceptionOnError = false;
        };

        public static IServiceCollection AddVdsValidationService(this IServiceCollection services)
        {
            // Apply defaults
            services.Configure<FluentValidationServiceOptions>(SetDefaultFluentValidationServiceOptions);

            // Override from configuration
            services.AddOptions<FluentValidationServiceOptions>().BindConfiguration($"VisibleDigitalSeal:{nameof(FluentValidationServiceOptions)}");

            // Services
            services.AddFluentValidators();
            services.AddScoped<IValidationService<PovVdsMessage>, FluentValidationService<PovVdsMessage>>();

            return services;
        }

        public static IServiceCollection AddVdsValidationService(this IServiceCollection services, IConfiguration namedConfigurationSection)
        {
            // Apply defaults
            services.Configure<FluentValidationServiceOptions>(SetDefaultFluentValidationServiceOptions);

            // Override from configuration
            services.Configure<FluentValidationServiceOptions>(namedConfigurationSection);

            //Services
            services.AddFluentValidators();
            services.AddScoped<IValidationService<PovVdsMessage>, FluentValidationService<PovVdsMessage>>();

            

            return services;
        }

        public static IServiceCollection AddVdsValidationService(this IServiceCollection services, Action<FluentValidationServiceOptions> configureOptions)
        {
            // Apply defaults
            services.Configure<FluentValidationServiceOptions>(SetDefaultFluentValidationServiceOptions);

            // Override from configuration
            services.Configure<FluentValidationServiceOptions>(configureOptions);

            // Service
            services.AddFluentValidators();
            services.AddScoped<IValidationService<PovVdsMessage>, FluentValidationService<PovVdsMessage>>();

            return services;
        }

        private static void AddFluentValidators(this IServiceCollection services)
        {
            // Validators
            services.AddScoped<AbstractValidator<Vds<PovVdsMessage>>, VdsValidator<PovVdsMessage>>();
            services.AddScoped<AbstractValidator<VdsSignature>, SignatureValidator>();
            services.AddScoped<AbstractValidator<VdsData<PovVdsMessage>>, DataValidator<PovVdsMessage>>();
            services.AddScoped<AbstractValidator<VdsHeader>, HeaderValidator>();
            services.AddScoped<AbstractValidator<PovVdsMessage>, PovMessageValidator<PovVdsMessage>>();
            services.AddScoped<AbstractValidator<PovPersonIdentification>, PovPersonIdentificationValidator>();
            services.AddScoped<AbstractValidator<PovVaccinationEvent>, PovVaccinationEventValidator>();
            services.AddScoped<AbstractValidator<PovVaccinationDetails>, PovVaccinationDetailsValidator>();
        }
    }
}
