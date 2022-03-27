using System;
using System.IO;
using System.Collections;



namespace WorkWithFiles
{

    class Program
    {
        static void Main(string[] args)
        {

            ChekFiles();



        }
        public static void ChekFiles()
        {

            string dirName = "E:/TestFolder";

            // если папка существует
            if (Directory.Exists(dirName))
            {
                Console.WriteLine($"Путь указан верно");
                Console.WriteLine("Подкаталоги:");
                string[] dirs = Directory.GetDirectories(dirName);
                foreach (string s in dirs)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine();
                Console.WriteLine("Файлы:");
                string[] files = Directory.GetFiles(dirName);
                foreach (string s in files)
                {
                    Console.WriteLine(s);
                }

                using var watcher = new FileSystemWatcher("E:/TestFolder");

                watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;
                watcher.Error += OnError;

                watcher.Filter = "*.*";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                Console.ReadLine();


            }


            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo("E:/TestFolder");
                if ((dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length) == 0)
                {
                    Console.WriteLine($"Файлов нет");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Console.WriteLine($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }


    }

}

