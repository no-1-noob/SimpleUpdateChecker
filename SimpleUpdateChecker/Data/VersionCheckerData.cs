using System;

namespace SimpleUpdateChecker.Data
{
    internal class VersionCheckerData
    {
        private string _acknowledgedVersion = string.Empty;
        private DateTime _dtLastVersionCheck = new DateTime(2000, 1, 1);

        public string AcknowledgedVersion { get => _acknowledgedVersion; set => _acknowledgedVersion = value; }
        public DateTime DtLastVersionCheck { get => _dtLastVersionCheck; set => _dtLastVersionCheck = value; }
    }
}
