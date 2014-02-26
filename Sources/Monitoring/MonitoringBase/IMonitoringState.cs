using System;
using System.Collections.Generic;
using Common.Logging;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring
{
    /// <summary>Monitoring state interface provides functionality to handle components hierarchy and track their monitoring states</summary>
    public interface IMonitoringState
    {
        /// <summary>Machine name</summary>
        string MachineName { get; }
        /// <summary>Module name</summary>
        string ModuleName { get; }
        /// <summary>Component name</summary>
        string ComponentName { get; }

        /// <summary>Monitoring state severity</summary>
        Severity Severity { get; set; }

        /// <summary>Logger</summary>
        ILog Logger { get; }
        /// <summary>Monitoring</summary>
        IMonitoring Monitoring { get; }

        /// <summary>Parent monitoring state</summary>
        IMonitoringState Parent { get; }
        /// <summary>Child monitoring states</summary>
        IEnumerable<IMonitoringState> Child { get; }

        /// <summary>Create and attach a new sub-component related to the parent component in the monitoring hierarchy</summary>
        /// <param name="name">Sub-component name</param>
        /// <returns>Attached new sub-component related to the parent component</returns>
        IMonitoringStateComponent AttachComponent(string name);
        /// <summary>Create and attach new sub-component related to the parent component in the monitoring hierarchy with predefined severity</summary>
        /// <param name="name">Sub-component name</param>
        /// <param name="severity">Predefined Severity of the new sub-component</param>
        /// <returns>Attached new sub-component related to the parent component</returns>
        IMonitoringStateComponent AttachComponent(string name, Severity severity);

        /// <summary>Create and attach a new numeric performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="counterType">Type of the performance counter (default is PerformanceCounterType.NumberOfItems)</param>
        /// <returns>Attached new numeric performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachNumericCounter(string counterName, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems);
        /// <summary>Create and attach a new numeric readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="counterValue">Value of the performance counter</param>
        /// <param name="counterType">Type of the performance counter (default is PerformanceCounterType.NumberOfItems)</param>
        /// <returns>Attached new numeric readonly performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachNumericCounter(string counterName, float counterValue, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems);
        /// <summary>Create and attach a new numeric readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="counterValueFunc">Value function of the performance counter</param>
        /// <param name="counterType">Type of the performance counter (default is PerformanceCounterType.NumberOfItems)</param>
        /// <returns>Attached new numeric readonly performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachNumericCounter(string counterName, Func<float> counterValueFunc, PerformanceCounterType counterType = PerformanceCounterType.CountOfItems);

        /// <summary>Create and attach a new string performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <returns>Attached new string performance counter related to the parent component</returns>
        IPerformanceCounterString AttachStringCounter(string counterName);
        /// <summary>Create and attach a new string readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="counterValue">Value of the performance counter</param>
        /// <returns>Attached new string readonly performance counter related to the parent component</returns>
        IPerformanceCounterString AttachStringCounter(string counterName, string counterValue);
        /// <summary>Create and attach a new string readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="counterValueFunc">Value function of the performance counter</param>
        /// <returns>Attached new string readonly performance counter related to the parent component</returns>
        IPerformanceCounterString AttachStringCounter(string counterName, Func<string> counterValueFunc);

        /// <summary>Attach a new system readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <param name="counterName">Name of the performance counter</param>
        /// <returns>Attached new system readonly performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachSystemCounter(string categoryName, string counterName);
        /// <summary>Attach a new system readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="instanceName">Instance name of the performance counter</param>
        /// <returns>Attached new system readonly performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachSystemCounter(string categoryName, string counterName, string instanceName);
        /// <summary>Attach a new system readonly performance counter related to the parent component in the monitoring hierarchy</summary>
        /// <param name="machineName">Machine name of the performance counter</param>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <param name="counterName">Name of the performance counter</param>
        /// <param name="instanceName">Instance name of the performance counter</param>        
        /// <returns>Attached new system readonly performance counter related to the parent component</returns>
        IPerformanceCounterNumeric AttachSystemCounter(string machineName, string categoryName, string counterName, string instanceName);

        /// <summary>Remove self from the monitoring hierarchy</summary>
        void RemoveFromMonitoring();
    }
}
