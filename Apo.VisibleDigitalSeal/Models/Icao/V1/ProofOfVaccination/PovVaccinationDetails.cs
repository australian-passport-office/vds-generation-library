/* 
PovVaccinationDetails.cs
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
using System;

namespace Apo.VisibleDigitalSeal.Models.Icao
{
    /// <summary>
    /// Details of a vaccine dose of a Proof of Vaccination (PoV) VDS-NC
    /// </summary>
    public class PovVaccinationDetails
    {
        /// <summary>
        /// (adm) Administering Centre. Name or code of administering centre or a health authority responsible for the vaccination event.
        /// </summary>
        [JsonProperty("adm", Order = 4)]
        public string AdministeringCentre { get; set; }

        /// <summary>
        /// (lot) Identifier for the batch number of the received vaccine.
        /// </summary>
        [JsonProperty("lot", Order = 5)]
        public string VaccineBatchNumber { get; set; }

        /// <summary>
        /// (ctr) Country ofvaccination. Defined as ICAO Doc 9303-3 country code.
        /// </summary>
        [JsonProperty("ctr", Order = 3)]
        public string CountryOfVaccination { get; set; }

        /// <summary>
        /// (dvc) Date on which the vaccine was administered. ISO 8601 date without time.
        /// </summary>
        [JsonProperty("dvc", Order = 1)]
        public string DateOfVaccination { get; set; }

        /// <summary>
        /// (seq) Vaccine dose number.
        /// </summary>
        [JsonProperty("seq", Order = 2)]
        public int DoseNumber { get; set; }

        /// <summary>
        /// (dvn) Date on which the next vaccination should be administered. ISO 8601 date without time.
        /// </summary>
        [JsonProperty("dvn", Order = 6)]
        public string DueDateOfNextDose { get; set; }
    }
}
