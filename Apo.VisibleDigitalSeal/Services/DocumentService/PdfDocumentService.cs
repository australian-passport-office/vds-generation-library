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
using Apo.VisibleDigitalSeal.Logging;
using Apo.VisibleDigitalSeal.Models;
using Apo.VisibleDigitalSeal.Models.Icao;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which renders a PDF document containing a VDS-NC, based on a HTML document.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class PdfDocumentService<TVdsMessage> : HtmlDocumentService<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// Format value included in output.
        /// </summary>
        protected readonly string pdfFormat = "pdf";

        /// <summary>
        /// Internal service configuration.
        /// </summary>
        protected PdfDocumentServiceOptions _pdfOptions;

        /// <summary>
        /// Logging service
        /// </summary>
        protected new ILogger<PdfDocumentService<TVdsMessage>> _logger { get; }

        /// <summary>
        /// A service which renders a PDF document containing a VDS-NC, based on a HTML document.
        /// </summary>
        /// <param name="options">Internal service configuration.</param>
        /// <param name="htmlDocumentService">HTML rendering service. Provides the original HTML document.</param>
        public PdfDocumentService(
            IOptions<PdfDocumentServiceOptions> pdfOptions,
            IOptions<HtmlDocumentServiceOptions> htmlOptions,
            ILogger<PdfDocumentService<TVdsMessage>> logger = null
        ): base(htmlOptions, logger)
        {
            _pdfOptions = pdfOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// Render the provided VDS information and barcode into a PDF document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <returns>A DocumentResult containing a PDF document as a string.</returns>
        public override IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode)
        {
            return RenderDocument(vds, barcode, new Dictionary<string, object>());
        }

        /// <summary>
        /// Render the provided VDS information and barcode into a PDF document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information used during document rendering.</param>
        /// <returns>A DocumentResult containing a PDF document as a UTF-8 string.</returns>
        public override IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode, Dictionary<string, object> additionalInformation)
        {
            if (additionalInformation == null)
            {
                additionalInformation = new Dictionary<string, object>();
            }

            using (var activity = new LoggedActivity<PdfDocumentService<TVdsMessage>>(_logger, "Create PDF document"))
            {
                try
                {
                    // Get HTML from the HTML service
                    var htmlString = base.RenderAsHtmlString(vds, barcode, additionalInformation);

                    // Convert to PDF
                    var converter = new HtmlToPdf();

                    // Converter options
                    ConfigurePdfRenderer(converter);

                    // Run converter
                    var pdfDoc = converter.ConvertHtmlString(htmlString);

                    // Write PDf to bytes
                    using (var stream = new MemoryStream())
                    {
                        pdfDoc.Save(stream);
                        var bytes = stream.ToArray();
                        return new DocumentResult<TVdsMessage>(bytes, pdfFormat, barcode, vds, additionalInformation);
                    }
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(PdfDocumentService<TVdsMessage>)}.{nameof(RenderDocument)}.", exception);
                }
            }
        }

        /// <summary>
        /// Setup the HTML to PDF converter.
        /// </summary>
        /// <param name="converter">The converter object to modify.</param>
        protected virtual void ConfigurePdfRenderer(HtmlToPdf converter)
        {
            converter.Options.WebPageWidth = _pdfOptions.PageWidth;
            converter.Options.KeepImagesTogether = true;
            converter.Options.KeepTextsTogether = true;

            converter.Options.CssMediaType = HtmlToPdfCssMediaType.Print;

            converter.Options.MarginTop = _pdfOptions.MarginTop;
            converter.Options.MarginRight = _pdfOptions.MarginRight;
            converter.Options.MarginBottom = _pdfOptions.MarginBottom;
            converter.Options.MarginLeft = _pdfOptions.MarginLeft;
        }
    }
}