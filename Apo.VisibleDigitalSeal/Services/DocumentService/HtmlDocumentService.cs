/* 
HtmlDocumentService.cs
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
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which renders a HTML document containing a VDS-NC based on CSHTML Razor templates.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class HtmlDocumentService<TVdsMessage> : IDocumentService<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// The internal configuration for this service.
        /// </summary>
        protected HtmlDocumentServiceOptions _options { get; set; }

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<HtmlDocumentService<TVdsMessage>> _logger { get; }

        /// <summary>
        /// A service which renders a HTML document containing a VDS-NC based on CSHTML Razor templates.
        /// </summary>
        /// <param name="options">The internal configuration for this service.</param>
        public HtmlDocumentService(IOptions<HtmlDocumentServiceOptions> options, ILogger<HtmlDocumentService<TVdsMessage>> logger = null)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Render the provided VDS information and barcode into a HTML document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <returns>A DocumentResult containing a HTML document as a string.</returns>
        public virtual IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode)
            => RenderDocument(vds, barcode, new Dictionary<string, object>());

        /// <summary>
        /// Render the provided VDS information and barcode into a HTML document.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information used during document rendering.</param>
        /// <returns>A DocumentResult containing a HTML document as a UTF-8 string.</returns>
        public virtual IDocumentResult<TVdsMessage> RenderDocument(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode, Dictionary<string, object> additionalInformation)
        {
            if (additionalInformation == null)
            {
                additionalInformation = new Dictionary<string, object>();
            }

            using (var activity = new LoggedActivity<HtmlDocumentService<TVdsMessage>>(_logger, "Create HTML document"))
            {
                try
                {
                    var htmlString = RenderAsHtmlString(vds, barcode, additionalInformation);
                    var htmlBytes = System.Text.Encoding.UTF8.GetBytes(htmlString);
                    return new DocumentResult<TVdsMessage>(htmlBytes, "html", barcode, vds, additionalInformation);
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(HtmlDocumentService<TVdsMessage>)}.{nameof(RenderDocument)}.", exception);
                }
            }
        }

        /// <summary>
        /// Render a VDS-NC, barcode and supporting information to a HTML string
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information used during document rendering.</param>
        /// <returns>A HTML document as a string</returns>
        public virtual string RenderAsHtmlString(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode, Dictionary<string, object> additionalInformation)
        {
            return CompileAndRunTemplate(vds, barcode, additionalInformation);
        }

        /// <summary>
        /// Generate a deterministic key for template caching by the internal library.
        /// </summary>
        /// <param name="name">VDS type.</param>
        /// <param name="version">VDS version.</param>
        /// <returns>String template key.</returns>
        public virtual string GenerateTemplateKey(string name, int version) => $"{name}.{version}";

        /// <summary>
        /// Render the data to the template. Compile if required.
        /// </summary>
        /// <param name="vds">The VDS-NC object to represent.</param>
        /// <param name="barcode">A barcode matching the VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information used during document rendering.</param>
        /// <returns>A HTML document as a string</returns>
        protected virtual string CompileAndRunTemplate(Vds<TVdsMessage> vds, IBarcodeResult<TVdsMessage> barcode, Dictionary<string, object> additionalInformation)
        {
            // Key used to identify the cached template
            var templateKey = GenerateTemplateKey(vds.Data.Header.Type, vds.Data.Header.Version);

            // The model to pass to the CSHTML template renderer
            var model = new CertificateTemplateModel<TVdsMessage>
            {
                Barcode = barcode,
                Vds = vds,
                AdditionalInformation = additionalInformation
            };
            var viewBag = new DynamicViewBag(additionalInformation);

            // Test for cached template
            if (Engine.Razor.IsTemplateCached(templateKey, null))
            {
                return Engine.Razor.Run(templateKey, null, model, viewBag);
            }
            else
            {
                var cshtmlText = GetRazorHtmlTemplate();
                return Engine.Razor.RunCompile(cshtmlText, templateKey, null, model, viewBag);
            }
        }

        /// <summary>
        /// Get the template which will be used in document formatting
        /// </summary>
        /// <returns></returns>
        protected string  GetRazorHtmlTemplate()
        {
            string templateContent;
            // Check for a template file path
            if (!string.IsNullOrWhiteSpace(_options.TemplateFilePath))
            {
                templateContent = File.ReadAllText(_options.TemplateFilePath);
            }
            else
            {
                // Load the default template
                var assembly = typeof(Apo.VisibleDigitalSeal.Services.HtmlDocumentService<>).GetTypeInfo().Assembly;
                Stream resource = assembly.GetManifestResourceStream("Apo.VisibleDigitalSeal.Template.ProofOfVaccinationV1CertificateTemplate.cshtml");
                StreamReader reader = new StreamReader(resource);
                templateContent = reader.ReadToEnd();
            }
            return templateContent;
        }
    }
}