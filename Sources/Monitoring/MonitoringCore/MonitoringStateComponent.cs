using System;
using NDepth.Business.BusinessObjects.Common;
using NDepth.Common.DisposableLock;

namespace NDepth.Monitoring.Core
{
    public class MonitoringStateComponent : MonitoringStateBase, IMonitoringStateComponent
    {
        internal MonitoringStateComponent(string name, Severity severity, IMonitoring monitoring)
            : base(name, severity, monitoring)
        {
        }

        internal MonitoringStateComponent(string name, Severity severity, IMonitoringState parent) 
            : base(name, severity, parent)
        {
        }

        #region Monitoring state component interface

        private State _componentState = State.Normal;
        private string _componentStateTitle = string.Empty;
        private string _componentStateDescription = string.Empty;

        private TimeSpan? _timespan;

        public State ComponentState { get { using (new ReadLock(Lock)) { return _componentState; } } }
        public string ComponentStateTitle { get { using (new ReadLock(Lock)) { return _componentStateTitle; } } }
        public string ComponentStateDescription { get { using (new ReadLock(Lock)) { return _componentStateDescription; } } }

        public void SetupStatesRepeat(TimeSpan? timespan)
        {
            using (new WriteLock(Lock))
            {
                _timespan = timespan;
            }
        }

        public void ChangeState(State state, string title, string description)
        {
            // Get the current severity value.
            var severity = Severity;

            using (new WriteLock(Lock))
            {
                if ((_componentState != state) || (_componentStateTitle != title) || (_componentStateDescription != description))
                {
                    // Prepare description message.
                    var message = "Component '" + ComponentName + "' changed its state from '" + _componentState + "' to '" + state + "': " + description;

                    // Save new state.
                    _componentState = state;
                    _componentStateTitle = title;
                    _componentStateDescription = description;

                    // Register monitoring event.
                    if (_timespan.HasValue)
                        Monitoring.RegisterRepeat(_timespan.Value, severity, _componentStateTitle, message);
                    else
                        Monitoring.Register(severity, _componentStateTitle, message);
                }
            }
        }

        #endregion
    }
}