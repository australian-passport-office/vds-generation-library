/* 
VdsValidator.cs
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
    /// Fluent validator for the top-level VDS object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class VdsValidator<TVdsMessage> : BaseFluentValidator<Vds<TVdsMessage>>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Fluent validator for the top-level VDS object.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        /// <param name="dataValidator">Fluent Validator for the data component.</param>
        /// <param name="signatureValdator">Fluent Validator for the signature component.</param>
        public VdsValidator(
            IOptions<FluentValidationServiceOptions> options,
            AbstractValidator<VdsData<TVdsMessage>> dataValidator,
            AbstractValidator<VdsSignature> signatureValdator
        ) : base(options)
        {
            RuleFor(x => x.Data).SetValidator(dataValidator);

            // The signature block can only be validated after signing
            // Allow for this to be disabled
            if (_options.RequireSignature)
            {
                RuleFor(x => x.Signature).SetValidator(signatureValdator);
            }
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            RuleFor(x => x.Data)
                .NotNull().WithMessage(ValidationMessages.MissingField);

            // The signature block can only be validated after signing
            // Allow for this to be disabled
            if (_options.RequireSignature)
            {
                RuleFor(x => x.Signature)
                    .NotNull().WithMessage(ValidationMessages.MissingField);
            }
        }
    }
}
