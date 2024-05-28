using SimpleUpdateChecker.Interface;
using System;

namespace SimpleUpdateChecker.Data
{
    internal class DefaultNewestVersion : INewestVersion
    {
        public string NewestVersion { get; set; }

        public Version NewVersionAvailable()
        {
            return new Version(NewestVersion);
        }
    }
}
