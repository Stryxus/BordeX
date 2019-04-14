using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;

using Newtonsoft.Json;

namespace BordeX
{
    public static class FileIO
    {
        public static string ApplicationPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string AppDataDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BordeX\\";
            }
        }

        public static string ProfilesFile
        {
            get
            {
                return AppDataDirectory + "Profiles.json";
            }
        }

        public static string SettingsFile
        {
            get
            {
                return AppDataDirectory + "Settings.json";
            }
        }

        public static string LogsDirectory
        {
            get
            {
                return AppDataDirectory + "\\Logs\\";
            }
        }

        public static string LogsLatestDirectory
        {
            get
            {
                return LogsDirectory + "Latest\\";
            }
        }

        public static void ValidateFileSystem()
        {
            if (!Exists(AppDataDirectory)) Create(AppDataDirectory);
            if (!Exists(LogsDirectory)) Create(LogsDirectory);
            if (!Exists(LogsLatestDirectory)) Create(LogsLatestDirectory);
            if (!Exists(SettingsFile)) Create(SettingsFile);
            if (!Exists(ProfilesFile)) Create(ProfilesFile);
        }

        public static void Create(string path)
        {
            if (path.LastIndexOf("\\", StringComparison.CurrentCulture) != (path.Length - 1)) File.Create(path).Close();
            else Directory.CreateDirectory(path);
        }

        public static void Delete(string path)
        {
            if (path.LastIndexOf("\\", StringComparison.CurrentCulture) != (path.Length - 1)) File.Delete(path);
            else Directory.Delete(path, true);
        }

        public static bool Exists(string path)
        {
            if (path.LastIndexOf("\\", StringComparison.CurrentCulture) != (path.Length - 1)) return File.Exists(path);
            else return Directory.Exists(path);
        }

        public static void SaveSettings(object settings)
        {
            using (FileStream stream = File.Open(SettingsFile, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    JsonSerializer serial = new JsonSerializer();
                    serial.Serialize(writer, settings);
                }
            }
        }

        public static T LoadSettings<T>() where T : new ()
        {
            using (StreamReader reader = File.OpenText(SettingsFile))
            {
                JsonSerializer serial = new JsonSerializer();
                string text = reader.ReadToEnd();
                if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                {
                    reader.Dispose();
                    SaveSettings(new T());
                    return new T();
                }
                return (T)serial.Deserialize(reader, typeof(T));
            }
        }

        public static void SaveProfiles(object instance)
        {
            using (FileStream stream = File.Open(ProfilesFile, FileMode.Truncate))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    JsonSerializer serial = new JsonSerializer();
                    serial.Serialize(writer, instance);
                }
            }
        }

        public static T LoadProfiles<T>() where T : new ()
        {
            using (StreamReader reader = File.OpenText(ProfilesFile))
            {
                JsonSerializer serial = new JsonSerializer();
                string text = reader.ReadToEnd();
                if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                {
                    reader.Dispose();
                    SaveProfiles(new T());
                    return new T();
                }
                return (T)serial.Deserialize(reader, typeof(T));
            }
        }
    }
}
