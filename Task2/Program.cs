using System;
using System.IO;
using System.Collections;
namespace WorkWithFiles;
class Calculate

{

    enum LogLevel
    {
        Info,
        Warning,
        Error,
    }

    private static void Log(LogLevel lvl, string msg) // все тоже логирование
        => Console.WriteLine("{0} | {1} | {2}",
            DateTime.Now,
            lvl,
            msg
        );
    static public void Main(string[] args)
    {

        if (args.Length > 0)
        {
            foreach (string arg in args)
                Log(LogLevel.Info, $"Size {arg} = {TryCalcDir(arg)}");
        }
        else
        {
            Log(LogLevel.Warning, "В аргументах командной строки не указан путь к директории");


            string str = "";
            while (string.IsNullOrWhiteSpace(str))
            {
                Console.WriteLine("Введите адрес директории для подсчета");
                str = Console.ReadLine();
                if (str == "-q")
                    return;
                Log(LogLevel.Info, $"Size {str} = {TryCalcDir(str)}");
                str = "";
            }

        }
    }

    static private long TryCalcDir(string arg)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(arg))
                throw new ArgumentException($"Путь к директории не указан");
            if (!Directory.Exists(arg))
                throw new ArgumentException($"Директория {arg} не существует");

            DirectoryInfo folder = new DirectoryInfo(arg); // идем в папку, ну как идем, попутно проверяем есть ли она

            long totalFolderSize = folderSize(folder);

            return totalFolderSize;
        }
        catch (Exception e)
        {
            Log(LogLevel.Error, $"Не удалось посчитать директорию\n{e.Message}");
            return -1;
        }
    }


    static long folderSize(DirectoryInfo folder)
    {

        long totalSizeOfDir = 0;


        FileInfo[] allFiles = folder.GetFiles(); //все файлы в директории


        foreach (FileInfo file in allFiles) // тащим размер каждого файла
        {
            totalSizeOfDir += file.Length;
        }


        DirectoryInfo[] subFolders = folder.GetDirectories(); //ищем вложенные папки


        foreach (DirectoryInfo dir in subFolders) //тащим размер вложенных папок
        {
            totalSizeOfDir += folderSize(dir);
        }


        return totalSizeOfDir; //вертаем в зад общий размер
    }
}