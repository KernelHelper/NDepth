using System;
using System.Runtime.Serialization;
using FluentValidation;

namespace NDepth.Business.BusinessObjects.Domain
{
    /// <summary>Order business object</summary>
    [DataContract]
    public class Order : IExtensibleDataObject
    {
        /// <summary>Identifier</summary>
        [DataMember]
        public virtual long Id { get; set; }
        /// <summary>Version</summary>
        [DataMember]
        public virtual int Version { get; set; }
        /// <summary>Account</summary>
        [DataMember]
        public virtual Account Account { get; set; }
        /// <summary>Symbol</summary>
        [DataMember]
        public virtual string Symbol { get; set; }
        /// <summary>Price</summary>
        [DataMember]
        public virtual decimal Price { get; set; }
        /// <summary>Volume</summary>
        [DataMember]
        public virtual decimal Volume { get; set; }

        #region Validator

        /// <summary>Validator class</summary>
        public class OrderValidator : AbstractValidator<Order>
        {
            internal OrderValidator()
            {
                RuleFor(entity => entity.Id).GreaterThan(0);
                RuleFor(entity => entity.Version).GreaterThan(0);
                RuleFor(entity => entity.Account).NotNull();
                RuleFor(entity => entity.Symbol).NotNull();
                RuleFor(entity => entity.Price).GreaterThan(0);
                RuleFor(entity => entity.Volume).GreaterThan(0);
            }
        }

        /// <summary>Validator instance</summary>
        public static OrderValidator Validator = new OrderValidator();

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
