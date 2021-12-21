package net.igregor.minecrafter;

import net.igregor.minecrafter.models.ImageConverter;

import java.io.*;
import java.util.Scanner;

public class App
{
    public static void main(String[] args)
    {
        Scanner input = new Scanner(System.in);

        String originalPath;
        String saveToHtmlPath;
        String saveToImagePath;
        int cellSize;
        int newWidth;

        System.out.println();
        System.out.print("Enter the path of the image to convert? ");
        originalPath = input.nextLine();

        System.out.println();
        System.out.print("How big do you want each pixel? ");
        cellSize = Integer.parseInt(input.nextLine());

        System.out.println();
        System.out.print("How many pixels wide do you want the new picture? ");
        newWidth = Integer.parseInt(input.nextLine());

        System.out.println();
        System.out.print("Enter the path and name of the html file? ");
        saveToHtmlPath = input.nextLine();

        System.out.println();
        System.out.print("Enter the path and name of the image file? ");
        saveToImagePath = input.nextLine();

        var converter = new ImageConverter(originalPath, cellSize, newWidth);
        File log = new File(saveToHtmlPath);

        try(FileOutputStream imageStream = new FileOutputStream(log,false);
            PrintWriter imageWriter = new PrintWriter(imageStream))
        {
            var html = converter.ToHtml();
            imageWriter.println(html);

            var image = converter.ToPixelImage();
            converter.saveImage(image,saveToImagePath);
        }
        catch(FileNotFoundException e)
        {
            System.out.println(e.getMessage());
        }
        catch (IOException e)
        {
            e.printStackTrace();
        }

    }
}
