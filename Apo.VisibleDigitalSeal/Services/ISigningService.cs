/* 
ISigningService.cs
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
    /// A service which digitally signed a VDS-NC Data object.
    /// </summary>
    /// <typeparam name="TVdsMessage">The type of the VDS message component.</typeparam>
    public interface ISigningService<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Sign the VDS-NC Data object and attach the signature to the VDS-NC object.
        /// </summary>
        /// <param name="vds">The VDS-NC to sign.</param>
        /// <returns>The VDS-NC with the signature object populated.</returns>
        Task<Vds<TVdsMessage>> CreateSignature(Vds<TVdsMessage> vds);
    }
}
