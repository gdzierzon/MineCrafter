package net.igregor.minecrafter.models;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

public class ImageConverter
{
    private String originalPath;
    private int cellSize;
    private final int newWidth;
    private int newHeight;

    public Image original = null;
    public Image pixel = null;

    public Image getPixel()
    {
        return pixel;
    }

    public ImageConverter(String originalPath, int cellSize, int newWidth)
    {
        this.originalPath = originalPath;
        this.cellSize = cellSize;
        this.newWidth = newWidth;

        init();
    }

    public String getOriginalPath()
    {
        return originalPath;
    }

    public void setOriginalPath(String originalPath)
    {
        this.originalPath = originalPath;
    }

    public int getCellSize()
    {
        return cellSize;
    }

    public void setCellSize(int cellSize)
    {
        this.cellSize = cellSize;
    }

    public int getNewWidth()
    {
        return newWidth;
    }

    public int getNewHeight()
    {
        return newHeight;
    }

    private void init()
    {
        File file = new File(originalPath);
        try
        {
            original = ImageIO.read(file);

            var ratio = newWidth / (double)original.getWidth(null);
            newHeight = (int)(original.getHeight(null) * ratio);

            pixel = Transform(original, newWidth, newHeight);
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
    }

    private Image Transform(Image source, int width, int height)
    {
        var destination = new BufferedImage(width, height,BufferedImage.TYPE_INT_RGB);
        var g = destination.getGraphics();

        g.drawImage(source, 0, 0, width, height,null);
        g.dispose();

        return destination;
    }

    public Image ToPixelImage()
    {
        int width = newWidth * cellSize;
        int height = newHeight * cellSize;

        var destination = new BufferedImage(width, height,BufferedImage.TYPE_INT_RGB);
        var g = destination.getGraphics();
        BufferedImage image = (BufferedImage)pixel;

        for (int row = 0; row < image.getHeight(null); row++)
        {
            for (int col = 0; col < image.getWidth(null); col++)
            {
                try
                {
                    Color color = new Color(image.getRGB(col, row));

                    int x = col * cellSize;
                    int y = row * cellSize;
                    g.setColor(color);
                    g.fillRect(x, y, width, height);
                }
                catch (Exception ex)
                {
                    System.out.println(ex.getMessage());
                    System.out.println("row: " + row);
                    System.out.println("col: " + col);
                    System.out.println();
                }
            }
        }

        return destination;
    }

    public String ToHtml()
    {
        int width = newWidth * cellSize;
        BufferedImage image = (BufferedImage)pixel;

        var builder = new StringBuilder();
        builder.append("<html><head><style>\n")
               .append("body{margin: 0px; display: flex; flex-direction: column;}\n")
               .append(".row{display: flex; flex-direction: row; width: " + width + "px;}\n")
               .append(".cell{height: " + cellSize + "px; width: " + cellSize + "px; overflow: hidden;}\n")
               .append("</style></head><body>\n");


        for (int row = 0; row < image.getHeight(null); row++)
        {
            builder.append("<div class='row'>");
            for (int col = 0; col < image.getWidth(null); col++)
            {
                try
                {
                    Color color = new Color(image.getRGB(col, row));
                    String hexColor = String.format("%06x", 0xFFFFFF & color.getRGB());
                    builder.append("<div class='cell' style='background-color: #" + hexColor + ";'>&nbsp;</div>");
                }
                catch (Exception ex)
                {
                    System.out.println(ex.getMessage());
                    System.out.println("row: " + row);
                    System.out.println("col: " + col);
                    System.out.println();
                }
            }
            builder.append("</div>\n");
        }

        builder.append("</body></html>\n");

        return builder.toString();
    }

    public void saveImage(Image image, String fileName)
    {
        var bufferedImage = new BufferedImage(image.getWidth(null), image.getHeight(null), BufferedImage.TYPE_INT_RGB);
        bufferedImage.getGraphics().drawImage(image,0,0, null);

        try
        {
            ImageIO.write(bufferedImage, "JPG", new File(fileName));
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }
    }

}
