using System.ServiceProcess;

namespace NDepth.Module
{
    /// <summary>
    /// Module service class configures and starts module as Windows Service.
    /// </summary>
    public partial class ModuleBootstrapService<T> : ServiceBase
        where T : ModuleBase
    {
        private readonly ModuleBootstrap<T> _module = new ModuleBootstrap<T>();

        public ModuleBootstrapService()
        {
            InitializeComponent();

            // Update service name.
            ServiceName = _module.Module.ModuleName;

            // Setup service behavior.
            CanStop = true;
            CanShutdown = true;
        }

        #region Module service interface

        protected override void OnStart(string[] args)
        {
            _module.Module.OnStart(args);
        }

        protected override void OnStop()
        {
            _module.Module.OnStop();
        }

        protected override void OnShutdown()
        {
            _module.Module.OnStop();
        }

        #endregion
    }
}
