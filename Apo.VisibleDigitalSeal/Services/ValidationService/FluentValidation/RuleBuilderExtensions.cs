/* 
RuleBuilderExtensions.cs
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
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// Extension methods to enable better use of the flient validator for specific cases.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// The maximum number of example values to show when giving validaiaton information for sets.
        /// </summary>
        private static readonly int _exampleSetSizeLimit = 5;

        /// <summary>
        /// Limit the characaters allow on a field by a regex definition
        /// </summary>
        /// <typeparam name="T">The type of the object being validated. The property validated is always a string.</typeparam>
        /// <param name="ruleBuilder">The IRuleBuilder object for the object being validated.</param>
        /// <param name="illegalCharacterExpression">A regular expression defining the characters which are NOT permitted in the string.
        /// It can be easier to use a negative pattern listing the charcter which are allowed. eg. '[^A-Za-z0-9]'</param>
        /// <returns>The IRuleBuilder for fluent style chaining.</returns>
        public static IRuleBuilderOptions<T, string> RestrictCharacters<T>(this IRuleBuilder<T, string> ruleBuilder, Regex illegalCharacterExpression)
        {
            return ruleBuilder
                .Must((parent, property, context) =>
                {

                    // Early exit for empty strings
                    if (string.IsNullOrEmpty(property))
                    {
                        return true;
                    }

                    // Regex for characters
#if NET461 || NETSTANDARD2_0
                    // Need to cast the IEnumerable to access the inner match
                    var illegalChars = illegalCharacterExpression.Matches(property).Cast<Match>();
#elif NETSTANDARD2_1
                    var illegalChars = illegalCharacterExpression.Matches(property);
#else
#error This code block does not match csproj TargetFrameworks list
#endif
                    if (!illegalChars.Any())
                    {
                        return true;
                    }
                    else
                    {
#if NET461 || NETSTANDARD2_0
                        // Need to cast the IEnumerable to access the inner values
                        context.MessageFormatter.AppendArgument("InvalidCharacters", string.Join(", ", illegalChars.SelectMany(x => x.Captures.Cast<Capture>().Select(y => $"'{y.Value}'"))));
#elif NETSTANDARD2_1
                        context.MessageFormatter.AppendArgument("InvalidCharacters", string.Join(", ", illegalChars.SelectMany(x => x.Captures.Select(y => $"'{y.Value}'"))));
#else
#error This code block does not match csproj TargetFrameworks list
#endif
                        return false;
                    }
                })
                .WithMessage(ValidationMessages.FieldContainsInvalidCharacters);
        }

        /// <summary>
        /// Validate a property against a proscriptive set of values.
        /// </summary>
        /// <typeparam name="TObject">The type of the object being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The IRuleBuilder object for the object being validated.</param>
        /// <param name="allowedValues">The list of permitted values.</param>
        /// <returns>The IRuleBuilder for fluent style chaining.</returns>
        public static IRuleBuilderOptions<TObject, TProperty> InSet<TObject, TProperty>(this IRuleBuilder<TObject, TProperty> ruleBuilder, IEnumerable<TProperty> allowedValues)
        {
            // Create a message which describes the set
            var setMessage = "";

            // If more than the limit of values are provided, truncate and provide a count for the remainder
            if (allowedValues.Count() > _exampleSetSizeLimit)
            {
                // Concatenate values to single message
                var nTopValues = string.Join(", ", allowedValues.Take(_exampleSetSizeLimit).Select(v => $"\"{v}\""));
                setMessage = $"{nTopValues} and {allowedValues.Count() - _exampleSetSizeLimit} more value(s)";
            }
            else
            {
                // Concatenate values to single message
                setMessage = string.Join(", ", allowedValues.Select(v => $"\"{v}\""));
            }

            return ruleBuilder
                .Must((property, value) => allowedValues.Contains(value))
                .WithMessage(ValidationMessages.NotInSet.Replace("{AllowedSet}", setMessage))
                .When(x => allowedValues != null && allowedValues.Any());
        }

        /// <summary>
        /// Validate that a date string is a valid format.
        /// </summary>
        /// <typeparam name="T">The type of the object being validated. The property must be a string.</typeparam>
        /// <param name="ruleBuilder">The IRuleBuilder object for the object being validated.</param>
        /// <returns>The IRuleBuilder for fluent style chaining.</returns>
        public static IRuleBuilderOptions<T, string> IsDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => DateTime.TryParseExact(x, DateFormat.ISO8601Date, CultureInfo.CurrentCulture, DateTimeStyles.None, out var _))
                .WithMessage(ValidationMessages.InvalidDateField);
        }

    }
}
