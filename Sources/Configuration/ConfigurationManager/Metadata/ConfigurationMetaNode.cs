using System;
using System.Collections.Generic;

namespace NDepth.Configuration.ConfigurationManager.Metadata
{
    /// <summary>
    /// Configuration meta node represents single configuration element metadata.
    /// </summary>
    public class ConfigurationMetaNode
    {
        private readonly IMetaDataConfigurationInfo _configMetadata;

        private readonly Dictionary<string, IMetaDataConfigurationInfo> _metaDataByName = new Dictionary<string, IMetaDataConfigurationInfo>();
        private readonly Dictionary<string, ConfigurationMetaNode> _childNodesByName = new Dictionary<string, ConfigurationMetaNode>();

        private readonly Dictionary<Type, IMetaDataConfigurationInfo> _metaDataByType = new Dictionary<Type, IMetaDataConfigurationInfo>();
        private readonly Dictionary<Type, ConfigurationMetaNode> _childNodesByType = new Dictionary<Type, ConfigurationMetaNode>();

        #region Constructors

        internal ConfigurationMetaNode(ConfigurationManager manager, ConfigurationMetaNode parent)
        {
            Manager = manager;
            Parent = parent;
        }

        internal ConfigurationMetaNode(ConfigurationManager manager, ConfigurationMetaNode parent, IMetaDataConfigurationInfo configMetadata)
            : this(manager, parent)
        {
            _configMetadata = configMetadata;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Configuration manager
        /// </summary>
        public ConfigurationManager Manager { get; private set; }

        /// <summary>
        /// Parent configuration metadata node
        /// </summary>
        public ConfigurationMetaNode Parent { get; private set; }

        #endregion

        #region Metadata configuration

        internal interface IMetaDataConfigurationInfo
        {
        }

        private class MetaDataConfigurationInfoWithName<TEntity> : IMetaDataConfigurationInfo
        {
            private readonly string Name;
            private readonly Type EntityType;

            internal MetaDataConfigurationInfoWithName(string name)
            {
                Name = name;
                EntityType = typeof(TEntity);
            }
        }

        private class MetaDataConfigurationInfoWithKey<TEntity, TKey> : IMetaDataConfigurationInfo
        {
            private readonly Type EntityType;
            private readonly Func<TEntity, TKey> KeyFunc;

            internal MetaDataConfigurationInfoWithKey(Func<TEntity, TKey> keyFunc)
            {
                EntityType = typeof(TEntity);
                KeyFunc = keyFunc;
            }
        }

        private class MetaDataConfigurationInfoWithKeyAndUnique<TEntity, TKey, TUnique> : IMetaDataConfigurationInfo
        {
            private readonly Type EntityType;
            private readonly Func<TEntity, TKey> KeyFunc;
            private readonly Func<TEntity, TUnique> UniqueFunc;

            internal MetaDataConfigurationInfoWithKeyAndUnique(Func<TEntity, TKey> keyFunc, Func<TEntity, TUnique> uniqueFunc)
            {
                EntityType = typeof(TEntity);
                KeyFunc = keyFunc;
                UniqueFunc = uniqueFunc;
            }
        }

        /// <summary>Metadata configure of the new configuration entity with provided key access function</summary>
        /// <param name="name">Name of the configuration entity</param>
        /// <returns>Registered container for the given configuration entity. It can be used for nested entities configuration.</returns>
        public ConfigurationMetaNode MetaDataConfigure<TEntity>(string name)
        {
            if (Manager.IsInitialized)
                throw new InvalidOperationException(Resources.Strings.WriterLockNotHeld);
            if (_metaDataByName.ContainsKey(name))
                throw new InvalidOperationException(string.Format(Resources.Strings.MetaDataConfigDuplicateByName, name));

            var mdConfigInfo = new MetaDataConfigurationInfoWithName<TEntity>(name);
            var mdChildNode = new ConfigurationMetaNode(Manager, this, mdConfigInfo);
            _metaDataByName.Add(name, mdConfigInfo);
            _childNodesByName.Add(name, mdChildNode);
            return mdChildNode;
        }

        /// <summary>Metadata configure of the new configuration entity collection with provided key access function</summary>
        /// <param name="keyFunc">Key function which is used to get key of the configuration entity (e.g. Id field)</param>
        /// <returns>Registered container for the given configuration entity collection. It can be used for nested entities configuration.</returns>
        public ConfigurationMetaNodes MetaDataConfigure<TEntity, TKey>(Func<TEntity, TKey> keyFunc)
        {
            if (Manager.IsInitialized)
                throw new InvalidOperationException(Resources.Strings.WriterLockNotHeld);
            if (_metaDataByType.ContainsKey(typeof(TEntity)))
                throw new InvalidOperationException(string.Format(Resources.Strings.MetaDataConfigDuplicateByType, typeof(TEntity).Name));

            var mdConfigInfo = new MetaDataConfigurationInfoWithKey<TEntity, TKey>(keyFunc);
            var mdChildNodes = new ConfigurationMetaNodes(Manager, this, mdConfigInfo);
            _metaDataByType.Add(typeof(TEntity), mdConfigInfo);
            _childNodesByType.Add(typeof(TEntity), mdChildNodes);
            return mdChildNodes;
        }

        /// <summary>Metadata configure of the new configuration entity collection with provided key and unique access functions</summary>
        /// <param name="keyFunc">Key function which is used to get key of the configuration entity (e.g. Id field)</param>
        /// <param name="uniqueFunc">Unique function which is used to get unique key of the configuration entity (e.g. Name field)</param>
        /// <returns>Registered container for the given configuration entity collection. It can be used for nested entities configuration.</returns>
        public ConfigurationMetaNodes MetaDataConfigure<TEntity, TKey, TUnique>(Func<TEntity, TKey> keyFunc, Func<TEntity, TUnique> uniqueFunc)
        {
            if (Manager.IsInitialized)
                throw new InvalidOperationException(Resources.Strings.WriterLockNotHeld);
            if (_metaDataByType.ContainsKey(typeof(TEntity)))
                throw new InvalidOperationException(string.Format(Resources.Strings.MetaDataConfigDuplicateByType, typeof(TEntity).Name));

            var mdConfigInfo = new MetaDataConfigurationInfoWithKeyAndUnique<TEntity, TKey, TUnique>(keyFunc, uniqueFunc);
            var mdChildNodes = new ConfigurationMetaNodes(Manager, this, mdConfigInfo);
            _metaDataByType.Add(typeof(TEntity), mdConfigInfo);
            _childNodesByType.Add(typeof(TEntity), mdChildNodes);
            return mdChildNodes;
        }

        #endregion
    }
}
