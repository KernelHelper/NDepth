namespace NDepth.Module
{
    /// <summary>
    /// Module configuration.
    /// </summary>
    public class ModuleConfiguration
    {
        /// <summary>Machine name</summary>
        public string MachineName { get; set; }
        /// <summary>Machine description</summary>
        public string MachineDescription { get; set; }
        /// <summary>Machine address</summary>
        public string MachineAddress { get; set; }
        /// <summary>Module name</summary>
        public string ModuleName { get; set; }
        /// <summary>Module description</summary>
        public string ModuleDescription { get; set; }
        /// <summary>Module version</summary>
        public string ModuleVersion { get; set; }
        /// <summary>Module connection string</summary>
        public string ConnectionString { get; set; }
        /// <summary>Module connection type</summary>
        public string ConnectionType { get; set; }

        public ModuleConfiguration()
        {
            MachineName = Resources.Strings.DefaultMachineName;
            MachineDescription = Resources.Strings.DefaultMachineDescription;
            MachineAddress = Resources.Strings.DefaultMachineAddress;
            ModuleName = Resources.Strings.DefaultModuleName;
            ModuleDescription = Resources.Strings.DefaultModuleDescription;
            ModuleVersion = Resources.Strings.DefaultModuleVersion;
            ConnectionString = string.Empty;
            ConnectionType = string.Empty;
        }

        public ModuleConfiguration(string machineName, string machineDescription, string machineAddress, string moduleName, string moduleDescription, string moduleVersion, string connectionString, string connectionType)
        {
            MachineName = machineName;
            MachineDescription = machineDescription;
            MachineAddress = machineAddress;
            ModuleName = moduleName;
            ModuleDescription = moduleDescription;
            ModuleVersion = moduleVersion;
            ConnectionString = connectionString;
            ConnectionType = connectionType;
        }
    }
}
