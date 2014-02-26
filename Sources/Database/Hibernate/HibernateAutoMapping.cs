using System;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentValidation;
using NDepth.Business.BusinessObjects;
using NDepth.Business.BusinessObjects.Common;

namespace NDepth.Database.Hibernate
{
    /// <summary>
    /// Hibernate auto mapping conversions provider.
    /// </summary>
    public class HibernateAutoMapping
    {
        public static AutoPersistenceModel CreateAutoMappingModel()
        {
            // Create and setup auto-mappings model.
            var autoMappingModel = AutoMap.AssemblyOf<AssemblyClass>()
                .Where(t => ((t.Namespace == "NDepth.Business.BusinessObjects.Domain") && !t.GetInterfaces().Contains(typeof(IValidator))))
                .Conventions.Add<IdConvention>()
                .Conventions.Add<DateTimeConvention>()
                .Conventions.Add<SeverityEnumConvention>()
                .OverrideAll(p => p.IgnoreProperty("ExtensionData"));

            return autoMappingModel;
        }

        #region Hibernate Auto Mapping conventions

        internal class IdConvention : IIdConvention
        {
            private const string IdName = "Id";
            private const string TableName = "HiLoTable";
            private const string TableColumnName = "TableName";
            private const string ColumnName = "NextHi";
            private const string MaxLo = "1000";

            public void Apply(IIdentityInstance instance)
            {
                instance.Column(IdName);
                instance.GeneratedBy.HiLo(TableName, ColumnName, MaxLo, builder => builder.AddParam("where", string.Format("{0} = '{1}'", TableColumnName, instance.EntityType.Name)));
            }
        }

        internal class ForeignKeyConvention : IReferenceConvention
        {
            public void Apply(IManyToOneInstance instance)
            {
                instance.Column(instance.Class.Name + "FK");
            }
        }

        internal class DateTimeConvention : IPropertyConvention, IPropertyConventionAcceptance
        {
            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria.Expect(x => x.Type == typeof(DateTime));
            }

            public void Apply(IPropertyInstance instance)
            {
                instance.CustomType("datetime2");
            }
        }

        internal class SeverityEnumConvention : IPropertyConvention, IPropertyConventionAcceptance
        {
            public void Apply(IPropertyInstance instance)
            {
                instance.CustomType(instance.Property.PropertyType);
            }

            public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
            {
                criteria.Expect(x => x.Property.PropertyType == typeof(Severity));
            }
        }

        #endregion
    }
}
