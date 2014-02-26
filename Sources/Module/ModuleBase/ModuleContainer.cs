using System.Diagnostics;
using Common.Logging;
using SimpleInjector;

namespace NDepth.Module
{
    /// <summary>
    /// Module dependency injection container.
    /// </summary>
    public static class ModuleContainer
    {
        /// <summary>Dependency injection container</summary>
        public static readonly Container Container = new Container();

        #region Common registration

        /// <summary>Register module configuration</summary>
        /// <param name="moduleConfiguration">Module configuration (null for default configuration)</param>
        public static void RegisterModuleConfiguration(ModuleConfiguration moduleConfiguration = null)
        {
            Container.RegisterSingle(() => moduleConfiguration ?? new ModuleConfiguration());
        }

        /// <summary>Register module</summary>
        public static void RegisterModule<T>() where T : ModuleBase
        {
            Container.RegisterSingle<ModuleBase, T>();
        }

        #endregion

        #region Verification

        /// <summary>Verify all dependencies</summary>
        public static void Verify()
        {
            Container.Verify();
        }

        #endregion
    }
}
