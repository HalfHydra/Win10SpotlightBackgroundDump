using System;
using System.IO;
using System.Drawing;

namespace WindowsBackgroundDump
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Windows Background Extractor\n>");

            string currentPath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.Combine(currentPath, "Output"))) 
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Output\\Desktop"));
                Directory.CreateDirectory(Path.Combine(currentPath, "Output\\Mobile"));
            }

            string CDNPath = "";

            string[] subdirectories = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages"));
            foreach (string directory in subdirectories)
            {
                if(directory.IndexOf("Microsoft.Windows.ContentDeliveryManager") != -1) 
                {
                    CDNPath = directory;
                    break;
                }
            }

            string imagePath = Path.Combine(CDNPath, "LocalState\\Assets");

            string targetPath = currentPath;
            
            if(Directory.Exists(imagePath)){
                string[] files = Directory.GetFiles(imagePath);
                foreach (string s in files)
                {   
                    string filePath = Path.Combine(imagePath, s);
                    Bitmap bmp = new Bitmap(filePath);
                    if (bmp.Width == 1920 && bmp.Height == 1080) {
                        targetPath = Path.Combine(currentPath, "Output\\Desktop");
                    } else if (bmp.Width == 1080 && bmp.Height == 1920) {
                        targetPath = Path.Combine(currentPath, "Output\\Mobile");
                    } else {
                        continue; //Bloatware, Tile Icons, etc.
                    }
                    string fileName = Path.GetFileName(s) + ".png";
                    string destFile = Path.Combine(targetPath, fileName);
                    File.Copy(s, destFile, true);
                    Console.Write("Copied image to " + destFile + "\n");
                }
            }
        }
    }
}