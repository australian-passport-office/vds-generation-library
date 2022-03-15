/* 
SigningServiceApiClient.cs
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Apo.VisibleDigitalSeal.Models.Icao;
using Microsoft.Extensions.Options;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A HTTP REST client used to send signing requests on behalf of the singing service.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class SigningServiceApiClient<TVdsMessage> : ISigningServiceApiClient<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Media type (header) for transmissions.
        /// </summary>
        private string mediaType = "application/json";

        /// <summary>
        /// Logger service.
        /// </summary>
        private readonly ILogger<SigningServiceApiClient<TVdsMessage>> _logger;

        /// <summary>
        /// HTTP Client.
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Internal configuration.
        /// </summary>
        private readonly HttpSigningServiceOptions _options;

        /// <summary>
        /// A HTTP REST client used to send signing requests on behalf of the singing service.
        /// </summary>
        /// <param name="httpClient">HTTP Client used to send requests.</param>
        /// <param name="logger">Logging service.</param>
        /// <param name="options">Internal configuration.</param>
        public SigningServiceApiClient(
            HttpClient httpClient,
            IOptions<HttpSigningServiceOptions> options,
            ILogger<SigningServiceApiClient<TVdsMessage>> logger = null
        )
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Issue a signing request to a remote service.
        /// </summary>
        /// <typeparam name="T">The serialization model for the expected response.</typeparam>
        /// <param name="vdsString">A string representation of a VDS-NC.</param>
        /// <returns>A deserialized response object.</returns>
        public async Task<T> PerformSigning<T>(string vdsString)
        {
            var response = await _httpClient.PostAsync(_options.SigningEndpoint, new StringContent(vdsString, Encoding.ASCII, mediaType));
            var signedVdsString = await response.Content.ReadAsStringAsync();
            var signedVds = JsonConvert.DeserializeObject<T>(signedVdsString);

            return signedVds;
        }
    }
}