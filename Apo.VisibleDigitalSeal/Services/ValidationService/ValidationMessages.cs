/* 
ValidationMessages.cs
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

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// Set of validation messages provided with validation errors.
    /// </summary>
    public class ValidationMessages
    {
        public const string MissingField = "The field {PropertyName} is required.";
        public const string IssuingCountryConstraints = "The field {PropertyName} must be a 3-Letter country code as defined in ICAO Doc 9303-3.";
        public const string FieldContainsInvalidCharacters = "The field {PropertyName} contains invalid character(s): {InvalidCharacters}.";
        public const string InvalidPartialDateField = "The field {PropertyName} must be in one of the following formats: " + DateFormat.ISO8601Date + ", " + DateFormat.ISO8601DateYearAndMonthOnly + ", or " + DateFormat.ISO8601DateYearOnly + ".";
        public const string InvalidEqualityValue = "The field {PropertyName} has an invalid value of '{PropertyValue}'. Must equal: {ComparisonValue}";
        public const string BatchNumberIsVaccineBrand = "The field {PropertyName} has an invalid value of '{PropertyValue}', the batch number cannot match the vaccine brand name.";
        public const string InvalidDateField = "The field {PropertyName} must be in " + DateFormat.ISO8601Date + " format.";
        public const string NotInSet = "The field {PropertyName} must be one of: {AllowedSet}";
        public const string OutsideLength = "The field {PropertyName} must be between {MinLength} and {MaxLength} characters.";
        public const string OutsideValueRange = "The field {PropertyName} must be between {From} and {To}.";
        public const string MustBeNull = "The field {PropertyName} must not have a value.";
    }
}
