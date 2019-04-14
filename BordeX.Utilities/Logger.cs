using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using System.Linq;
using System.IO;

using Ionic.Zip;

namespace BordeX
{
    public static class Logger
    {
        public static void LogInfo(object s)
        {
            AsyncConsole.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "] " + "[INFO]: " + s.ToString());
        }

        public static void LogWarn(object s)
        {
            AsyncConsole.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "] " + "[WARN]: " + s.ToString());
        }

        public static void LogError(object s, bool viewMessage = false)
        {
            AsyncConsole.WriteLine("[" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "] " + "[ERROR]: " + s.ToString());
            if (viewMessage) MessageBox.Show(s.ToString() + "\n\nFor help, please visit the BordeX Steam forums!", "BordeX public Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static class AsyncConsole
        {
            private static BlockingCollection<string> queue = new BlockingCollection<string>();
            private static Thread thread;

            static AsyncConsole()
            {
                if (FileIO.Exists(FileIO.LogsLatestDirectory + "Info.txt") && FileIO.Exists(FileIO.LogsLatestDirectory + "Warn.txt") && FileIO.Exists(FileIO.LogsLatestDirectory + "Error.txt"))
                {
                    if (Directory.GetFiles(FileIO.LogsDirectory, ".zip", SearchOption.TopDirectoryOnly).Length >= 10)
                    {
                        FileSystemInfo fileInfo = new DirectoryInfo(FileIO.LogsDirectory).GetFileSystemInfos().OrderByDescending(fi => fi.CreationTime).First();
                        FileIO.Delete(fileInfo.FullName);
                    }

                    using (ZipFile file = new ZipFile())
                    {
                        string timestring = DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString();
                        file.AddDirectory(FileIO.LogsLatestDirectory);
                        foreach (char c in Path.GetInvalidFileNameChars()) timestring = timestring.Replace(c, '_');
                        file.Save(FileIO.LogsDirectory + timestring + ".zip");
                    }
                }

                thread = new Thread(async () =>
                {
                    FileIO.Delete(FileIO.LogsLatestDirectory + "Info.txt");
                    FileIO.Delete(FileIO.LogsLatestDirectory + "Warn.txt");
                    FileIO.Delete(FileIO.LogsLatestDirectory + "Error.txt");

                    FileIO.Create(FileIO.LogsLatestDirectory + "Info.txt");
                    FileIO.Create(FileIO.LogsLatestDirectory + "Warn.txt");
                    FileIO.Create(FileIO.LogsLatestDirectory + "Error.txt");

                    StreamWriter infoWriter = new StreamWriter(File.OpenWrite(FileIO.LogsLatestDirectory + "Info.txt"));
                    StreamWriter warnWriter = new StreamWriter(File.OpenWrite(FileIO.LogsLatestDirectory + "Warn.txt"));
                    StreamWriter errorWriter = new StreamWriter(File.OpenWrite(FileIO.LogsLatestDirectory + "Error.txt"));

                    while (true)
                    {
                        string current = queue.Take();
                        if (current.Contains("[INFO]:")) await infoWriter.WriteLineAsync(current);
                        else if (current.Contains("[WARN]:")) await warnWriter.WriteLineAsync(current);
                        else if (current.Contains("[ERROR]:")) await errorWriter.WriteLineAsync(current);
#if DEBUG
                        Debug.WriteLine(current);
#else
                        Console.WriteLine(current);
#endif
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }

            public static void WriteLine(string value)
            {
                queue.Add(value);
            }
        }
    }
}
