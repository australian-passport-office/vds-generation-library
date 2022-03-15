/* 
QrCodeServiceOptions.cs
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
using QRCoder;
using System.Drawing.Imaging;

namespace Apo.VisibleDigitalSeal.Services
{
    public class QrCodeServiceOptions
    {
        public int PixelsPerModule { get; set; }
        /// <summary>
        /// The error correction level. Either L (7%), M (15%), Q (25%) or H (30%). Tells how much of the QR Code can get corrupted before the code isn't readable any longer.
        /// </summary>
        public QRCodeGenerator.ECCLevel EccLevel { get; set; }
        /// <summary>
        /// The color to use for the dark zones in hexidecimal format (eg. #FFFFFF)
        /// </summary>
        public string DarkColor { get; set; }
        /// <summary>
        /// The color to use for the light zones in hexidecimal format (eg. #FFFFFF)
        /// </summary>
        public string LightColor { get; set; }
        /// <summary>
        /// Draws a light color border around the QR code
        /// </summary>
        public bool DrawBorder { get; set; }
        /// <summary>
        /// The file format of the output barcode
        /// </summary>
        public string ImageFormat { get; set; }
    }
}
