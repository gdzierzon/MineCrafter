using MineCrafter.Imaging.Extensions;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace MineCrafter.Imaging
{
    public class ImageConverter: IDisposable
    {
        private bool disposedValue;

        private FileInfo OriginalPath { get; set; }
        private int CellSize { get; set; }
        private int NewWidth { get; set; }
        private int NewHeight { get; set; }

        public Image Original { get; set; } = null;
        public Image Pixel { get; set; } = null;
        private Image Final { get; set; } = null;

        private String FileName
        {
            get
            {
                return OriginalPath.Name.Replace(FileExtension, "");
            }
        }

        private String FileExtension
        {
            get { return OriginalPath.Extension; }
        }

        public ImageConverter(string originalPath, int cellSize, int newWidth)
        {
            OriginalPath = new FileInfo(originalPath);
            CellSize = cellSize;
            NewWidth = newWidth;

            Init();
        }

        private void Init()
        {
            Original = new Bitmap(Image.FromFile(OriginalPath.FullName));

            var ratio = NewWidth / (double)Original.Width;
            NewHeight = (int)(Original.Height * ratio);

            Pixel = Transform(Original, NewWidth, NewHeight);
        }

        private Image Transform(Image source, int width, int height)
        {
            var destination = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(destination);
            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.None;
            g.PixelOffsetMode = PixelOffsetMode.None;
            g.CompositingQuality = CompositingQuality.AssumeLinear;

            g.DrawImage(source, 0, 0, width, height);

            return destination;
        }

        public Image ToPixelImage()
        {
            Bitmap source = new Bitmap(Pixel);
            int width = NewWidth * CellSize;
            int height = NewHeight * CellSize;

            var destination = new Bitmap(width, height);
            var g = Graphics.FromImage(destination);

            for (int row = 0; row < source.Height; row++)
            {
                Console.WriteLine();
                Console.WriteLine($"Row: {row}");
                for (int col = 0; col < source.Width; col++)
                {
                    Console.Write($"{col} ");

                    Color pixel = source.GetPixel(col, row);
                    Brush brush = new SolidBrush(pixel);
                    var back = new Pen(brush);

                    int x = col * CellSize;
                    int y = row * CellSize;
                    var rectangle = new Rectangle(x, y, width, height);

                    g.FillRectangle(brush, rectangle);
                }
            }

            return destination;
        }

        public string ToHtml()
        {
            var source = new Bitmap(Pixel);
            var width = NewWidth * CellSize;

            var builder = new StringBuilder();
            builder.AppendLine("<html><head><style>");
            builder.AppendLine("body{margin: 0px; display: flex; flex-direction: column;}");
            builder.AppendLine($".row{{display: flex; flex-direction: row; width: {width}px;}}");
            builder.AppendLine($".cell{{height: {CellSize}px; width: {CellSize}px; overflow: hidden;}}");
            builder.AppendLine("</style></head><body>");

            for (int row = 0; row < source.Height; row++)
            {
                builder.Append("<div class='row'>");
                for (int col = 0; col < source.Width; col++)
                {
                    Color pixel = source.GetPixel(col, row);
                    String hexColor = pixel.ToHex();
                    builder.Append($"<div class='cell' style='background-color: {hexColor};'>&nbsp;</div>");
                }
                builder.AppendLine("</div>");
            }

            builder.AppendLine("</body></html>");

            return builder.ToString();
        }

        public void SaveImage(String fileName)
        {
            SaveImage(Final, fileName);
        }

        public void SaveImage(Image image, String fileName)
        {
            image.Save(fileName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                Original = null;
                Pixel = null;
                Final = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code.
            // Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
