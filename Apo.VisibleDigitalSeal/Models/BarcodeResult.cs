/* 
BarcodeResult.cs
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

namespace Apo.VisibleDigitalSeal.Models
{
    /// <summary>
    /// Result object from creation of a barcode. 
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class BarcodeResult<TVdsMessage>: IBarcodeResult<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Bytes of the barcode image.
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Format of the barcode image.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// VDS-NC object which the barcode represents.
        /// </summary>
        public Vds<TVdsMessage> SourceVds { get; set; }

        /// <summary>
        /// Barcode Result Constructor.
        /// </summary>
        /// <param name="sourceVds">The VDS which the barcode represents.</param>
        /// <param name="imageData">The content (byte string) of the barcode image.</param>
        /// <param name="format">The format of the barcode image.</param>
        public BarcodeResult(Vds<TVdsMessage> sourceVds, byte[] imageData, string format)
        {
            this.SourceVds = sourceVds;
            this.Bytes = imageData;
            this.Format = format;
        }
    }
}

