using System.Reflection;
using JetBrains.Annotations;

namespace Tp.EntityConvertor
{
    /// <summary>
    /// Pair of arg property info and describing such arg attribute.
    /// </summary>
    internal class PropertyArgumentAttributePair
    {
        public PropertyArgumentAttributePair([NotNull] PropertyInfo propertyInfo,
            [NotNull] CommandLineArgumentAttribute attribute)
        {
            PropertyInfo = propertyInfo;
            Attribute = attribute;
        }

        /// <summary>
        /// Argument property info.
        /// </summary>
        [NotNull]
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Arg meta attribute.
        /// </summary>
        [NotNull]
        public CommandLineArgumentAttribute Attribute { get; }
    }
}