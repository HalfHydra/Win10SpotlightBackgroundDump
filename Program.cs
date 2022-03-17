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

            string CDNPath = null;

            string[] subDirs = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages"));
            foreach (string dir in subDirs)
            {
                if(dir.IndexOf("Microsoft.Windows.ContentDeliveryManager") != -1) 
                {
                    CDNPath = dir;
                    break;
                }
            }

            if(CDNPath == null)
            {
                Console.Write("Could not find the Microsoft.Windows.ContentDeliveryManager folder");
                return;
            }

            string currentPath = Directory.GetCurrentDirectory();

            string desktopPath = Path.Combine(currentPath, "Output\\Desktop");

            string mobilePath = Path.Combine(currentPath, "Output\\Mobile");

            if (!Directory.Exists(desktopPath)) 
            {
                Directory.CreateDirectory(desktopPath);
                Directory.CreateDirectory(mobilePath);
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
                        targetPath = desktopPath;
                    } else if (bmp.Width == 1080 && bmp.Height == 1920) {
                        targetPath = mobilePath;
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