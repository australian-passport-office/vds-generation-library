/* 
ISigningServiceApiClient.cs
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
using System.Threading.Tasks;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// Client used to issue signing requests to remove services.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public interface ISigningServiceApiClient<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Issue a signing request to a remote service.
        /// </summary>
        /// <typeparam name="T">Type of the expected response.</typeparam>
        /// <param name="vdsString">A string representatio of a VDS-NC.</param>
        /// <returns>A deserialised response object</returns>
        Task<T> PerformSigning<T>(string vdsString);
    }
}
