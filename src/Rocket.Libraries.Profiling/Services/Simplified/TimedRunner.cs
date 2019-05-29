using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Libraries.Profiling.Services.Simplified
{
    /// <summary>
    /// This class contains methods that allow you to wrap method calls in blocks that time how long their execution takes and writes out the result to a
    /// customizable receiver.
    /// </summary>
    public class TimedRunner : ITimedRunner
    {
        private readonly IProfilingInformationTarget _profilingInformationTarget;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="profilingInformationTarget">Ideally the receiver should be injected into this class</param>
        public TimedRunner(IProfilingInformationTarget profilingInformationTarget)
        {
            if (profilingInformationTarget == null)
            {
                throw new Exception($"Concrete implementation of {nameof(IProfilingInformationTarget)} was not injected. Profiler won't work without a target");
            }
            _profilingInformationTarget = profilingInformationTarget;
        }

        /// <summary>
        /// Gets or sets whether this specific instance of the TimedRunner is enabled or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Executes an async method, returns its result and sends out information about its duration
        /// </summary>
        /// <typeparam name="TResult">The return type of the input method to be executed.</typeparam>
        /// <param name="fnAsync">The async method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        /// <returns>Result of the input method</returns>
        public async Task<TResult> TimeAsync<TResult>(Func<Task<TResult>> fnAsync, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            var actualTag = GetActualTag(tag, lineNumber, filename);
            using (new ProfilerService(a => _profilingInformationTarget.OnInformationReceived(a), actualTag, !Enabled))
            {
                return await fnAsync();
            }
        }

        /// <summary>
        /// Executes an async method, without a return value. Also sends out information about duration of the execution.
        /// </summary>
        /// <param name="fnAsync">The async method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        /// <returns>Result of the input method</returns>
        public async Task TimeAsync(Func<Task> fnAsync, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            var actualTag = GetActualTag(tag, lineNumber, filename);
            using (new ProfilerService(a => _profilingInformationTarget.OnInformationReceived(a), actualTag, !Enabled))
            {
                await fnAsync();
            }
        }

        /// <summary>
        /// Executes an synchronous method, returns its result and sends out information about its duration
        /// </summary>
        /// <typeparam name="TResult">The return type of the input method to be executed.</typeparam>
        /// <param name="fn">The synchronous method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        public TResult Time<TResult>(Func<TResult> fn, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            var actualTag = GetActualTag(tag, lineNumber, filename);
            using (new ProfilerService(a => _profilingInformationTarget.OnInformationReceived(a), actualTag, !Enabled))
            {
                return fn();
            }
        }

        /// <summary>
        /// Executes an synchronous method, without a return value. Also sends out information about duration of the execution.
        /// </summary>
        /// <param name="fn">The synchronous method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        public void Time(Action fn, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            var actualTag = GetActualTag(tag, lineNumber, filename);
            using (new ProfilerService(a => _profilingInformationTarget.OnInformationReceived(a), actualTag, !Enabled))
            {
                fn();
            }
        }

        private string GetActualTag(string userSuppliedTag, int lineNumber, string filename)
        {
            var userExplicitlySuppliedTag = !string.IsNullOrEmpty(userSuppliedTag);
            if (userExplicitlySuppliedTag)
            {
                return userSuppliedTag;
            }
            else
            {
                return $"File: {Path.GetFileName(filename)}, Line: {lineNumber}";
            }
        }
    }
}