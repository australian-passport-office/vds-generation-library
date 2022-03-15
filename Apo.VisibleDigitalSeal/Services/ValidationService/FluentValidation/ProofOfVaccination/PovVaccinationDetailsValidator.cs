/* 
PovVaccinationDetailsValidator.cs
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
using FluentValidation;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A validator for a VDS-NC proof of vaccination (PoV) vaccination details componenet.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class PovVaccinationDetailsValidator : BaseFluentValidator<PovVaccinationDetails>
    {
        /// <summary>
        /// A validator for a VDS-NC proof of vaccination (PoV) vaccination details componenet.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        public PovVaccinationDetailsValidator(IOptions<FluentValidationServiceOptions> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            // Date of Vaccination
            RuleFor(x => x.DateOfVaccination)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .IsDate();

            // Dose Number
            RuleFor(x => x.DoseNumber)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                // Technically, must be a number with at most 2 digits
                .InclusiveBetween(1, 99).WithMessage(ValidationMessages.OutsideValueRange);

            // Country of Vaccination
            var countryRule = RuleFor(x => x.CountryOfVaccination)
                .NotNull().WithMessage(ValidationMessages.MissingField);
            if (_options.VaccinationCountries != null && _options.VaccinationCountries.Any())
            {
                countryRule.InSet(_options.VaccinationCountries);
            }
            else
            {
                // Apply a length constraint when no countries are provided in the config
                // for an approximation of ICAO Doc 9303-3
                countryRule
                    .Length(3).WithMessage(ValidationMessages.IssuingCountryConstraints)
                    .RestrictCharacters(vdsAllowedCharacters);
            }

            // Administering Centre
            RuleFor(x => x.AdministeringCentre)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .Length(1, 20).WithMessage(ValidationMessages.OutsideLength)
                .RestrictCharacters(vdsAllowedCharacters);

            // Batch number
            RuleFor(x => x.VaccineBatchNumber)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .Length(1, 20).WithMessage(ValidationMessages.OutsideLength)
                .RestrictCharacters(vdsAllowedCharacters);

            // Additional Identifier
            var nextDoseRule = RuleFor(x => x.DueDateOfNextDose);
            if (_options.CheckDueDateOfNextDose == Optionality.Required)
            {
                nextDoseRule
                    .NotNull().WithMessage(ValidationMessages.MissingField)
                    .IsDate();
            }
            else if (_options.CheckDueDateOfNextDose == Optionality.Disallowed)
            {
                nextDoseRule
                    .Null().WithMessage(ValidationMessages.MustBeNull);
            }
            else
            {
                // Include case for when a date is not provided
                nextDoseRule.IsDate().When(x => !string.IsNullOrEmpty(x.DueDateOfNextDose));
            }
        }
    }
}