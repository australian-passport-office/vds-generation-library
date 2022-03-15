/* 
CertificateTemplateModel.cs
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

namespace Apo.VisibleDigitalSeal.Models
{
    /// <summary>
    /// This class is used as the view model for template rendering using RazorEngine.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class CertificateTemplateModel<TVdsMessage>
        where TVdsMessage: IVdsMessage
    {
        /// <summary>
        /// Barcode to include in the document.
        /// </summary>
        public IBarcodeResult<TVdsMessage> Barcode { get; set; }
        
        /// <summary>
        /// The VDS represented in the barcode and document.
        /// </summary>
        public Vds<TVdsMessage> Vds { get; set; }

        /// <summary>
        /// Additional information to provide to the document 
        /// </summary>
        public Dictionary<string, object> AdditionalInformation { get; set; }
    }
}
