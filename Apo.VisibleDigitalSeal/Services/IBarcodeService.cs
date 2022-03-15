/* 
IBarcodeService.cs
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

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which renders VDS-NC objects into barcodes
    /// </summary>
    public interface IBarcodeService
    {
        /// <summary>
        /// Create a barcode from a VDS-NC object.
        /// </summary>
        /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>A barcode image encoded as bytes.</returns>
        IBarcodeResult<TVdsMessage> RenderBarcode<TVdsMessage>(Vds<TVdsMessage> vds)
            where TVdsMessage : IVdsMessage;

        /// <summary>
        /// Create a barcode from a string.
        /// </summary>
        /// <param name="barcodeContent">String content to render to barcode.</param>
        /// <returns>A barcode image encoded as bytes.</returns>
        byte[] RenderBarcode(string barcodeContent);
    }
}
