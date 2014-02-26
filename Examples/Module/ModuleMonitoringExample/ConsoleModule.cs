using System;
using System.Linq;
using System.Threading;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Module;

namespace NDepth.Examples.Module.ModuleMonitoringExample
{
    public class ConsoleModule : ModuleBase
    {
        #region Module interface

        protected override void OnStart(string[] args)
        {
            Logger.InfoFormat(Resources.Strings.ModuleStarted, ModuleName, MachineName);
        }

        protected override void OnRun()
        {
            var from = DateTime.UtcNow;

            Monitoring.Register(Severity.None, "Monitoring Example", "Registered monitoring event with severity 'None'!");
            Monitoring.Register(Severity.Trace, "Monitoring Example", "Registered monitoring event with severity 'Trace'!");
            Monitoring.Register(Severity.Debug, "Monitoring Example", "Registered monitoring event with severity 'Debug'!");
            Monitoring.Register(Severity.Info, "Monitoring Example", "Registered monitoring event with severity 'Info'!");
            Monitoring.Register(Severity.Warning, "Monitoring Example", "Registered monitoring event with severity 'Warning'!");
            Monitoring.Register(Severity.Error, "Monitoring Example", "Registered monitoring event with severity 'Error'!");
            Monitoring.Register(Severity.ErrorWithEmail, "Monitoring Example", "Registered monitoring event with severity 'ErrorWithEmail'!");
            Monitoring.Register(Severity.ErrorWithSms, "Monitoring Example", "Registered monitoring event with severity 'ErrorWithSms'!");
            Monitoring.Register(Severity.Fatal, "Monitoring Example", "Registered monitoring event with severity 'Fatal'!");
            Monitoring.Register(Severity.Notify, "Monitoring Example", "Registered monitoring event with severity 'Notify'!");
            Monitoring.Register(Severity.NotifyWithEmail, "Monitoring Example", "Registered monitoring event with severity 'NotifyWithEmail'!");
            Monitoring.Register(Severity.NotifyWithSms, "Monitoring Example", "Registered monitoring event with severity 'NotifyWithSms'!");

            var to = DateTime.UtcNow;

            // Sleep for a while to let monitoring events to be flushed.
            Thread.Sleep(1000);

            // Fetch and show monitoring events with paging.
            FetchMonitoringEvents(from, to, 2, true);

            // Wait for user stop action.
            Console.WriteLine(Resources.Strings.ModuleStopMessage);
            Console.ReadKey();
        }

        protected override void OnStop()
        {
            Logger.InfoFormat(Resources.Strings.ModuleStopped, ModuleName, MachineName);
        }

        #endregion

        #region Utility methods

        private void FetchMonitoringEvents(DateTime from, DateTime to, int page, bool forward)
        {
            var monitoringEventsList = Monitoring.Fetch(from, to, null, null, null, null, page, forward);
            while (monitoringEventsList.Count > 0)
            {
                foreach (var monitoringEvent in monitoringEventsList)
                    Console.WriteLine(monitoringEvent.Description);

                monitoringEventsList = Monitoring.Fetch(from, to, null, null, null, monitoringEventsList.Last().Id, page, forward);
            }
        }

        #endregion
    }
}
