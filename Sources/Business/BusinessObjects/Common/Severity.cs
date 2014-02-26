namespace NDepth.Business.BusinessObjects.Common
{
    /// <summary>Severity codes</summary>
    public enum Severity : short
    {
        /// <summary>None level severity. Nothing will be stored into the monitoring events storage.</summary>
        None = 0,
        /// <summary>Trace level severity. Monitoring event will be represented as a trace message.</summary>
        Trace = 5,
        /// <summary>Debug level severity. Monitoring event will be stored only in debug configuration.</summary>
        Debug = 10,
        /// <summary>Info level severity. Monitoring event will be stored and shown as information.</summary>
        Info = 30,
        /// <summary>Warning level severity. Monitoring event will be stored and shown as warning.</summary>
        Warning = 50,
        /// <summary>Error level severity. Monitoring event will be stored and shown as error.</summary>
        Error = 70,
        /// <summary>Error with Email severity. Monitoring event will be stored and shown as error. Email will be send if possible.</summary>
        ErrorWithEmail = 75,
        /// <summary>Error with SMS severity. Monitoring event will be stored and shown as error. Email and SMS will be send if possible.</summary>
        ErrorWithSms = 80,
        /// <summary>Fatal error severity. Monitoring event will be stored and shown as fatal error. Email and SMS will be send if possible.</summary>
        Fatal = 90,

        /// <summary>Notification severity. Monitoring event will be stored and shown as notification.</summary>
        Notify = 1000,
        /// <summary>Notification severity. Monitoring event will be stored and shown as notification. Email will be send if possible.</summary>
        NotifyWithEmail = 1001,
        /// <summary>Notification severity. Monitoring event will be stored and shown as notification. Email and SMS will be send if possible.</summary>
        NotifyWithSms = 1002
    }
}
