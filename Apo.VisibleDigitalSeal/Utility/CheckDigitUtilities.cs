/* 
CheckDigitUtilities.cs
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
using System.Linq;

namespace Apo.VisibleDigitalSeal.Utility

{
    /// <summary>
    /// This check-digit algorithm is defined by ICAO.
    /// https://www.icao.int/publications/Documents/9303_p3_cons_en.pdf.
    /// </summary>
    public static class CheckDigitUtilities
    {
        private static readonly int[] multiplierPattern = new int[] { 7, 3, 1 };

        /// <summary>
        /// Append a check-digit to the string using the ICAO standard defined in ICAO Doc 9303-3.
        /// </summary>
        /// <param name="input">The string to append to.</param>
        /// <returns>A new string matching the original with the check-digit appended. If input is null or an empty string, returns null</returns>
        public static string WithCheckDigit(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            return $"{input}{CalculateCheckDigit(input)}";
        }

        /// <summary>
        /// Calculate a check-digit for a string using the ICAO standard defined in ICAO Doc 9303-3.
        /// </summary>
        /// <param name="input">The string.</param>
        /// <returns>A check-digit. If input is null or an empty string, returns null</returns>
        public static int? CalculateCheckDigit(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return input.Select((x, i) => GetCharacterValue(x) * GetMultiplier(i)).Sum() % 10;
        }

        private static int GetCharacterValue(char input)
        {
            if (char.IsDigit(input))
            {
                return (int)char.GetNumericValue(input);
            }

            var upper = char.ToUpper(input);
            if (upper >= 'A' && upper <= 'Z')
            {
                return upper - 55;
            }

            return 0;
        }

        private static int GetMultiplier(int index)
        {
            return multiplierPattern[index % multiplierPattern.Length];
            
        }
    }
}
