using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Rocket.Libraries.Profiling.Models;

namespace Rocket.Libraries.Profiling.Services
{
    /// <summary>
    /// This service assists in profiling events
    /// </summary>
    public class ProfilerService : IDisposable
    {
        private Stopwatch _stopWatch;
        private Expression<Action<ProfilingInformation>> _logTarget;
        private ProfilingInformation _profilingInfo;

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="logTarget">The target action to be called at end of profiling</param>
        /// <param name="tag">A user defined tag that uniquely identifies a block being profiled</param>
        /// <param name="silent">Set this to true to ensure that the profiling results are not report.</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        public ProfilerService(Expression<Action<ProfilingInformation>> logTarget, string tag, bool silent, [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            _profilingInfo = new ProfilingInformation
            {
                Tag = tag,
                Silent = silent,
                Filename = filename,
                LineNumber = lineNumber
            };

            _logTarget = logTarget;
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        /// <summary>
        /// Called when exiting the block being profiled.
        /// </summary>
        public void Dispose()
        {
            _profilingInfo.Duration = _stopWatch.Elapsed;
            if (_profilingInfo.Silent == false)
            {
                _logTarget.Compile().Invoke(_profilingInfo);
            }
            _stopWatch.Stop();
            _stopWatch = null;
        }
    }
}