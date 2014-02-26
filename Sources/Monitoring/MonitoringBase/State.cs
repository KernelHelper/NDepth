namespace NDepth.Monitoring
{
    /// <summary>Component states</summary>
    public enum State
    {
        /// <summary>Component is in normal state</summary>
        Normal = 0,
        
        /// <summary>Component is in broken state</summary>
        Broken = 20,
        /// <summary>Component is in overflow state</summary>
        Overflow = 21,
        /// <summary>Component is in unknown state</summary>
        Unknown = 22,

        /// <summary>Component is not initialized</summary>
        NotInitialized = 40,
        /// <summary>Component is initializing now</summary>
        Initializing = 41,
        /// <summary>Component failed to initialize properly</summary>
        InitializationFailed = 42,
        /// <summary>Component is properly initialized</summary>
        Initialized = 43,
        /// <summary>Component is de-initializing now</summary>
        Deinitializing = 44,
        /// <summary>Component failed to de-initialize properly</summary>
        DeinitializationFailed  = 45,
        /// <summary>Component is properly de-initialized</summary>
        Deinitialized = 46,

        /// <summary>Component is not connected</summary>
        NotConnected = 60,
        /// <summary>Component is connecting now</summary>
        Connecting = 61,
        /// <summary>Component failed to connect properly</summary>
        ConnectionFailed = 62,
        /// <summary>Component is properly connected</summary>
        Connected = 63,
        /// <summary>Component is disconnecting now</summary>
        Disconnecting = 64,
        /// <summary>Component failed to disconnect properly</summary>
        DisconnectionFailed = 65,
        /// <summary>Component is properly disconnected</summary>
        Disconnected = 66,

        /// <summary>Component failed to authenticate to resource</summary>
        AuthenticationFailed = 80,
        /// <summary>Component failed to authorize to resource</summary>
        AuthorizationFailed = 81
    }
}
