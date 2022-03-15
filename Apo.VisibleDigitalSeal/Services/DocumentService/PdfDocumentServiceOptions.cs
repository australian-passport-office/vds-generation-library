/* 
PdfDocumentService.cs
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
namespace Apo.VisibleDigitalSeal.Services
{
    public class PdfDocumentServiceOptions
    {
        /// <summary>
        /// The width in pixels of the simualted browser used to render HTML to PDF.
        /// Lowering this value will enlarge the document elements in the page.
        /// </summary>
        public int PageWidth { get; set; }

        /// <summary>
        /// Top page margin in points (1/72nd inch).
        /// </summary>
        public int MarginTop { get; set; }

        /// <summary>
        /// Right page margin in points (1/72nd inch).
        /// </summary>
        public int MarginRight { get; set; }

        /// <summary>
        /// Bottom page margin in points (1/72nd inch).
        /// </summary>
        public int MarginBottom { get; set; }

        /// <summary>
        /// Left page margin in points (1/72nd inch).
        /// </summary>
        public int MarginLeft { get; set; }
    }
}
