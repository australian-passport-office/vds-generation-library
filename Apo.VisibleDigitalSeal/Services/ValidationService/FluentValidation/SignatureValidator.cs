/* 
SignatureValidator.cs
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
    /// Fluent validator for the VDS signature object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class SignatureValidator : BaseFluentValidator<VdsSignature>
    {
        /// <summary>
        /// Fluent validator for the VDS signature object.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        public SignatureValidator(IOptions<FluentValidationServiceOptions> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            var sigRule = RuleFor(x => x.SignatureAlgo)
                .NotNull().WithMessage(ValidationMessages.MissingField);

            if (_options.SigningAlgorithms != null && _options.SigningAlgorithms.Any())
            {
                sigRule.InSet(_options.SigningAlgorithms);
            }

            RuleFor(x => x.Certificate)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(base64UrlCharacters);

            RuleFor(x => x.SignatureValue)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .RestrictCharacters(base64UrlCharacters);
        }
    }
}

