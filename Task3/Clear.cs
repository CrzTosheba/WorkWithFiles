using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    internal class Clear
    {
        private static DateTime ThresholdDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(30));

        public static void SetThreshold(int minutes)
            => ThresholdDate = DateTime.Now.Subtract(TimeSpan.FromMinutes(minutes));

        public static void TryClearDir(string arg)
        {
            try
            {
                ClearDir(arg);
            }
            catch (Exception h)
            {
                Logger.Log(LogLevel.Error, h.Message);
            }
        }

        private static void ClearDir(string arg)
        {

            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException($"Путь к директории не указан");
            if (!Directory.Exists(arg))
                throw new ArgumentException($"Директория {arg} не существует");

            Logger.Log(LogLevel.Debug, $"Начинается очистка директории {arg}");

            var dir = new DirectoryInfo(arg);
            //Удаляем старые файлы в данной директории
            DeleteOldFiles(dir);
            //Удаляем старые директории
            DeleteOldDirs(dir);

            //Рекурсивно проверяем оставшиеся директории на наличие старых сущностей
            ClearInner(dir);

            Logger.Log(LogLevel.Debug, $"Директория {arg} очищена");
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
                Logger.Log(LogLevel.Debug, $"Файл {file.Name} удален");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Warning, $"Не удалось удалить файл {file.Name}. Информация об ошибке:\n{e.GetType()}: {e.Message}");
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
                Logger.Log(LogLevel.Info, $"Директория {file.Name} удалена");
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.Warning, $"Не удалось удалить директорию { file.Name}. Информация об ошибке:\n{ e.GetType()}: {e.Message}");
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
