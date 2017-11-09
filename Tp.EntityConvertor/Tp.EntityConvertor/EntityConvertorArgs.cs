using System;
using System.Collections.Generic;

namespace Tp.EntityConvertor
{
    /// <summary>
    /// Command line arguments.
    /// </summary>
    internal class EntityConvertorArgs
    {
        /// <summary>
        /// Base url to the Targetprocess instance where generals are converted.
        /// </summary>
        [CommandLineArgument(new [] { "-u", "--instance_url" }, 
            description: "Base url to the Targetprocess instance where generals are converted.",
            sampleValue: "https://md5.tpondemand.com")]
        public Uri InstanceUri { get; set; }

        /// <summary>
        /// Access token for TP access.
        /// </summary>
        [CommandLineArgument(new[] { "-t", "--access_token" },
            description: "Access token for access to Targetprocess instance. See Targetprocess / My profile / Access Tokens.",
            sampleValue: "MSAd1j2s31a54dk567fkidsa5iksa5l67asdfll56f7asdvcrfsadfla567sdfsakl576df5asd5fmaa==")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Timeout to wait for conversion to complete.
        /// </summary>
        [CommandLineArgument(new[] { "-time", "--timeout" },
            description: "Time to wait for conversion to complete (in HH:mm:ss[.FFFF] format).",
            sampleValue: "0:5:30")]
        public TimeSpan WaitTimeout { get; set; }

        /// <summary>
        /// Entity kind id to convert entity to.
        /// </summary>
        [CommandLineArgument(new[] { "-ekid", "--entity_kind_id" },
            description: "Entity kind id to convert general to.",
            sampleValue: "12")]
        public int EntityKindId { get; set; }

        /// <summary>
        /// Ids of generals to convert to type with entity kind id <see cref="EntityKindId"/>.
        /// </summary>
        [CommandLineArgument(new[] { "-ids", "--general_ids" },
            description: "Separated by comma array of general's ids to convert.",
            sampleValue: "123,451,898")]
        public int[] GeneralIdsToConvert { get; set; }
    }
}