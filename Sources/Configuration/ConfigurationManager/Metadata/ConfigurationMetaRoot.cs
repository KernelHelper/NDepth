namespace NDepth.Configuration.ConfigurationManager.Metadata
{
    /// <summary>
    /// Configuration meta root represents top level storage of configuration metadata nodes and collections.
    /// </summary>
    public class ConfigurationMetaRoot : ConfigurationMetaNode
    {
        #region Constructor

        internal ConfigurationMetaRoot(ConfigurationManager manager) 
            : base(manager, null)
        {
        }

        #endregion
    }
}
