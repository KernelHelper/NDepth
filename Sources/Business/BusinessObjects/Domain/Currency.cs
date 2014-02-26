using System;
using System.Runtime.Serialization;
using FluentValidation;

namespace NDepth.Business.BusinessObjects.Domain
{
    /// <summary>Currency business object</summary>
    [DataContract]
    public class Currency : IExtensibleDataObject
    {
        /// <summary>Identifier</summary>
        [DataMember]
        public virtual long Id { get; set; }
        /// <summary>Version</summary>
        [DataMember]
        public virtual int Version { get; set; }
        /// <summary>Name</summary>
        [DataMember]
        public virtual string Name { get; set; }
        /// <summary>Description of the currency</summary>
        [DataMember]
        public virtual string Description { get; set; }

        #region Validator

        /// <summary>Validator class</summary>
        public class CurrencyValidator : AbstractValidator<Currency>
        {
            internal CurrencyValidator()
            {
                RuleFor(entity => entity.Id).GreaterThan(0);
                RuleFor(entity => entity.Version).GreaterThan(0);
                RuleFor(entity => entity.Name).NotNull();
                RuleFor(entity => entity.Description).NotNull();
            }
        }

        /// <summary>Validator instance</summary>
        public static CurrencyValidator Validator = new CurrencyValidator();

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
