using System;

namespace NDepth.Monitoring
{
    /// <summary>Component monitoring state interface provides functionality to handle states of particular components in hierarchy</summary>
    public interface IMonitoringStateComponent : IMonitoringState
    {
        /// <summary>Component state</summary>
        State ComponentState { get; }
        /// <summary>Component state title</summary>
        string ComponentStateTitle { get; }
        /// <summary>Component state description</summary>
        string ComponentStateDescription { get; }

        /// <summary>
        /// Setup monitoring events repeating of the current component.
        /// Method is useful if some component can change its states very quickly (e.g. connecting/disconnecting to/from some unstable resource).
        /// In this case it is more reasonable to setup repeating time period to several minutes (e.g. TimeSpan.FromMinutes(5)) and store all monitoring
        /// events once in the time period.
        /// </summary>
        /// <param name="timespan">Repeat period for monitoring. Disable repeating if null.</param>
        void SetupStatesRepeat(TimeSpan? timespan);

        /// <summary>Change the current component state</summary>
        /// <param name="state">New state of the component</param>
        /// <param name="title">Title of the new state</param>
        /// <param name="description">Description of the new state</param>
        void ChangeState(State state, string title, string description);
    }
}
