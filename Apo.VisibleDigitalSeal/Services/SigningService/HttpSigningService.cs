/* 
HttpSigningService.cs
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
using Apo.VisibleDigitalSeal.Logging;
using Apo.VisibleDigitalSeal.Models.Icao;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which transmits a VDS-NC over HTTP REST to obtain a signature.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class HttpSigningService<TVdsMessage> : ISigningService<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Internal service configuration.
        /// </summary>
        protected HttpSigningServiceOptions _options;

        /// <summary>
        /// The client service used to send HTTP messages.
        /// </summary>
        protected ISigningServiceApiClient<TVdsMessage> _apiClient;

        /// <summary>
        /// The service used to encode the VDS-NC to a transmittable format.
        /// </summary>
        protected IStringEncodingService _stringEncodingService;

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<HttpSigningService<TVdsMessage>> _logger { get; }

        /// <summary>
        /// A service which transmits a VDS-NC over HTTP REST to obtain a signature.
        /// </summary>
        /// <param name="options">Internal service configuration.</param>
        /// <param name="apiClient">The client service used to send HTTP messages.</param>
        /// <param name="stringEncodingService">The service used to encode the VDS-NC to a transmittable format.</param>
        public HttpSigningService(
            IOptions<HttpSigningServiceOptions> options,
            ISigningServiceApiClient<TVdsMessage> apiClient,
            IStringEncodingService stringEncodingService,
            ILogger<HttpSigningService<TVdsMessage>> logger = null
        )
        {
            _options = options.Value;
            _apiClient = apiClient;
            _stringEncodingService = stringEncodingService;
            _logger = logger;
        }

        /// <summary>
        /// Request a signature and attach the results to the VDS-NC.
        /// </summary>
        /// <param name="vds">The VDS-NC object to sign.</param>
        /// <returns>Returns the original VDS-NC with the signature fields populated.</returns>
        public virtual async Task<Vds<TVdsMessage>> CreateSignature(Vds<TVdsMessage> vds)
        {
            using (var activity = new LoggedActivity<HttpSigningService<TVdsMessage>>(_logger, "Sign VDS message"))
            {
                try
                {
                    // Prepare request body
                    var dataString = _stringEncodingService.EncodeForSigning(vds);

                    // Send Request
                    var signature = await _apiClient.PerformSigning<VdsSignature>(dataString);

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

                    return vds;
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(HttpSigningService<TVdsMessage>)}.{nameof(CreateSignature)}.", exception);
                }
            }

        }
    }
}
