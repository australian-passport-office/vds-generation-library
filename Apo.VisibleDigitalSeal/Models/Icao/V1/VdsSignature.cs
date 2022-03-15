/* 
VdsSignature.cs
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
*/using Newtonsoft.Json;

namespace Apo.VisibleDigitalSeal.Models.Icao
{
    /// <summary>
    /// Signature object of a VDS-NC.
    /// </summary>
    public class VdsSignature
    {
        /// <summary>
        /// (cer) The X.509 signer certificate in base64url [RFC 4648].
        /// </summary>
        [JsonProperty("cer", Order = 2)]
        public string Certificate { get; set; }

        /// <summary>
        /// (alg) Signature Algorithm. The signature algorithm used to produce the signature.
        /// </summary>
        [JsonProperty("alg", Order = 1)]
        public string SignatureAlgo { get; set; }

        /// <summary>
        /// (sigvl) Signature value signed over the Data in base64url [RFC 4648]
        /// </summary>
        [JsonProperty("sigvl", Order = 3)]
        public string SignatureValue { get; set; }
    }
}
