/* 
PovVdsMessage.cs
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
    /// A Proof of Vaccination (PoV) message for a VDS-NC
    /// </summary>
    public class PovVdsMessage: IVdsMessage
    {
        /// <summary>
        /// (uvci) Unique Vaccination Certificate Identifier.
        /// </summary>
        [JsonProperty("uvci", Order = 1)]
        public string UVCI { get; set; }

        /// <summary>
        /// (pid) Person Identification.
        /// </summary>
        [JsonProperty("pid", Order = 2)]
        public PovPersonIdentification PersonIdentification { get; set; }

        /// <summary>
        /// (ve) Vaccination Events.
        /// </summary>
        [JsonProperty("ve", Order = 3)]
        public List<PovVaccinationEvent> VaccinationEvents { get; set; }
    }
}
