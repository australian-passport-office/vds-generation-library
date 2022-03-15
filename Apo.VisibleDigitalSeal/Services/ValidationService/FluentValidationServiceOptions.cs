/* 
FluentValidationServiceOptions.cs
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
using System.Collections.Generic;

namespace Apo.VisibleDigitalSeal.Services
{
    public class FluentValidationServiceOptions
    {
        /// <summary>
        /// If true, validation will throw exceptions if validation fails.
        /// </summary>
        public bool ThrowExceptionOnError { get; set; }

        /// <summary>
        /// Is the signature (sig) object required during validation.
        /// </summary>
        public bool RequireSignature { get; set; }

        /// <summary>
        /// Is the Unique Identifier required during validation.
        /// </summary>
        public bool RequireUniqueIdentifier { get; set; }

        /// <summary>
        /// Is the additional identifer required, optional or disallowed.
        /// </summary>
        public Optionality CheckAdditionalIdentifier { get; set; }
        
        /// <summary>
        /// Is the date of birth required.
        /// </summary>
        public bool RequireDateOfBirth { get; set; }

        /// <summary>
        /// The character used to fill partial dates of birth.
        /// If not included, partial dates are not filled.
        /// </summary>
        public char? DateOfBirthFiller { get; set; }

        /// <summary>
        /// Is the additional identifer required, optional or disallowed.
        /// </summary>
        public Optionality CheckDueDateOfNextDose { get; set; }

        /// <summary>
        /// The permitted values for Vds.Data.Header.Type.
        /// </summary>
        public List<string> VdsTypes { get; set; }

        /// <summary>
        /// IEnumerable of permitted algorithms. Defaults to the ICAO VDS-NC standard (ES256|ES384|ES512).
        /// </summary>
        public List<string> SigningAlgorithms { get; set; }

        /// <summary>
        /// IEnumerable of countries permitted to issue VDS-NC Certificates in this application.
        /// </summary>
        public List<string> IssuingCountries { get; set; }

        /// <summary>
        /// IEnumerable of permitted Vaccines or Phophylaxes for a Proof of Vaccination certificate.
        /// Formatted as CD-11 Extension codes (http://id.who.int/icd/entity/164949870).
        /// </summary>
        public List<string> VaccinesOrProphylaxes { get; set; }

        /// <summary>
        /// IEnumerable of permitted Vaccine Brands for a Proof of Vaccination certificate.
        /// </summary>
        public List<string> VaccineBrands { get; set; }

        /// <summary>
        /// IEnumerable of permitted Disease for a Proof of Vaccination certificate.
        /// Formatted as ICD-11 codes
        /// </summary>
        public List<string> DiseaseOrAgentTargeted { get; set; }

        /// <summary>
        /// IEnumerable of permitted country codes for vaccination issuance.
        /// </summary>
        public List<string> VaccinationCountries { get; set; }
    }

}
