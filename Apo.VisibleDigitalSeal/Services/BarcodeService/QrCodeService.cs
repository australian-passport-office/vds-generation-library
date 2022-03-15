/* 
QrCodeService.cs
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
using QRCoder;
using System;
using System.Drawing.Imaging;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which creates QR Codes.
    /// </summary>
    public class QrCodeService : IBarcodeService
    {
        /// <summary>
        /// The internal configuration for this service.
        /// </summary>
        protected QrCodeServiceOptions _options;

        /// <summary>
        /// The service used to transform a VDS-NC object into a string format.
        /// </summary>
        protected IStringEncodingService _stringEncodingService;

        /// <summary>
        /// The output image format for QR Codes.
        /// </summary>
        protected ImageFormat _imageFormat;

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<QrCodeService> _logger { get; }

        /// <summary>
        /// A service which create QR Codes.
        /// </summary>
        /// <param name="options">The internal configuration for this service.</param>
        /// <param name="stringEncodingService">The service used to transform a VDS-NC object into a string format.</param>
        public QrCodeService(IOptions<QrCodeServiceOptions> options, IStringEncodingService stringEncodingService, ILogger<QrCodeService> logger = null)
        {
            _options = options.Value;
            _stringEncodingService = stringEncodingService;
            _imageFormat = ParseImageFormat(_options.ImageFormat);
            _logger = logger;
        }

        /// <summary>
        /// Create a barcode from a VDS-NC object.
        /// </summary>
        /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>A barcode image encoded as bytes.</returns>
        public virtual IBarcodeResult<TVdsMessage> RenderBarcode<TVdsMessage>(Vds<TVdsMessage> vds)
            where TVdsMessage : IVdsMessage
        {
            using (var activity = new LoggedActivity<QrCodeService>(_logger, "Create QR code"))
            {
                try
                {
                    // Encode the VDS as a string
                    var barcodeContent = _stringEncodingService.EncodeForBarcode(vds);
                    // Render to image
                    var barcodeBytes = RenderBarcode(barcodeContent);
                    return new BarcodeResult<TVdsMessage>(vds, barcodeBytes, _imageFormat.ToString());
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(QrCodeService)}.{nameof(RenderBarcode)}.", exception);
                }
            }
        }

        /// <summary>
        /// Create a barcode from a string.
        /// </summary>
        /// <param name="barcodeContent">String content to render to barcode.</param>
        /// <returns>A barcode image encoded as bytes.</returns>
        public virtual byte[] RenderBarcode(string barcodeContent)
        {
            // Prep QR code library
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(barcodeContent, _options.EccLevel);
            var qrCode = new QRCode(qrCodeData);

            // Render to bitmap
            var bitmap = qrCode.GetGraphic(_options.PixelsPerModule, _options.DarkColor, _options.LightColor, _options.DrawBorder);

            // Convert the bitmap to specified format
            using (var barcodeStream = new System.IO.MemoryStream())
            {
                bitmap.Save(barcodeStream, _imageFormat);
                return barcodeStream.ToArray();
            }
        }

        /// <summary>
        /// Parses a string image format into the library specific format string-enum implementation.
        /// </summary>
        /// <param name="imageFormat">The string name/type of the image format.</param>
        /// <returns>An ImageFormat matching the string format presented.</returns>
        /// <exception cref="NotSupportedException">If the input format cannot be parsed.</exception>
        protected virtual ImageFormat ParseImageFormat(string imageFormat)
        {
            switch (imageFormat.ToLowerInvariant())
            {
                case "bmp": return ImageFormat.Bmp;
                case "mef": return ImageFormat.Emf;
                case "wmf": return ImageFormat.Wmf;
                case "gif": return ImageFormat.Gif;
                case "jpeg": return ImageFormat.Jpeg;
                case "jpg": return ImageFormat.Jpeg;
                case "png": return ImageFormat.Png;
                case "tiff": return ImageFormat.Tiff;
                case "exif": return ImageFormat.Exif;
                case "icon": return ImageFormat.Icon;
                case "ico": return ImageFormat.Icon;
                default: throw new NotSupportedException($"{nameof(QrCodeService)} does not support the provided image format '{imageFormat}'");
            }
        }
    }
}
