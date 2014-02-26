namespace NDepth.Module
{
    /// <summary>
    /// Module console class configures and starts module as a console application.
    /// </summary>
    public sealed class ModuleBootstrapConsole<T> : ModuleBootstrap<T>
        where T : ModuleBase
    {
        public ModuleBootstrapConsole()
        {
            Bootstrap();
        }

        #region Module service interface

        /// <summary>Start module handler. Can be implemented to define module startup functionality.</summary>
        /// <param name="args">Command line arguments</param>
        public void OnStart(string[] args)
        {
            Module.OnStart(args);
        }
        
        /// <summary>Run module handler. Can be implemented to define module run functionality.</summary>
        public void OnRun()
        {
            Module.OnRun();
        }

        /// <summary>Stop module handler. Can be implemented to define module stop functionality.</summary>
        public void OnStop()
        {
            Module.OnStop();
        }

        #endregion
    }
}
