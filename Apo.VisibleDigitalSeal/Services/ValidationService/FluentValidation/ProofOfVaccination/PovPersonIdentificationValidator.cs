/* 
PovPersonIdentificationValidator.cs
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

using Apo.VisibleDigitalSeal.Constants;
using Apo.VisibleDigitalSeal.Models.Icao;
using FluentValidation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A validator for a VDS-NC proof of vaccination (Pov) Person Identification componenet.
    /// </summary>
    public class PovPersonIdentificationValidator : BaseFluentValidator<PovPersonIdentification>
    {
        /// <summary>
        /// Values for 'sex' as defined by ICAO 9303-4.
        /// </summary>
        private readonly IEnumerable<string> _allowedSexes = new string[] { "F", "M", "X" };

        private readonly string _dobPatternYearOnly;
        private readonly string _dobPatternYearMonthOnly;
        private readonly bool _useFiller;

        /// <summary>
        /// A validator for a VDS-NC proof of vaccination (Pov) Person Identification componenet.
        /// </summary>
        /// <param name="options"></param>
        public PovPersonIdentificationValidator(IOptions<FluentValidationServiceOptions> options)
            : base(options)
        {
            if (_options.DateOfBirthFiller.HasValue)
            {
                _useFiller = _options.DateOfBirthFiller.HasValue;
                _dobPatternYearMonthOnly = $"({DateFormat.ISO8601DateYearAndMonthOnlyRegex})-{_options.DateOfBirthFiller}{_options.DateOfBirthFiller}";
                _dobPatternYearOnly = $"({DateFormat.ISO8601DateYearOnlyRegex})-{_options.DateOfBirthFiller}{_options.DateOfBirthFiller}-{_options.DateOfBirthFiller}{_options.DateOfBirthFiller}";
            }
        }

        /// <summary>
        /// Configure validation rules.
        /// </summary>
        protected override void ApplyRules()
        {
            // Name
            RuleFor(x => x.Name)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .Length(1, 39).WithMessage(ValidationMessages.OutsideLength)
                .RestrictCharacters(vdsAllowedCharacters);

            // Date of Birth
            RuleFor(x => x.DateOfBirth)
                // Required only when the Unique Identifier is not provided (or by config)
                .NotNull().When(x => string.IsNullOrWhiteSpace(x.UniqueIdentifier) || _options.RequireDateOfBirth)
                // Check ISO 8601 date format with partial dates
                .Must(value =>
                {
                    // Full date
                    if (DateTime.TryParseExact(value?.ToString(), DateFormat.ISO8601Date, CultureInfo.CurrentCulture, DateTimeStyles.None, out var _))
                    {
                        return true;
                    }
                    else
                    {
                        // Test Partial dates
                        // Get the actual date component
                        string partialDateString;
                        if (_useFiller)
                        {
                            // Partial dates have filler characters
                            // Look for a year-month partial date
                            var match = Regex.Match(value?.ToString(), _dobPatternYearMonthOnly);
                            if (!match.Success)
                            {
                                // Look for a year-only partial date
                                match = Regex.Match(value?.ToString(), _dobPatternYearOnly);
                                if (!match.Success)
                                {
                                    // The date does not match any pattern
                                    // Validation fails
                                    return false;
                                }
                            }
                            partialDateString = match.Groups[1].Value;
                        }
                        else
                        {
                            // Partial dates do not have filler characters
                            // Use the string directly
                            partialDateString = value?.ToString();
                        }

                        // Test the date
                        return DateTime.TryParseExact(partialDateString, DateFormat.ISO8601DateYearAndMonthOnly, CultureInfo.CurrentCulture, DateTimeStyles.None, out var _)
                            || DateTime.TryParseExact(partialDateString, DateFormat.ISO8601DateYearOnly, CultureInfo.CurrentCulture, DateTimeStyles.None, out var _);
                    }
                })
                .WithMessage(ValidationMessages.InvalidPartialDateField);

            // Unique Identifier (Travel Document)
            var uniqueIdRule = RuleFor(x => x.UniqueIdentifier);

            if (_options.RequireUniqueIdentifier)
            {
                uniqueIdRule.NotNull().WithMessage(ValidationMessages.MissingField);
            }

            uniqueIdRule
                .Length(1, 11).WithMessage(ValidationMessages.OutsideLength)
                .RestrictCharacters(vdsAllowedCharacters);

            // Additional Identifier
            var additionalIdRule = RuleFor(x => x.AdditionalIdentifier);

            if (_options.CheckAdditionalIdentifier == Optionality.Required)
            {
                additionalIdRule
                    .NotNull().WithMessage(ValidationMessages.MissingField)
                    .Length(1, 24).WithMessage(ValidationMessages.OutsideLength)
                    .RestrictCharacters(vdsAllowedCharacters);
            }
            else if (_options.CheckAdditionalIdentifier == Optionality.Disallowed)
            {
                additionalIdRule
                    .Null().WithMessage(ValidationMessages.MustBeNull);
            }
            else
            {
                additionalIdRule
                    .Length(1, 24).WithMessage(ValidationMessages.OutsideLength)
                    .RestrictCharacters(vdsAllowedCharacters);
            }

            // Sex (ICAO Doc 9303-4 Section 4.1.1.1)
            RuleFor(x => x.Sex)
                .NotNull().WithMessage(ValidationMessages.MissingField)
                .InSet(_allowedSexes);
        }
    }
}

