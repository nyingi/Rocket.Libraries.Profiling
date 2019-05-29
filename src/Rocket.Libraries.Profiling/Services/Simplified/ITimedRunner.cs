using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Rocket.Libraries.Profiling.Services.Simplified
{
    /// <summary>
    /// This class contains methods that allow you to wrap method calls in blocks that time how long their execution takes and writes out the result to a
    /// customizable receiver.
    /// </summary>
    public interface ITimedRunner
    {
        /// <summary>
        /// Gets or sets whether this specific instance of the TimedRunner is enabled or not.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Executes an synchronous method, without a return value. Also sends out information about duration of the execution.
        /// </summary>
        /// <param name="fn">The synchronous method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        void Time(Action fn, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "");

        /// <summary>
        /// Executes an synchronous method, returns its result and sends out information about its duration
        /// </summary>
        /// <typeparam name="TResult">The return type of the input method to be executed.</typeparam>
        /// <param name="fn">The synchronous method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        TResult Time<TResult>(Func<TResult> fn, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "");

        /// <summary>
        /// Executes an async method, without a return value. Also sends out information about duration of the execution.
        /// </summary>
        /// <param name="fnAsync">The async method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        /// <returns>Result of the input method</returns>
        Task TimeAsync(Func<Task> fnAsync, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "");

        /// <summary>
        /// Executes an async method, returns its result and sends out information about its duration
        /// </summary>
        /// <typeparam name="TResult">The return type of the input method to be executed.</typeparam>
        /// <param name="fnAsync">The async method that is input for running</param>
        /// <param name="tag">Optional custom user tag. If it is not provided, one is generated for you based on the line number and filename</param>
        /// <param name="lineNumber">Compiler supplied line number. Do not provide this value yourself</param>
        /// <param name="filename">Compiler supplied filename. Do not provide this value yourself</param>
        /// <returns>Result of the input method</returns>
        Task<TResult> TimeAsync<TResult>(Func<Task<TResult>> fnAsync, string tag = "", [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "");
    }
}