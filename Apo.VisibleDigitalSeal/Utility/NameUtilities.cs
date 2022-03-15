/* 
NameUtilities.cs
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

using System;
using System.Text;
using System.Linq;

namespace Apo.VisibleDigitalSeal.Utility
{
    /// <summary>
    /// Utilities for formatting names for the ICAO VDS-NC standard.
    /// </summary>
    public static class NameUtilities
    {
        private const int maxLength = 39;
        private const int maxPrimaryLength = 31;
        private const char defaultFillerCharacter = '<';


        /// <summary>
        /// Truncates a name, matching the rules provided by ICAO Doc9303-3 and Doc9303-4 for Passport Machine Readable Zone (MRZ).
        /// </summary>
        /// <param name="primaryField">The primary identification field before truncation.</param>
        /// <param name="secondaryField">The secondary identification field before truncation.</param>
        /// <param name="maxLength">The target total length of the combined name.</param>
        /// <param name="maximumPrimaryLength">When truncation, the primary identifier will be limited to this length, allowing more space for the secondary identifier.</param>
        /// <param name="fillerCharacter">The character used for filling space in the MRZ.</param>
        /// <param name="fillToEnd">If true, if the name is less than .</param>
        /// <returns>Holder name formatted and truncated as per MRZ rules.</returns>
        public static string Truncate(string primaryField, string secondaryField, int maxLength = maxLength, int maximumPrimaryLength = maxPrimaryLength, char fillerCharacter = defaultFillerCharacter, bool fillToEnd = false)
        {
            if (string.IsNullOrEmpty(primaryField) && string.IsNullOrEmpty(secondaryField))
            {
                return string.Empty;
            }

            // If no secondary field is provided, swap it for the primary field.
            if (string.IsNullOrEmpty(primaryField))
            {
                primaryField = secondaryField;
                secondaryField = null;
            }

            primaryField = primaryField.Clean(fillerCharacter);
            secondaryField = secondaryField.Clean(fillerCharacter);

            // Start with the secondary field truncated to the limit characters
            var truncatedName = new StringBuilder(new string(primaryField.Take(maximumPrimaryLength).ToArray()));
            truncatedName.Append(fillerCharacter);

            if (!string.IsNullOrEmpty(secondaryField))
            {
                truncatedName.Append(fillerCharacter);
                truncatedName.Append(secondaryField);
            }

            if (truncatedName.Length > maxLength)
            {
                var lastCharacter = truncatedName[maxLength - 1];

                if (lastCharacter == fillerCharacter)
                {
                    var remainingNames = truncatedName.ToString(maxLength, truncatedName.Length - maxLength - 1).Trim();

                    if (!string.IsNullOrEmpty(remainingNames))
                    {
                        truncatedName = new StringBuilder(truncatedName.ToString(0, maxLength - 2));
                        truncatedName.Append(fillerCharacter);
                        truncatedName.Append(remainingNames.First());
                    }
                }
            }

            return new string(truncatedName.ToString().Take(maxLength).ToArray()).Trim();
        }

        /// <summary>
        /// Remove some punctuation and make spacing consistent.
        /// </summary>
        /// <param name="input">The input string to clean.</param>
        /// <param name="fillerCharacter">The character used for filling space in the MRZ.</param>
        /// <returns>The input string with cleaning rules applied.</returns>
        private static string Clean(this string input, char fillerCharacter = defaultFillerCharacter)
        {
            var tokens = input?
                .Replace("'", string.Empty)
                .Replace("-", string.Empty)
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens == null || !tokens.Any())
            {
                return string.Empty;
            }

            return string.Join(fillerCharacter.ToString(), tokens);
        }

    }
}
