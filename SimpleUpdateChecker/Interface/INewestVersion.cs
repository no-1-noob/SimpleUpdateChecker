using System;

namespace SimpleUpdateChecker.Interface
{
    /// <summary>
    /// Class to deserialize the server response wich gives the newest version no
    /// </summary>
    internal interface INewestVersion
    {
        /// <summary>
        /// Return a Version type with the major, minor, build values of the newest available version
        /// </summary>
        /// <returns></returns>
        Version NewVersionAvailable();
    }
}
