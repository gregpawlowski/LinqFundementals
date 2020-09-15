using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

// Get first 5 highest files.

namespace Introduction
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Windows";
            ShowLargeFilesWithoutLinq(path);
            Console.WriteLine("******");
            ShowLargeFilesWithLinq(path);
        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            // Linq query syntax
            //var query = from file in new DirectoryInfo(path).GetFiles()
            //            orderby file.Length descending
            //            select file;

            // Linq method syntax
            var query = new DirectoryInfo(path).GetFiles()
                    .OrderByDescending(f => f.Length)
                    .Take(5);


            foreach (var file in query.Take(5))
            {
                // Left justify inside of 20 spaces
                // Right justify in a ten space column adn format as a number (so that it has commas) and 0 positions after decimal point.
                Console.WriteLine($"{file.Name,-20} : {file.Length,15:N0}");
            }

        }

        private static void ShowLargeFilesWithoutLinq(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();

            // Using a lambda instead of a whole class implementation.
            Array.Sort(files, (file1, file2) => file2.Length.CompareTo(file1.Length));

            for (var i = 0; i < 5; i++)
            {
                // Left justify inside of 20 spaces
                // Right justify in a ten space column adn format as a number (so that it has commas) and 0 positions after decimal point.
                Console.WriteLine($"{files[i].Name, -20} : {files[i].Length, 15:N0}");
            }
        }
    }

    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
        {
            return x.Length.CompareTo(y.Length);
        }
    }
}
