using System;
using JetBrains.Annotations;

namespace Tp.EntityConvertor
{
    /// <summary>
    /// Command line argument meta attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    internal class CommandLineArgumentAttribute : Attribute
    {
        public CommandLineArgumentAttribute([NotNull] string[] keys,
            [NotNull] string description,
            [NotNull] string sampleValue)
        {
            Keys = keys;
            Description = description;
            SampleValue = sampleValue;
        }

        /// <summary>
        /// Accepted command line args for this arg.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        public string[] Keys { get; }

        /// <summary>
        /// Description. Used in usage for example.
        /// </summary>
        [NotNull]
        public string Description { get; }

        /// <summary>
        /// Sample value.
        /// </summary>
        [NotNull]
        public string SampleValue { get; }
    }
}