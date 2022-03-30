using System;
using System.IO;
using System.Collections;
namespace Task3;
class Calculate
{

    static public void Main(string[] args)
    {
        Clear.SetThreshold(1);
        Logger.MinLevel = LogLevel.Info;

        if (args.Length > 0)
        {
            foreach (string arg in args)
                Logger.Log(LogLevel.Info, $"Size {arg} = {TryCalcDir(arg)}");
        }
        else
        {
            Logger.Log(LogLevel.Warning, "В аргументах командной строки не указан путь к директории");


            string str = "";
            while (string.IsNullOrWhiteSpace(str))
            {
                Console.WriteLine("Введите адрес директории для подсчета");
                str = Console.ReadLine();
                if (str == "-q")
                    return;

                ClearDir(str);
                str = "";
            }

        }
    }

    private static void ClearDir(string? str)
    {
        var before = TryCalcDir(str);
        Clear.TryClearDir(str);
        var after = TryCalcDir(str);

        Logger.Log(LogLevel.Info, $"Размер до очистки {before} байт");
        Logger.Log(LogLevel.Info, $"Размер после очистки {after} байт");
        Logger.Log(LogLevel.Info, $"Очищено {after - before} байт");

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
            Logger.Log(LogLevel.Error, $"Не удалось посчитать директорию\n{e.Message}");
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