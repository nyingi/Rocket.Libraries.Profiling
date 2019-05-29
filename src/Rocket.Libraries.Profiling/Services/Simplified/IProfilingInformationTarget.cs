using Rocket.Libraries.Profiling.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Profiling.Services.Simplified
{
    /// <summary>
    /// Receiver for profiling results.
    /// Provide a concrete implementation for this yourself, then configure DI to inject this for you.
    /// Ideally in the DI configuration, its scope should be a singleton.
    /// </summary>
    public interface IProfilingInformationTarget
    {
        /// <summary>
        /// Receives incoming information about a profiled method.
        /// </summary>
        /// <param name="profilingInformation">Information about a profiled method.</param>
        void OnInformationReceived(ProfilingInformation profilingInformation);
    }
}