using MineCrafter.Imaging;
using System;
using System.IO;

namespace MineCrafter.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string originalPath;
            string saveToPath;
            int cellSize;
            int newWidth;

            System.Console.WriteLine();
            System.Console.Write("Enter the path of the image to convert? ");
            originalPath = System.Console.ReadLine();

            System.Console.WriteLine();
            System.Console.Write("How big do you want each pixel? ");
            cellSize = int.Parse(System.Console.ReadLine());

            System.Console.WriteLine();
            System.Console.Write("How many pixels wide do you want the new picture? ");
            newWidth = int.Parse(System.Console.ReadLine());

            System.Console.WriteLine();
            System.Console.Write("Enter the path and name of the html file? ");
            saveToPath = System.Console.ReadLine();

            using(var converter = new ImageConverter(originalPath, cellSize, newWidth))
            using(var writer = new StreamWriter(saveToPath))
            {
                var html = converter.ToHtml();
                writer.WriteLine(html);
            }

            System.Console.WriteLine($"{saveToPath} has been generated!");

        }
    }
}
