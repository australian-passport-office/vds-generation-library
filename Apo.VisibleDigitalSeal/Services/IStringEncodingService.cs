/* 
IStringEncodingService.cs
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
    /// <summary>
    /// A service used to create string representations of the VDS-NC for signing and barcode rendering.
    /// </summary>
    public interface IStringEncodingService
    {
        /// <summary>
        /// Serialize a VDS-NC to string for signing.
        /// </summary>
        /// <typeparam name="T">The VDS-NC Type.</typeparam>
        /// <param name="data">The VDS-NC object.</param>
        /// <returns>Serialized string.</returns>
        string EncodeForSigning<T>(T data);

        /// <summary>
        /// Serialize a VDS-NC to string for barcode rendering.
        /// </summary>
        /// <typeparam name="T">The VDS-NC Type.</typeparam>
        /// <param name="data">The VDS-NC object.</param>
        /// <returns>Serialized object as string.</returns>
        string EncodeForBarcode<T>(T data);
    }
}
