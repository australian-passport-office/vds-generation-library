/* 
IDocumentService.cs
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
using Apo.VisibleDigitalSeal.Models;
using Apo.VisibleDigitalSeal.Models.Icao;
using System.Collections.Generic;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which renders a document containing a VDS-NC.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public interface IDocumentService<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Render the provided VDS information and barcode into a document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <returns>A document. The format is determined by the implementing service.</returns>
        IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode);

        /// <summary>
        /// Render the provided VDS information and barcode into a document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <param name="additionalInformation">Additional information which supports document rendering.</param>
        /// <returns>A document. The format is determined by the implementing service.</returns>
        IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode, Dictionary<string, object> additionalInformation);
    }
}
