/* 
PovPreProcessorService.cs
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
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Apo.VisibleDigitalSeal.Logging;

namespace Apo.VisibleDigitalSeal.Services
{
    /// <summary>
    /// A service which provides initial transformation on a VDS-NC object.
    /// </summary>
    /// <typeparam name="TVdsMessage">Type of the message component of a VDS-NC.</typeparam>
    public class PovPreProcessorService<TVdsMessage> : IPreProcessorService<TVdsMessage>
        where TVdsMessage : PovVdsMessage
    {
        protected readonly string _keyIsTruncated = "IsTruncated";
        protected readonly string _keyMaxDoses = "MaxDoses";
        protected readonly string _keyOriginalEventCount = "OriginalEventCount";
        protected readonly string _keyOriginalDoseCount = "OriginalDoseCount";
        protected readonly string _keyTruncatedEventCount = "TruncatedEventCount";
        protected readonly string _keyTruncatedDoseCount = "TruncatedDoseCount";

        /// <summary>
        /// Internal configuration.
        /// </summary>
        protected readonly PreProcessorServiceOptions _options;

        /// <summary>
        /// The string encoding service. This should amtch the service used for barcode rendering.
        /// </summary>
        protected readonly IStringEncodingService _encodingService;

        /// <summary>
        /// Logging service
        /// </summary>
        protected ILogger<PovPreProcessorService<TVdsMessage>> _logger { get; }


        /// <summary>
        /// A service which provides initial transformation on a VDS-NC object.
        /// </summary>
        /// <param name="options">Internal configuration.</param>
        /// <param name="encodingService">The string encoding service. This should amtch the service used for barcode rendering.</param>
        public PovPreProcessorService(IOptions<PreProcessorServiceOptions> options, IStringEncodingService encodingService, ILogger<PovPreProcessorService<TVdsMessage>> logger = null)
        {
            _options = options.Value;
            _encodingService = encodingService;
            _logger = logger;
        }

        /// <summary>
        /// Apply initial transforms to the VDS-NC object.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <returns>The VDS-NC object.</returns>
        public virtual Vds<TVdsMessage> PreProcess(Vds<TVdsMessage> vds) {
            var _ = new Dictionary<string, object>();
            return PreProcess(vds, ref _);
        }
        /// <summary>
        /// Apply initial transforms to the VDS-NC object.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <param name="additionalInformation">Supporting information to be provided to other pipeline components. This may be modified.</param>
        /// <returns>The VDS-NC object.</returns>
        public virtual Vds<TVdsMessage> PreProcess(Vds<TVdsMessage> vds, ref Dictionary<string, object> additionalInformation)
        {
            if (additionalInformation == null)
            {
                additionalInformation = new Dictionary<string, object>();
            }

            using (var activity = new LoggedActivity<PovPreProcessorService<TVdsMessage>>(_logger, "Apply PreProcessor transforms"))
            {
                try
                {
                    // Truncate doses using Max Dose count
                    if (_options.TruncateDosesByCount && _options.MaxDoses > 0)
                    {
                        TruncateByMaxDoses(vds, additionalInformation);
                    }

                    return vds;
                }
                catch (Exception exception)
                {
                    activity.LogException(exception);
                    throw new Exception($"An exception occurred in {nameof(PovPreProcessorService<TVdsMessage>)}.{nameof(PreProcess)}.", exception);
                }
            }

        }

        /// <summary>
        /// Determines the priority of the dose to include. Higher values are included first.
        /// </summary>
        /// <param name="events">All vaccination events in this visible digital seal.</param>
        /// <param name="vaccEvent">The vaccination event containing current dose.</param>
        /// <param name="vaccination">The current dose being accessed.</param>
        /// <returns>Integer representing the priority of the dose. Higher priority doses are included first.</returns>
        public virtual int GetDosePriority(IEnumerable<PovVaccinationEvent> events, PovVaccinationEvent vaccEvent, PovVaccinationDetails vaccination)
        {
            return vaccination.DoseNumber;
        }

        /// <summary>
        /// Store information on the results of a truncation operation.
        /// </summary>
        /// <param name="vds">The VDS-NC object.</param>
        /// <param name="additionalInformation">Additional information to be provided to later services.</param>
        /// <param name="originalEventCount">The number of vaccination events in the VDS-NC before truncation</param>
        /// <param name="originalDoseCount">The number of vaccination doses in the VDS-NC before truncation</param>
        /// <param name="truncatedEventCount">The number of vaccination events in the VDS-NC after truncation</param>
        /// <param name="truncatedDoseCount">The number of vaccination doses in the VDS-NC after truncation</param>
        public virtual void RecordTruncationData(Vds<TVdsMessage> vds, Dictionary<string, object> additionalInformation, int originalEventCount, int originalDoseCount, int truncatedEventCount, int truncatedDoseCount)
        {
            additionalInformation[_keyIsTruncated] = originalDoseCount != truncatedDoseCount;
            additionalInformation[_keyMaxDoses] = _options.MaxDoses;
            additionalInformation[_keyOriginalEventCount] = originalEventCount;
            additionalInformation[_keyOriginalDoseCount] = originalDoseCount;
            additionalInformation[_keyTruncatedEventCount] = truncatedEventCount;
            additionalInformation[_keyTruncatedDoseCount] = truncatedDoseCount;
        }

        /// <summary>
        /// Reduce the number of doses in a VDS-NC down to a maximum limit provided in the service configuration.
        /// If the number of doses is below the limit, no change is made.
        /// Doses are prioritised by the GetDosePriority function.
        /// </summary>
        /// <param name="vds">The VDS-NC object to truncate.</param>
        /// <param name="additionalInformation">Additional information. May be modified.</param>
        public virtual void TruncateByMaxDoses(Vds<TVdsMessage> vds, Dictionary<string, object> additionalInformation)
        {
            var vaccEvents = vds.Data.Message.VaccinationEvents;

            // Count total events
            var totalVaccinations = vaccEvents.Sum(x => x.VaccinationDetails.Count());

            // Check limit
            if (_options.MaxDoses > 0 && totalVaccinations > _options.MaxDoses)
            {
                // Count total vaccinations
                var totalVaccinationEvents = vaccEvents.Count();

                // Order by priority function and truncate to max
                var vaccinationsToInclude = vaccEvents
                    // Pair events and doses and sort by priority
                    .SelectMany(ve => ve.VaccinationDetails.Select(v => new { Vaccination = v, VaccinationEvent = ve }))
                    .OrderByDescending(x => GetDosePriority(vaccEvents, x.VaccinationEvent, x.Vaccination))
                    // Truncate to length
                    .Take(_options.MaxDoses)
                    // Sort and group back into events 
                    .OrderBy(x => x.Vaccination.DoseNumber)
                    .GroupBy(x => x.VaccinationEvent)
                    .Select(x =>
                    {
                        return new PovVaccinationEvent
                        {
                            DiseaseOrAgentTargeted = x.Key.DiseaseOrAgentTargeted,
                            VaccineBrand = x.Key.VaccineBrand,
                            VaccineOrProphylaxis = x.Key.VaccineOrProphylaxis,
                            VaccinationDetails = x.Select(y => y.Vaccination).ToList()
                        };
                    }).ToList();

                // Replace events in to original VDS
                vds.Data.Message.VaccinationEvents = vaccinationsToInclude;

                // Save additional information
                RecordTruncationData(
                    vds,
                    additionalInformation,
                    totalVaccinationEvents,
                    totalVaccinations,
                    vds.Data.Message.VaccinationEvents.Count,
                    vds.Data.Message.VaccinationEvents.SelectMany(ve => ve.VaccinationDetails).Count()
                );

                _logger.LogDebug($"Applied dose truncation by maximum dose count. Current max: ${vds.Data.Message.VaccinationEvents.Count}");

            }
        }
    }
}
