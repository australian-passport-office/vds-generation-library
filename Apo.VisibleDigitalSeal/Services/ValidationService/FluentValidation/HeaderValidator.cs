/* 
HeaderValidator.cs
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
    /// Fluent validator for the top-level VDS object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class HeaderValidator : BaseFluentValidator<VdsHeader>
    {
        /// <summary>
        /// Fluent validator for the top-level VDS object.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        public HeaderValidator(IOptions<FluentValidationServiceOptions> options) : base(options)
        {
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            // Type
            var typeRule = RuleFor(x => x.Type)
                .RestrictCharacters(vdsAllowedCharacters)
                .NotNull().WithMessage(ValidationMessages.MissingField);

            if (_options.VdsTypes != null && _options.VdsTypes.Any())
            {
                typeRule.InSet(_options.VdsTypes);
            }

            // Issuing Country
            var issuerRule = RuleFor(x => x.IssuingCountry)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(vdsAllowedCharacters);

            if (_options.IssuingCountries != null && _options.IssuingCountries.Any())
            {
                issuerRule.InSet(_options.IssuingCountries);
            }
            else
            {
                // Apply a length constraint when no countries are provided in the config
                // for an approximation of ICAO Doc 9303-3
                issuerRule
                    .Length(3).WithMessage(ValidationMessages.IssuingCountryConstraints)
                    .RestrictCharacters(vdsAllowedCharacters);
            }

            // Version
            // Until the next version of the VDS, this can only be 1
            RuleFor(x => x.Version)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .Equal(1).WithMessage(ValidationMessages.InvalidEqualityValue);
        }
    }
}

