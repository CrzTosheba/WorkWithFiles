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
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirName);
                if ((dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length) == 0) //ловим ошибки, например папка не найдена
                {
                    Console.WriteLine($"Файлов нет");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            // если папка существует
            if (Directory.Exists(dirName))
            {
                Console.WriteLine($"Путь найден");
                DirectoryInfo directory = new DirectoryInfo(dirName);
                var allFiles = directory.EnumerateFiles();        // таки получаем инфу о файлах
                var dirs = directory.EnumerateDirectories();      // получаем инфу о директориях

                //убьем файлы
                foreach (FileInfo file in allFiles)
                {
                    if (file.LastAccessTime < DateTime.Now.Subtract(TimeSpan.FromMinutes(30)))
                    {
                        file.Delete();
                    }
                        
                }
                //ну и другие директории
                foreach (DirectoryInfo di in dirs)
                {
                    if (di.LastAccessTime < DateTime.Now.Subtract(TimeSpan.FromMinutes(30)))
                    {
                        di.Delete(true);
                    }
                        
                }
                Console.WriteLine("Completed..");
                Console.ReadKey();
                }
            

        }


        


    }

}

