using System;
using System.IO;
using System.Collections;
namespace WorkWithFiles;
class Calculate

{


    static public void Main()
    {

        try
        {
            DirectoryInfo folder = new DirectoryInfo("E:/TestFolder"); // идем в папку, ну как идем, попутно проверяем есть ли она


            long totalFolderSize = folderSize(folder);

            Console.WriteLine("Размер, байт: " +
                              totalFolderSize);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
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