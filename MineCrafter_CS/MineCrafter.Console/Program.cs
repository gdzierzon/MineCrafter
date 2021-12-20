using MineCrafter.Imaging;
using System;
using System.IO;

namespace MineCrafter.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var original = "D:\\images\\tiger.jpg";
            var saveTo = "D:\\images";
            var htmlPath = "D:\\images\\tiger.html";

            using (var converter = new ImageConverter(original, saveTo, 20, 50))
            using(var writer = new StreamWriter(htmlPath))
            {
                var html = converter.ToHtml();
                writer.WriteLine(html);
            }

            System.Console.WriteLine("Done...");

        }
    }
}
