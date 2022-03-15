/* 
LoggingUtilities.cs
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
using System;

namespace Apo.VisibleDigitalSeal.Logging
{
    /// <summary>
    /// Class used to provide consistent log information.
    /// </summary>
    public class LoggedActivity<TType>: IDisposable
    {
        protected ILogger<TType> _logger;
        protected string _activityName;

        public LoggedActivity(ILogger<TType> logger, string activityName)
        {
            _logger = logger;
            _activityName = activityName;
            LogActivityStart();
        }

        public void LogActivityStart()
        {
            _logger?.LogInformation($"{_activityName} has started.");
        }

        public void LogActivityEnd()
        {
            _logger?.LogInformation($"{_activityName} has finished.");
        }

        public void LogException(Exception exception)
        {
            _logger?.LogError(exception, $"{_activityName} threw an exception.");
        }

        public void Dispose()
        {
            // Always log the end
            LogActivityEnd();
        }
    }
}
