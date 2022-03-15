/* 
BaseFluentValidator.cs
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
using FluentValidation;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A base class to provide consistent design for fluent validators.
    /// </summary>
    /// <typeparam name="TTarget">The type of the object to be validated.</typeparam>
    public abstract class BaseFluentValidator<TTarget> : AbstractValidator<TTarget>
    {
        /// <summary>
        /// Characters allowed in a VDS-NC
        /// </summary>
        protected static readonly Regex vdsAllowedCharacters = new Regex("([^a-zA-Z0-9 !@#$%&'*+/=?^_`{|}~.-])");
        /// <summary>
        /// Characters allowed in a Base64-URL encoded string
        /// </summary>
        protected static readonly Regex base64UrlCharacters = new Regex("([^a-zA-Z0-9-_=])");

        /// <summary>
        /// Internal configuration.
        /// </summary>
        protected readonly FluentValidationServiceOptions _options;

        /// <summary>
        /// A base class to provide consistent design for fluent validators.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        public BaseFluentValidator(IOptions<FluentValidationServiceOptions> options)
        {
            _options = options.Value;

            ApplyRules();
        }

        /// <summary>
        /// Configure the 'at-level' validation rules.
        /// </summary>
        protected abstract void ApplyRules();

    }

}
