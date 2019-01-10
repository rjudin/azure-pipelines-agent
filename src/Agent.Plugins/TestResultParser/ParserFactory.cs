using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Agent.Plugins.Log.TestResultParser.Contracts;

namespace Agent.Plugins.Log.TestResultParser.Plugin
{
    public class ParserFactory
    {
        public static IEnumerable<AbstractTestResultParser> GetTestResultParsers(ITestRunManager testRunManager, ITraceLogger logger, ITelemetryDataCollector telemetry)
        {
            Assembly.LoadFrom(@"Agent.Plugins.Log.TestResultParser.Parser.dll");

            var interfaceType = typeof(AbstractTestResultParser);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => (AbstractTestResultParser)Activator.CreateInstance(x, testRunManager, logger, telemetry));
        }
    }
}
