/* 
FluentValidatorService.cs
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
using Apo.VisibleDigitalSeal.Logging;
using Apo.VisibleDigitalSeal.Models.Icao;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service for validation that a VDS-NC object satisfies ICAO VDS-NC standard and applicaiton specific settings.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class FluentValidationService<TVdsMessage> : IValidationService<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Internal configuration.
        /// </summary>
        protected FluentValidationServiceOptions _options;

        /// <summary>
        /// The internal validator.
        /// </summary>
        protected AbstractValidator<Vds<TVdsMessage>> _validator;

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<FluentValidationService<TVdsMessage>> _logger { get; }

        /// <summary>
        /// A service for validation that a VDS-NC object satisfies ICAO VDS-NC standard and applicaiton specific settings.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        public FluentValidationService(
            IOptions<FluentValidationServiceOptions> options,
            AbstractValidator<Vds<TVdsMessage>> vdsValidator,
            ILogger<FluentValidationService<TVdsMessage>> logger = null
        )
        {
            _options = options.Value;
            _validator = vdsValidator;
            _logger = logger;
        }

        /// <summary>
        /// Assess a VDS-NC object and provided messages where it does not pass the validation rules.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>IEnumerable of validation failure messages. Null on success.</returns>
        public virtual IEnumerable<string> Validate(Vds<TVdsMessage> vds)
        {
            using (var activity = new LoggedActivity<FluentValidationService<TVdsMessage>>(_logger, "Validate VDS"))
            {
                try
                {
                    if (_options.ThrowExceptionOnError)
                    {
                        // This will throw an exception if validation fails
                        _validator.ValidateAndThrow(vds);
                    }
                    else
                    {
                        var result = _validator.Validate(vds);
                        if (!result.IsValid)
                        {
                            // Return the error messages
                            return result.Errors.Select(err => err.ErrorMessage);
                        }
                    }

                    // No errors, return null
                    return null;
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(FluentValidationService<TVdsMessage>)}.{nameof(Validate)}.", exception);
                }
            }

        }

        /// <summary>
        /// A static method, provided to simplify the creation of a validation 'tree' when not using 
        /// dependnecy injection.
        /// </summary>
        /// <param name="options">The validation configuration.</param>
        /// <returns>A VDS Proof of Vaccination validator.</returns>
        public static VdsValidator<PovVdsMessage> GetDefaultPovValidator(IOptions<FluentValidationServiceOptions> options)
        {
            return new VdsValidator<PovVdsMessage>(
                options,
                dataValidator: new DataValidator<PovVdsMessage>(
                    options,
                    headerValidator: new HeaderValidator(options),
                    messageValidator: new PovMessageValidator<PovVdsMessage>(
                        options,
                        identificationValidator: new PovPersonIdentificationValidator(
                            options
                        ),
                        eventValidator: new PovVaccinationEventValidator(
                            options,
                            detailsValidator: new PovVaccinationDetailsValidator(options)
                        )
                    )
                ),
                signatureValdator: new SignatureValidator(options)
            );
        }
    }
}
