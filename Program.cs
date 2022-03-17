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

            string outputPath = Path.Combine(currentPath, "Output");

            if (!Directory.Exists(Path.Combine(outputPath, "Desktop"))) Directory.CreateDirectory(Path.Combine(outputPath, "Desktop"));

            if (!Directory.Exists(Path.Combine(outputPath, "Mobile"))) Directory.CreateDirectory(Path.Combine(outputPath, "Mobile"));

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

            string imagePath = Path.Combine(Path.Combine(CDNPath, "LocalState"), "Assets");

            string targetPath = outputPath;
            
            if(Directory.Exists(imagePath)){
                string[] files = Directory.GetFiles(imagePath);
                foreach (string s in files)
                {   
                    string filePath = Path.Combine(imagePath, s);
                    FileInfo info = new FileInfo(filePath);
                    if(info.Length > 200000) //filter out the tile icons and bloatware
                    {
                        Bitmap bmp = new Bitmap(filePath);
                        if (bmp.Width == 1920 && bmp.Height == 1080) {
                            targetPath = Path.Combine(outputPath, "Desktop");
                        } else {
                            targetPath = Path.Combine(outputPath, "Mobile");
                        }
                        string fileName = Path.GetFileName(s) + ".png";
                        string destFile = Path.Combine(targetPath, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
            }
        }
    }
}