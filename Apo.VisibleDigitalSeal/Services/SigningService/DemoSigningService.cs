/* 
DemoSigningService.cs
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
using Apo.VisibleDigitalSeal.Models.Icao;
using System;
using System.Threading.Tasks;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which transmits a VDS-NC over HTTP REST to obtain a signature.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    [Obsolete("The DemoSigningService class should not be used in production", error: false)]
    public class DemoSigningService<TVdsMessage> : ISigningService<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {

        /// <summary>
        /// A demonstration service which attaches a demonstration signature to a VDS-NC.
        /// Do not use this in production.
        /// </summary>
        public DemoSigningService()
        {
        }

        /// <summary>
        /// Attach a demonstration signature to a VDS-NC.
        /// Do not use this in production.
        /// </summary>
        /// <param name="vds">The VDS-NC object to sign.</param>
        /// <returns>Returns the original VDS-NC with the signature fields populated.</returns>
        public virtual Task<Vds<TVdsMessage>> CreateSignature(Vds<TVdsMessage> vds)
        {
            // Demo Object
            var signature = new VdsSignature()
            {
                Certificate = "DEMONSTRATION_CERTIFICATE_ADDED_BY_DEMO_SIGNING_SERVICE",
                SignatureAlgo = "DEMONSTRATION_ALGORITHM_ADDED_BY_DEMO_SIGNING_SERVICE",
                SignatureValue = "DEMONSTRATION_VALUE_ADDED_BY_DEMO_SIGNING_SERVICE"
            };
            
            // Copy only the required information
            // The signature object may have been overridden or may contain additional information which should be preserved
            if (vds.Signature != null)
            {
                vds.Signature.Certificate = signature.Certificate;
                vds.Signature.SignatureAlgo = signature.SignatureAlgo;
                vds.Signature.SignatureValue = signature.SignatureValue;
            }
            else
            {
                vds.Signature = signature;
            }

            return Task.FromResult(vds);
        }
    }
}
