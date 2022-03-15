/* 
PovVaccinationEvents.cs
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
using System.Collections.Generic;

namespace Apo.VisibleDigitalSeal.Models.Icao
{
    /// <summary>
    /// A vaccination event of a Proof of Vaccination (PoV) VDS-NC.
    /// </summary>
    public class PovVaccinationEvent
    {
        /// <summary>
        /// (dis) Disease or Agent Targetted. ICD-11.
        /// </summary>
        [JsonProperty("dis", Order = 3)]
        public string DiseaseOrAgentTargeted { get; set; }

        /// <summary>
        /// (des) Vaccine / Prophylaxis. ICD-11 Extension code.
        /// </summary>
        [JsonProperty("des", Order = 1)]
        public string VaccineOrProphylaxis { get; set; }

        /// <summary>
        /// (nam) Vaccine Brand. 
        /// </summary>
        [JsonProperty("nam", Order = 2)]
        public string VaccineBrand { get; set; }

        /// <summary>
        /// (vd) Vaccination Details in this event.
        /// </summary>
        [JsonProperty("vd", Order = 4)]
        public List<PovVaccinationDetails> VaccinationDetails { get; set; }
    }
}
