using System;
using System.Reflection;
using System.Diagnostics;

namespace BordeX
{
    public static class References
    {
        public static string Name = Assembly.GetEntryAssembly().GetName().Name;
        public static string Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
        public static string Author = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName;
        public static string Copyright = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
        public static string[] Contributors = new string[]
        {
            // Add your name under here if you contribute on github!

        };

        public static string StorePage = "http://store.steampowered.com/";
        public static string ForumPage = "";
    }
}
