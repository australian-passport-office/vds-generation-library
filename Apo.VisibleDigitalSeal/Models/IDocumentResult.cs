/* 
IDocumentResult.cs
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

/// <summary>
/// The result object from the creation of a document.
/// </summary>
/// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
namespace Apo.VisibleDigitalSeal.Models
{
    public interface IDocumentResult<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// The content of the document as bytes.
        /// </summary>
        byte[] Bytes { get; set; }
        /// <summary>
        /// The format of the document.
        /// </summary>
        string Format { get; set; }
        /// <summary>
        /// The VDS-NC barcode contained in the document.
        /// </summary>
        IBarcodeResult<TVdsMessage> Barcode { get; set; }
        /// <summary>
        /// The VDS-NC represented by a docuent.
        /// </summary>
        Vds<TVdsMessage> Vds { get; set; }
        /// <summary>
        /// The additional information provided to the document renderer.
        /// </summary>
        Dictionary<string, object> AdditionalInformation { get; set; }
    }
}
