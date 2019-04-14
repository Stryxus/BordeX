using System;
using System.Management;

namespace BordeX
{
    public static class WMI
    {
        public static bool IsWindows10
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        string v = queryObj["Version"].ToString();
                        float version = float.Parse(v.Substring(0, v.LastIndexOf(".")));
                        if (version >= 10) return true;
                        else return false;
                    }
                }
                catch (ManagementException e)
                {
                    Logger.LogError("There was an error trying to query data from the operating system\n\nError Message:\n" + e.Message);
                    return false;
                }
                return false;
            }
        }
    }
}
