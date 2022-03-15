/* 
IPreProcessorService.cs
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
using System.Collections.Generic;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which provides initial transformation on a VDS-NC object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public interface IPreProcessorService<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Apply intiial transforms to the VDS-NC object.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>The VDS-NC object.</returns>
        Vds<TVdsMessage> PreProcess(Vds<TVdsMessage> vds);

        /// <summary>
        /// Apply intial transforms to the VDS-NC object.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information to be provided to other pipeline components.</param>
        /// <returns>The VDS-NC object.</returns>
        Vds<TVdsMessage> PreProcess(Vds<TVdsMessage> vds, ref Dictionary<string, object> additionalInformation);
    }
}
