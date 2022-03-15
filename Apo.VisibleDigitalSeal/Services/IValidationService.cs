/* 
IValidationService.cs
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
    /// A service for validation that a VDS-NC object satisfies ICAO VDS-NC standard and applicaiton specific settings.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public interface IValidationService<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Assess a VDS-NC object and provided messages where it does not pass the validation rules.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>IEnumerable of validation failure messages. Null on success.</returns>
        IEnumerable<string> Validate(Vds<TVdsMessage> vds);
    }
}
