using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Monitoring.Core
{
    public class PerformanceCounterSystem : PerformanceCounterBase, IPerformanceCounterNumeric
    {
        private readonly PerformanceCounter _system;

        internal PerformanceCounterSystem(PerformanceCounter system, Severity severity, IMonitoring monitoring)
            : base(true, GetCounterType(system.CounterType), GetCounterName(system.MachineName, system.CategoryName, system.CounterName, system.InstanceName), severity, monitoring)
        {
            _system = system;
            UpdateInternal();
        }

        internal PerformanceCounterSystem(PerformanceCounter system, Severity severity, IMonitoringState parent)
            : base(true, GetCounterType(system.CounterType), GetCounterName(system.MachineName, system.CategoryName, system.CounterName, system.InstanceName), severity, parent)
        {
            _system = system;
            UpdateInternal();
        }

        #region Public properties

        /// <summary>Machine name of the system performance counter</summary>
        public string SystemMachineName { get { return _system.MachineName; } }
        /// <summary>Category name of the system performance counter</summary>
        public string SystemCategoryName { get { return _system.CategoryName; } }
        /// <summary>Name of the system performance counter</summary>
        public string SystemCounterName { get { return _system.CounterName; } }
        /// <summary>Instance of the system performance counter</summary>
        public string SystemInstanceName { get { return _system.InstanceName; } }

        #endregion

        #region Public static methods

        /// <summary>Get the list of system performance counters categories for the current machine</summary>
        /// <returns>Categories list of system performance counters for the current machine</returns>
        public static IEnumerable<string> GetSystemCounterCategories()
        {
            return PerformanceCounterCategory.GetCategories().Select(pcc => pcc.CategoryName);
        }

        /// <summary>Get the list of system performance counters categories for the given machine</summary>
        /// <param name="machineName">Machine name of the performance counter</param>
        /// <returns>Categories list of system performance counters for the given machine</returns>
        public static IEnumerable<string> GetSystemCounterCategories(string machineName)
        {
            return PerformanceCounterCategory.GetCategories(machineName).Select(pcc => pcc.CategoryName);
        }

        /// <summary>Get the list of the system performance counter instances for the given category and the current machine</summary>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <returns>Instances list of the system performance counter for the given category and the current machine</returns>
        public static IEnumerable<string> GetSystemCounterInstances(string categoryName)
        {
            // Check if the required category exists.
            if (!PerformanceCounterCategory.Exists(categoryName))
                return Enumerable.Empty<string>();

            var category = new PerformanceCounterCategory(categoryName);

            return category.GetInstanceNames();
        }

        /// <summary>Get the list of the system performance counter instances for the given category and the given machine</summary>
        /// <param name="machineName">Machine name of the performance counter</param>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <returns>Instances list of the system performance counter for the given category and the given machine</returns>
        public static IEnumerable<string> GetSystemCounterInstances(string machineName, string categoryName)
        {
            // Check if the required category exists.
            if (!PerformanceCounterCategory.Exists(categoryName, machineName))
                return Enumerable.Empty<string>();

            var category = new PerformanceCounterCategory(categoryName, machineName);

            return category.GetInstanceNames();
        }

        /// <summary>Get the list of system performance counters for the given category and the current machine</summary>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <returns>List of system performance counters for the given category and the current machine</returns>
        public static IEnumerable<string> GetSystemCounterNames(string categoryName)
        {
            // Check if the required category exists.
            if (!PerformanceCounterCategory.Exists(categoryName))
                return Enumerable.Empty<string>();

            var category = new PerformanceCounterCategory(categoryName);
            
            return (category.CategoryType == PerformanceCounterCategoryType.SingleInstance) ? category.GetCounters().Select(pc => pc.CounterName) : new HashSet<string>(category.GetInstanceNames().SelectMany(category.GetCounters).Select(pc => pc.CounterName));
        }

        /// <summary>Get the list of system performance counters for the given category and the given machine</summary>
        /// <param name="machineName">Machine name of the performance counter</param>
        /// <param name="categoryName">Category name of the performance counter</param>
        /// <returns>List of system performance counters for the given category and the given machine</returns>
        public static IEnumerable<string> GetSystemCounterNames(string machineName, string categoryName)
        {
            // Check if the required category exists.
            if (!PerformanceCounterCategory.Exists(categoryName, machineName))
                return Enumerable.Empty<string>();

            var category = new PerformanceCounterCategory(categoryName, machineName);

            return (category.CategoryType == PerformanceCounterCategoryType.SingleInstance) ? category.GetCounters().Select(pc => pc.CounterName) : new HashSet<string>(category.GetInstanceNames().SelectMany(category.GetCounters).Select(pc => pc.CounterName));
        }

        #endregion

        #region Numeric performance counter interface

        public float CounterValue { get; private set; }

        public override void Update()
        {
            UpdateInternal();
        }

        public float GetRawValue()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetRawValue(float rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetRawValue(DateTime rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void Increment()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void Decrement()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void DecrementBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public float GetBaseRawValue()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void SetBaseRawValue(float rawValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBase()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void IncrementBaseBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void DecrementBase()
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        public void DecrementBaseBy(float byValue)
        {
            throw new InvalidOperationException(string.Format(Resources.Strings.PerformanceCounterReadonly, ComponentName));
        }

        #endregion

        #region Utility methods

        private void UpdateInternal()
        {
            // Update state of the performance counter value.
            CounterValue = _system.NextValue();
        }

        internal override void UpdateState()
        {
            // Call the base method.
            base.UpdateState();

            UpdateInternal();
        }

        private static PerformanceCounterType GetCounterType(System.Diagnostics.PerformanceCounterType type)
        {
            switch (type)
            {
                case System.Diagnostics.PerformanceCounterType.NumberOfItems32:
                case System.Diagnostics.PerformanceCounterType.NumberOfItems64:
                case System.Diagnostics.PerformanceCounterType.NumberOfItemsHEX32:
                case System.Diagnostics.PerformanceCounterType.NumberOfItemsHEX64:
                    return PerformanceCounterType.CountOfItems;
                case System.Diagnostics.PerformanceCounterType.RateOfCountsPerSecond32:
                case System.Diagnostics.PerformanceCounterType.RateOfCountsPerSecond64:
                    return PerformanceCounterType.CountPerSecond;
                case System.Diagnostics.PerformanceCounterType.CountPerTimeInterval32:
                case System.Diagnostics.PerformanceCounterType.CountPerTimeInterval64:
                    return PerformanceCounterType.CountOfItems;
                case System.Diagnostics.PerformanceCounterType.RawFraction:
                case System.Diagnostics.PerformanceCounterType.SampleFraction:
                case System.Diagnostics.PerformanceCounterType.CounterTimerInverse:
                case System.Diagnostics.PerformanceCounterType.Timer100Ns:
                case System.Diagnostics.PerformanceCounterType.Timer100NsInverse:
                    return PerformanceCounterType.PercentValue;
                case System.Diagnostics.PerformanceCounterType.AverageTimer32:
                case System.Diagnostics.PerformanceCounterType.AverageBase:
                case System.Diagnostics.PerformanceCounterType.AverageCount64:
                case System.Diagnostics.PerformanceCounterType.RawBase:
                case System.Diagnostics.PerformanceCounterType.SampleBase:
                case System.Diagnostics.PerformanceCounterType.SampleCounter:
                    return PerformanceCounterType.CountOfItems;
                case System.Diagnostics.PerformanceCounterType.ElapsedTime:
                    return PerformanceCounterType.ElapsedTime;
                case System.Diagnostics.PerformanceCounterType.CounterDelta32:
                case System.Diagnostics.PerformanceCounterType.CounterDelta64:
                    return PerformanceCounterType.DeltaValue;
                default:
                    return PerformanceCounterType.Unknown;
            }
        }

        private static string GetCounterName(string machineName, string categoryName, string counterName, string instanceName)
        {
            var name = new StringBuilder();

            bool addSeparator = false;

            if (!string.IsNullOrEmpty(machineName))
            {
                name.Append(machineName);
                addSeparator = true;
            }
            if (!string.IsNullOrEmpty(categoryName))
            {
                if (addSeparator)
                    name.Append(':');
                name.Append(categoryName);
                addSeparator = true;
            }
            if (!string.IsNullOrEmpty(counterName))
            {
                if (addSeparator)
                    name.Append(':');
                name.Append(counterName);
                addSeparator = true;
            }
            if (!string.IsNullOrEmpty(instanceName))
            {
                if (addSeparator)
                    name.Append(':');
                name.Append(instanceName);
            }

            return name.ToString();
        }

        #endregion
    }
}