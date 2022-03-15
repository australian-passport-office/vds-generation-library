/* 
DataValidator.cs
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
    /// Fluent validator for the VDS data object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class DataValidator<TVdsMessage> : BaseFluentValidator<VdsData<TVdsMessage>>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Fluent validator for the VDS data object.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        /// <param name="headerValidator">Fluent Validator for the header component.</param>
        /// <param name="messageValidator">Fluent Validator for the message component.</param>
        public DataValidator(
            IOptions<FluentValidationServiceOptions> options,
            AbstractValidator<VdsHeader> headerValidator,
            AbstractValidator<TVdsMessage> messageValidator
        ) : base(options)
        {
            RuleFor(x => x.Header).SetValidator(headerValidator);
            RuleFor(x => x.Message).SetValidator(messageValidator);
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            RuleFor(x => x.Header)
                .NotNull().WithMessage(ValidationMessages.MissingField);

            RuleFor(x => x.Message)
                .NotNull().WithMessage(ValidationMessages.MissingField);
        }
    }
}
