/* 
VdsHeader.cs
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
    /// Header object for a VDS-NC.
    /// </summary>
    public class VdsHeader
    {
        /// <summary>
        /// (t) Type is set to "icao.test" for PoT (data defined by CAPSCA), "icao.vacc" for PoV(data defined by WHO).
        /// </summary>
        [JsonProperty("t", Order = 1)]
        public string Type { get; set; }

        /// <summary>
        /// (v) Version. Each of the use cases will define a version number for the structure. In case of changes in structure, the version number will be incremented.
        /// </summary>
        [JsonProperty("v", Order = 2)]
        public int Version { get; set; }

        /// <summary>
        /// (is) Issuing Country. A three letter code identifying the issuing state. The three letter code is according to Doc 9303-3.
        /// </summary>
        [JsonProperty("is", Order = 3)]
        public string IssuingCountry { get; set; }

    }
}
