using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tp.EntityConvertor
{
    internal class Program
    {
        private static int Main([NotNull] [ItemNotNull] string[] args)
        {
            var mayBeTypedArgs = EntityConvertorArgsParser.ParseCommandLine(args);
            if (mayBeTypedArgs.HasValue)
            {
                return PerformConversionGeneralToType(mayBeTypedArgs.Value);
            }

            Console.Error.WriteLine("Incorrect command line params.\n");
            PrintUsage();

            return ErrorCodes.IncorrectArgs;
        }

        private static int PerformConversionGeneralToType([NotNull] EntityConvertorArgs args)
        {
            // Targetprocess enforces TLS 1.2 On-Demand, so add this to supported security protocols.
            // https://www.targetprocess.com/blog/2016/03/targetprocess-will-start-enforcing-tls-1-2-encryption/
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            // When you want to use C# 7.0 value tuple, for .NET 4.6.2 or lower, .NET Core 1.x, and .NET Standard 1.x
            // you need to install the NuGet package System.ValueTuple.
            var conversions = new List<(int GeneralId, Task<HttpResponseMessage> ConversionTask)>(args.GeneralIdsToConvert.Count());
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var httpClient = new HttpClient())
            {
                Uri conversionEndpointUri = CreateConversionEndpointUri(args);

                // Try run conversions in parallel to reduce overall time.
                foreach (var generalIdToConvert in args.GeneralIdsToConvert)
                {
                    var convertDto = new ConvertGeneralToTypeDto
                    {
                        EntityTypeId = args.EntityKindId,
                        GeneralId = generalIdToConvert
                    };

                    var conversionRequest = CreateConversionRequest(convertDto, serializerSettings);
                    var conversionTask = httpClient.PostAsync(conversionEndpointUri, conversionRequest);

                    conversions.Add((generalIdToConvert, conversionTask));
                }

                var waitTimeout = args.WaitTimeout;

                try
                {
                    bool areAllConversionsDone = Task.WaitAll(conversions.Select(conversion => conversion.ConversionTask).ToArray<Task>(), waitTimeout);
                    if (!areAllConversionsDone)
                    {
                        throw new TimeoutException($"Conversion is not completed in {waitTimeout}, ending coversion.");
                    }

                }
                catch (AggregateException ex)
                {
                    Console.Error.WriteLine(ex.Flatten());
                    return ErrorCodes.ConvertError;
                }
                catch (TimeoutException ex)
                {
                    Console.Error.WriteLine(ex);
                    return ErrorCodes.ConvertError;
                }
            }

            var conversionErrors = conversions
                .Where(conversion => !conversion.ConversionTask.Result.IsSuccessStatusCode)
                .Select(conversion =>
                {
                    var conversionTaskResponse = conversion.ConversionTask.Result;
                    var body = conversionTaskResponse.Content.ToString();
                    return $"Error during conversion of general with id {conversion.GeneralId}.{Environment.NewLine}" +
                            $"Status code: {conversionTaskResponse.StatusCode}, Body: {body}" +
                            $"{Environment.NewLine}";
                })
                .ToList();

            if (conversionErrors.Count == 0)
            {
                Console.WriteLine($"All generals with ids {string.Join(", ", args.GeneralIdsToConvert)} " +
                    "are converted successfully.");
                return ErrorCodes.Ok;
            }

            conversionErrors.ForEach(conversionError => Console.Error.WriteLine(conversionError));
            return ErrorCodes.ConvertError;
        }

        [NotNull]
        private static StringContent CreateConversionRequest([NotNull] ConvertGeneralToTypeDto convertDto,
            [NotNull] JsonSerializerSettings serializerSettings)
        {
            const string jsonMediaType = "application/json";
            var requestContent = JsonConvert.SerializeObject(convertDto, serializerSettings);

            return new StringContent(requestContent, Encoding.UTF8, jsonMediaType);
        }

        [NotNull]
        private static Uri CreateConversionEndpointUri([NotNull] EntityConvertorArgs args)
        {
            const string convertGeneralToTypeEndpointPath = "PageServices/ActionsService.asmx/ConvertGeneralToType";
            const string authenticationTokenParamTemplate = "?access_token={0}";

            string authenticatedConvertGeneralToTypeEndpointPath =
                $"{convertGeneralToTypeEndpointPath}{string.Format(authenticationTokenParamTemplate, args.AccessToken)}";

            return new Uri(args.InstanceUri, authenticatedConvertGeneralToTypeEndpointPath);
        }

        private static void PrintUsage()
        {
            var programName = Process.GetCurrentProcess().MainModule.ModuleName;

            Console.WriteLine($"Usage: {programName} [args]");
            Console.WriteLine("where args:");

            foreach (var supportedArg in EntityConvertorArgsParser.SupportedArgs.Value)
            {
                Console.WriteLine($"{string.Join(", ", supportedArg.Attribute.Keys)}\t{supportedArg.Attribute.Description}");
            }

            Console.WriteLine();
            Console.WriteLine("Samples: ");
            Console.WriteLine($"{programName} {string.Join(" ", EntityConvertorArgsParser.SupportedArgs.Value.Select(v => $"{v.Attribute.Keys.First()} {v.Attribute.SampleValue}"))}");
            Console.WriteLine($"{programName} {string.Join(" ", EntityConvertorArgsParser.SupportedArgs.Value.Select(v => $"{v.Attribute.Keys.Last()} {v.Attribute.SampleValue}"))}");
        }
    }
}
