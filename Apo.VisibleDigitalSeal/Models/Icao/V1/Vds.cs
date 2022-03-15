/* 
Vds.cs
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
    /// Top level Visible Digital Seal for Non-Constrained Environments (VDS-NC) object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class Vds<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// (data) Data. Contains Header and Message. To be signed data.
        /// </summary>
        [JsonProperty("data", Order = 1)]
        public VdsData<TVdsMessage> Data { get; set; }

        /// <summary>
        /// (sig) Signature of the data object.
        /// </summary>
        [JsonProperty("sig", Order = 2)]
        public VdsSignature Signature { get; set; }
    }
}
