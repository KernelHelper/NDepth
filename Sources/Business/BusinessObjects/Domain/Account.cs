using System;
using System.Runtime.Serialization;
using FluentValidation;

namespace NDepth.Business.BusinessObjects.Domain
{
    /// <summary>Account business object</summary>
    [DataContract]
    public class Account : IExtensibleDataObject
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

        #region Validator

        /// <summary>Validator class</summary>
        public class AccountValidator : AbstractValidator<Account>
        {
            internal AccountValidator()
            {
                RuleFor(entity => entity.Id).GreaterThan(0);
                RuleFor(entity => entity.Version).GreaterThan(0);
                RuleFor(entity => entity.Name).NotNull();
            }
        }

        /// <summary>Validator instance</summary>
        public static AccountValidator Validator = new AccountValidator();

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
