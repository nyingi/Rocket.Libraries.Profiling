using System;

namespace Rocket.Libraries.Profiling.Models
{
    /// <summary>
    /// Contains results of profiling
    /// </summary>
    public class ProfilingInformation
    {
        /// <summary>
        /// Gets or sets a user defined tag to identify the block being profiled.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The duration the profiled activity took
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether or not profiling results shall be written reported to the logger.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where the profiled method is located.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the line number on which the profiled method is located.
        /// </summary>
        public int LineNumber { get; set; }
    }
}