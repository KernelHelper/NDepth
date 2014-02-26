using System;
using System.Runtime.Serialization;
using FluentValidation;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Business.BusinessObjects.Domain
{
    /// <summary>Monitoring event represents some event in past that occurs on some machine/module/component with some severity</summary>
    [DataContract]
    public class MonitoringEvent : IExtensibleDataObject
    {
        /// <summary>Identifier</summary>
        [DataMember]
        public virtual long Id { get; set; }
        /// <summary>Timestamp</summary>
        [DataMember]
        public virtual DateTime Timestamp { get; set; }
        /// <summary>Machine name</summary>
        [DataMember]
        public virtual string Machine { get; set; }
        /// <summary>Module name</summary>
        [DataMember]
        public virtual string Module { get; set; }
        /// <summary>Component name</summary>
        [DataMember]
        public virtual string Component { get; set; }
        /// <summary>Severity code</summary>
        [DataMember]
        public virtual Severity Severity { get; set; }
        /// <summary>Title of the monitoring event</summary>
        [DataMember]
        public virtual string Title { get; set; }
        /// <summary>Description of the monitoring event</summary>
        [DataMember]
        public virtual string Description { get; set; }

        #region Validator

        /// <summary>Validator class</summary>
        public class MonitoringEventValidator : AbstractValidator<MonitoringEvent>
        {
            internal MonitoringEventValidator()
            {
                RuleFor(entity => entity.Id).GreaterThan(0);
                RuleFor(entity => entity.Machine).NotNull();
                RuleFor(entity => entity.Module).NotNull();
                RuleFor(entity => entity.Component).NotNull();
                RuleFor(entity => entity.Title).NotNull();
                RuleFor(entity => entity.Description).NotNull();
            }
        }

        /// <summary>Validator instance</summary>
        public static MonitoringEventValidator Validator = new MonitoringEventValidator();

        #endregion

        #region IExtensibleDataObject

        [NonSerialized]
        private ExtensionDataObject _theData;

        public virtual ExtensionDataObject ExtensionData
        {
            get { return _theData; }
            set { _theData = value; }
        }

        #endregion
    }
}
