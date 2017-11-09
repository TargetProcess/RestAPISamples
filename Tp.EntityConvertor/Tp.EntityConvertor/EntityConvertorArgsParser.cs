using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Tp.Core;

namespace Tp.EntityConvertor
{
    static class EntityConvertorArgsParser
    {
        public static Lazy<PropertyArgumentAttributePair[]> SupportedArgs => new Lazy<PropertyArgumentAttributePair[]>(() =>
            typeof(EntityConvertorArgs).GetProperties()
                .Select(propertyInfo => new
                {
                    PropertyInfo = propertyInfo,
                    Attribute = propertyInfo.GetCustomAttribute<CommandLineArgumentAttribute>()
                })
                .Where(meta => meta.Attribute != null)
                .Select(meta => new PropertyArgumentAttributePair
                (
                    meta.PropertyInfo,
                    meta.Attribute
                ))
                .ToArray()
        );

        private static readonly Dictionary<Type, Action<PropertyInfo, string, EntityConvertorArgs>> ArgsPropertySettersMap =
            new Dictionary<Type, Action<PropertyInfo, string, EntityConvertorArgs>>
        {
            {
                typeof(Uri), (propertyInfo, urlValue, args) =>
                {
                    try
                    {
                        propertyInfo.SetValue(args, new Uri(urlValue));
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentOutOfRangeException($"{urlValue} is not a valid URI. {ex.Message}", ex);
                    }
                }
            },
            {
                typeof(TimeSpan), (propertyInfo, timeSpanValue, args) =>
                {
                    TimeSpan parsedTimeSpanValue;
                    if (!TimeSpan.TryParse(timeSpanValue, out parsedTimeSpanValue))
                    {
                        throw new ArgumentOutOfRangeException($"{timeSpanValue} is not a valid time span.");
                    }

                    propertyInfo.SetValue(args, parsedTimeSpanValue);
                }
            }
        };

        public static Maybe<EntityConvertorArgs> ParseCommandLine([NotNull] [ItemNotNull] string[] args)
        {
            // (Key + value) * args.count
            if (args.Length != SupportedArgs.Value.Length * 2)
            {
                return Maybe<EntityConvertorArgs>.Nothing;
            }

            var typedArgs = new EntityConvertorArgs();
            var supportedArgs = SupportedArgs.Value;

            PropertyInfo expectedArgPropertyInfo = null;
            var passedArgPropertyInfos = new HashSet<PropertyInfo>();

            foreach (var arg in args)
            {
                var argMeta = supportedArgs.FirstOrDefault(supportedArg => supportedArg.Attribute.Keys.Contains(arg));

                if (expectedArgPropertyInfo != null)
                {
                    if (argMeta != null)
                    {
                        // Key in place of value.
                        return Maybe<EntityConvertorArgs>.Nothing;
                    }

                    var mayBeTypedArgs = SetPropertyValue(expectedArgPropertyInfo, arg.Trim(), typedArgs);
                    if (!mayBeTypedArgs.HasValue)
                    {
                        // Can't set key by value, incorrect value.
                        return Maybe<EntityConvertorArgs>.Nothing;
                    }

                    typedArgs = mayBeTypedArgs.Value;
                    expectedArgPropertyInfo = null;
                }
                else
                {
                    // Unknown key.
                    if (argMeta == null)
                    {
                        return Maybe<EntityConvertorArgs>.Nothing;
                    }

                    expectedArgPropertyInfo = argMeta.PropertyInfo;
                    passedArgPropertyInfos.Add(expectedArgPropertyInfo);
                }
            }

            // Not all args are passed.
            if (passedArgPropertyInfos.Count != SupportedArgs.Value.Length)
            {
                return Maybe<EntityConvertorArgs>.Nothing;
            }

            return typedArgs;
        }

        private static Maybe<EntityConvertorArgs> SetPropertyValue(
            [NotNull] PropertyInfo argPropertyInfo,
            [NotNull] string arg,
            [NotNull] EntityConvertorArgs typedArgs)
        {
            var argType = argPropertyInfo.GetGetMethod().ReturnType;

            try
            {
                if (!argType.IsArray)
                {
                    ArgsPropertySettersMap.GetValue(argType)
                        .Do(argsSetter => argsSetter(argPropertyInfo, arg, typedArgs),
                            () => argPropertyInfo.SetValue(typedArgs, Convert.ChangeType(arg, argType)));
                }
                else
                {
                    var mayBeArray = ParseArray(arg, argType);
                    if (!mayBeArray.HasValue)
                    {
                        return Maybe<EntityConvertorArgs>.Nothing;
                    }

                    argPropertyInfo.SetValue(typedArgs, mayBeArray.Value);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{arg} is not a valid {argType.Name} argument. {ex.Message}");
                return Maybe<EntityConvertorArgs>.Nothing;
            }

            return typedArgs;
        }

        private static Maybe<Array> ParseArray([NotNull] string arg, [NotNull] Type argType)
        {
            Debug.Assert(argType.IsArray, "Should be array type here.");

            var elementType = argType.GetElementType();
            if (elementType == null)
            {
                return Maybe<Array>.Nothing;
            }

            var values = arg.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => Convert.ChangeType(v.Trim(), elementType)).ToArray();
            int index = 0;
            var result = values.Aggregate(Array.CreateInstance(elementType, values.Length), (a, v) =>
            {
                a.SetValue(v, index++);
                return a;
            });

            return result;
        }
    }
}
