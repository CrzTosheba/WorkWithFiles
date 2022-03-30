using System;
using System.IO;
using System.Collections;



namespace WorkWithFiles
{

    class Program
    { // сделаем уровни логирования, для удобства.
        enum LogLevel
        {
            Info,
            Warning,
            Error,
        }

        static readonly DateTime ThresholdDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(1));

        private static void Log(LogLevel lvl, string msg)
            => Console.WriteLine("{0} | {1} | {2}",
                DateTime.Now,
                lvl,
                msg
            );

        static void Main(string[] args)
        {
                       
            if (args.Length > 0) {
                foreach (string arg in args)
                    TryClearDir(arg);
            }else
            {
                Log(LogLevel.Warning, "Не указан путь к директории");


                string str = "";
                while (string.IsNullOrWhiteSpace(str)) {
                    Console.WriteLine("Для выхода введите -q");
                    Console.WriteLine("Введите адрес директории для очистки");
                    
                    str = Console.ReadLine();
                    if (str == "-q")
                        return;
                    TryClearDir(str);
                    str = "";
                }
                
            }
            
        }

        private static void TryClearDir(string arg) // ловим ошибки
        {
            try
            {
                ClearDir(arg);
            }
            catch (Exception h)
            {
                Log(LogLevel.Error, h.Message);
            }
        }

        private static void ClearDir(string arg)
        {

            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException($"Путь к директории не указан");
            if (!Directory.Exists(arg))
                throw new ArgumentException($"Директория {arg} не существует");

            Log(LogLevel.Info, $"Проверяем возможность удаления {arg}");

            var dir = new DirectoryInfo(arg);
            //Удаляем старые файлы в данной директории
            DeleteOldFiles(dir);
            //Удаляем старые директории
            DeleteOldDirs(dir);

            //Рекурсивно проверяем оставшиеся директории на наличие старых сущностей
            ClearInner(dir);

            Log(LogLevel.Info, $"Операция завершена {arg} ");
        }

        private static void DeleteOldFiles(DirectoryInfo dir)
        {
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                if (file.LastAccessTime < ThresholdDate)
                {
                    TryDeleteFile(file);
                }
            }
        }

        private static void TryDeleteFile(FileInfo file)
        {
            try
            {
                file.Delete();
                Log(LogLevel.Info, $"Файл {file.Name} удален");
            }
            catch (Exception e)
            { 
                Log(LogLevel.Warning, $"Не удалось удалить файл {file.Name}. Информация об ошибке:\n{e.GetType()}: {e.Message}");
            }
        }

        private static void DeleteOldDirs(DirectoryInfo dir)
        {
            var dirs = dir.GetDirectories();
            foreach (var d in dirs)
            {
                if (d.LastAccessTime < ThresholdDate)
                {
                    TryDeleteDir(d);
                }
            }
        }

        private static void TryDeleteDir(DirectoryInfo file)
        {
            try
            {
                file.Delete(true);
                Log(LogLevel.Info, $"Директория {file.Name} удалена");
            }
            catch (Exception e)
            {
                Log(LogLevel.Warning, $"Не удалось удалить директорию { file.Name}. Информация об ошибке:\n{ e.GetType()}: {e.Message}");
            }
        }

        private static void ClearInner(DirectoryInfo dir)
        {
            var dirs = dir.GetDirectories();
            foreach (var d in dirs)
                ClearDir(d.FullName);
        }
    }

}

