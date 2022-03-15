/* 
VisibleDigitalSealGenerator.cs
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

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apo.VisibleDigitalSeal.Models;
using Apo.VisibleDigitalSeal.Models.Icao;
using Apo.VisibleDigitalSeal.Services;
using System;
using Apo.VisibleDigitalSeal.Logging;

namespace Apo.VisibleDigitalSeal
{
    public class VisibleDigitalSealGenerator<TVdsMessage> : IVisibleDigitalSealGenerator<TVdsMessage>
        where TVdsMessage : IVdsMessage
    {
        /// <summary>
        /// A preprocessor service for initial data transforms.
        /// </summary>
        protected IPreProcessorService<TVdsMessage> _preProcessorService { get; }

        /// <summary>
        /// A service for validating the VDS-NC before document creation.
        /// </summary>
        protected IValidationService<TVdsMessage> _validationService { get; }

        /// <summary>
        /// A service for producing digital signatures.
        /// </summary>
        protected ISigningService<TVdsMessage> _signingService { get; }

        /// <summary>
        /// A service for rendering barcodes.
        /// </summary>
        protected IBarcodeService _barcodeService { get; }

        /// <summary>
        /// A service for rendering documents.
        /// </summary>
        protected IDocumentService<TVdsMessage> _documentService { get; }

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<VisibleDigitalSealGenerator<TVdsMessage>> _logger { get; }

        /// <summary>
        /// </summary>
        /// <param name="preProcessorService">A preprocessor service for initial data transforms.</param>
        /// <param name="validationService">A service for validating the VDS-NC before document creation.</param>
        /// <param name="signingService">A service for producing digital signatures.</param>
        /// <param name="barcodeService">A service for rendering barcodes.</param>
        /// <param name="documentService">A service for rendering documents.</param>
        public VisibleDigitalSealGenerator(
            IPreProcessorService<TVdsMessage> preProcessorService,
            IValidationService<TVdsMessage> validationService,
            ISigningService<TVdsMessage> signingService,
            IBarcodeService barcodeService,
            IDocumentService<TVdsMessage> documentService,
            ILogger<VisibleDigitalSealGenerator<TVdsMessage>> logger = null
        )
        {
            _validationService = validationService;
            _signingService = signingService;
            _barcodeService = barcodeService;
            _documentService = documentService;
            _preProcessorService = preProcessorService;
            _logger = logger;
        }

        /// <summary>
        /// Create a document for a VDS-NC.
        /// </summary>
        /// <param name="vds">The VDS-NC.</param>
        /// <param name="additionalInformation">Supporting information for document creation.</param>
        /// <returns>A document representing the VDS-NC.</returns>
        public virtual async Task<IVdsResult<TVdsMessage>> CreateVisibleDigitalSealDocument(Vds<TVdsMessage> vds, Dictionary<string, object> additionalInformation)
        {
            using (var activity = new LoggedActivity<VisibleDigitalSealGenerator<TVdsMessage>>(_logger, "Create visible digital seal document"))
            {
                try
                {
                    // Apply preprocessor transforms
                    vds = _preProcessorService.PreProcess(vds, ref additionalInformation);

                    // Validate the visual digital seal object
                    var messages = _validationService.Validate(vds);
                    if (messages == null || !messages.Any())
                    {
                        // Sign the body
                        vds = await _signingService.CreateSignature(vds);

                        // Render the barcode
                        var barcode = _barcodeService.RenderBarcode(vds);

                        // Render the document
                        var document = _documentService.RenderDocument(vds, barcode, additionalInformation);

                        return new VdsResult<TVdsMessage>(document);
                    }
                    else
                    {
                        // Invalid VDS, return messages
                        return new VdsResult<TVdsMessage>(vds, additionalInformation, messages);
                    }

                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(VisibleDigitalSealGenerator<TVdsMessage>)}.{nameof(CreateVisibleDigitalSealDocument)}.", exception);
                }
            }
        }
    }
}