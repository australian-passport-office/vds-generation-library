/* 
PovMessageValidator.cs
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

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A validator for a VDS-NC proof of vaccination (PoV) message.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class PovMessageValidator<TVdsMessage> : BaseFluentValidator<TVdsMessage>
        where TVdsMessage: PovVdsMessage
    {

        /// <summary>
        /// A validator for a VDS-NC proof of vaccination message.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        /// <param name="identificationValidator">Validator for the personal identification component.</param>
        /// <param name="eventValidator">Validator for the vaccination event components.</param>
        public PovMessageValidator(
            IOptions<FluentValidationServiceOptions> options,
            AbstractValidator<PovPersonIdentification> identificationValidator,
            AbstractValidator<PovVaccinationEvent> eventValidator
        ) : base (options)
        {
            RuleFor(x => x.PersonIdentification).SetValidator(identificationValidator);

            RuleFor(x => x.VaccinationEvents).ForEach(x => x.SetValidator(eventValidator));
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            RuleFor(x => x.UVCI)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .Length(1, 12).WithMessage(ValidationMessages.OutsideLength)
                .RestrictCharacters(vdsAllowedCharacters);

            RuleFor(x => x.PersonIdentification)
                .NotNull().WithMessage(ValidationMessages.MissingField);

            RuleFor(x => x.VaccinationEvents)
                .NotNull().WithMessage(ValidationMessages.MissingField);
        }
    }
}

