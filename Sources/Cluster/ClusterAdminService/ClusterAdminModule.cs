using NDepth.Module;

namespace NDepth.Cluster.ClusterAdminService
{
    public class ClusterAdminModule : ModuleBase
    {
        #region Module interface

        protected override void OnStart(string[] args)
        {
            Logger.InfoFormat(Resources.Strings.ModuleStarted, ModuleName, MachineName);
        }

        protected override void OnStop()
        {
            Logger.InfoFormat(Resources.Strings.ModuleStopped, ModuleName, MachineName);
        }

        #endregion
    }
}
