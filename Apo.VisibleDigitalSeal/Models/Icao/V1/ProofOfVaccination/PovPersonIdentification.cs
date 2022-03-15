/* 
PovPersonIdentification.cs
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
using Newtonsoft.Json;

namespace Apo.VisibleDigitalSeal.Models.Icao
{
    /// <summary>
    /// The Person Identification object of a Proof of Vaccination (PoV) VDS-NC.
    /// </summary>
    public class PovPersonIdentification
    {
        /// <summary>
        /// (dob) Date of Birth of the holder. ISO 8601 without time.
        /// </summary>
        [JsonProperty("dob", Order = 2)]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// (n) Name of the holder.
        /// </summary>
        [JsonProperty("n", Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// (sex) Sex of the holder. Format specified in Doc 9303-4.
        /// </summary>
        [JsonProperty("sex", Order = 5)]
        public string Sex { get; set; }

        /// <summary>
        /// (i) Unique Identifier. The Travel document number associated with the holder.
        /// </summary>
        [JsonProperty("i", Order = 3)]
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// (ai) Additional Identifier. An additional document number, at the discretion of the issuer.
        /// </summary>
        [JsonProperty("ai", Order = 4)]
        public string AdditionalIdentifier { get; set; }
    }
}
