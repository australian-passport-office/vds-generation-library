/* 
PovVaccinationEventValidator.cs
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
    /// A validator for a VDS-NC proof of vaccination (PoV) Vaccination Event componenet.
    /// </summary>
    public class PovVaccinationEventValidator : BaseFluentValidator<PovVaccinationEvent>
    {
        /// <summary>
        /// A validator for a VDS-NC proof of vaccination (PoV) Vaccination Event componenet.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        /// <param name="detailsValidator">Validator for the vaccination details components.</param>
        public PovVaccinationEventValidator(
            IOptions<FluentValidationServiceOptions> options,
            AbstractValidator<PovVaccinationDetails> detailsValidator
        ) : base(options)
        {
            // Vaccination Details
            RuleFor(x => x.VaccinationDetails).ForEach(x => x.SetValidator(detailsValidator));
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            // Vaccine or Prophylaxis
            var prophylaxisRule = RuleFor(x => x.VaccineOrProphylaxis)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(vdsAllowedCharacters)
                .Length(1, 6).WithMessage(ValidationMessages.OutsideLength);

            if (_options.VaccinesOrProphylaxes != null && _options.VaccinesOrProphylaxes.Any())
            {
                prophylaxisRule.InSet(_options.VaccinesOrProphylaxes);
            }

            // Vaccine Brand
            var brandRule = RuleFor(x => x.VaccineBrand)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(vdsAllowedCharacters);

            if (_options.VaccineBrands != null && _options.VaccineBrands.Any())
            {
                brandRule.InSet(_options.VaccineBrands);
            }

            // Disease Targetted
            var diseaseRule = RuleFor(x => x.DiseaseOrAgentTargeted)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(vdsAllowedCharacters)
                .Length(1, 6).WithMessage(ValidationMessages.OutsideLength);

            if (_options.DiseaseOrAgentTargeted != null && _options.DiseaseOrAgentTargeted.Any())
            {
                diseaseRule.InSet(_options.DiseaseOrAgentTargeted);
            }

            // Vaccination Details
            RuleFor(x => x.VaccinationDetails)
                .NotNull().WithMessage(ValidationMessages.MissingField);
        }
    }
}
