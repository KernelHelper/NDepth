namespace NDepth.Configuration.ConfigurationManager.Metadata
{
    /// <summary>
    /// Configuration meta nodes represents an organized collection of configuration metadata elements.
    /// </summary>
    public class ConfigurationMetaNodes : ConfigurationMetaNode
    {
        #region Constructors

        internal ConfigurationMetaNodes(ConfigurationManager manager, ConfigurationMetaNode parent)
            : base(manager, parent)
        {
        }

        internal ConfigurationMetaNodes(ConfigurationManager manager, ConfigurationMetaNode parent, IMetaDataConfigurationInfo metaDataConfig)
            : base(manager, parent, metaDataConfig)
        {
        }

        #endregion
    }
}
