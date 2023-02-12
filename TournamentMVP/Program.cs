using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace TournamentMVP
{
    class Program
    {
        static void Main(string[] args)
        {
            var sportDataFiles = Directory.GetFiles(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\TournamentMVP\Sports\"))).ToList();


            Console.WriteLine("Welcome to the Tucan Tournament.");
            Console.WriteLine("============================================================================================================");
            Console.WriteLine("Selected tournaments statistics would you like to see?\n");

            string selectedFile = string.Empty;
            string selectedFileName = string.Empty;

            do
            {
                Console.WriteLine("Select Sport Data File");
                for (int i = 0; i < sportDataFiles.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {Path.GetFileName(sportDataFiles[i])}");
                }

                string sourceIndexStr = Console.ReadLine();
                int sourceIndex;

                if (int.TryParse(sourceIndexStr, out sourceIndex) && sourceIndex > 0 && sourceIndex <= sportDataFiles.Count)
                {
                    selectedFile = sportDataFiles[sourceIndex - 1];
                    Console.WriteLine($"\nSelected  {Path.GetFileName(selectedFile)} as sport data file.\n");
                    selectedFileName = Path.GetFileName(selectedFile);
                }
                else if (sourceIndexStr == "q")
                {
                    Console.WriteLine("\nOk... Bye.");
                    return;
                }
                else
                {
                    Console.WriteLine($"\n{sourceIndexStr} is not a valid source selection.");
                }
                Console.WriteLine("============================================================================================================");
                Console.WriteLine("\n");
            } while (sportDataFiles == null);

            // Read files and calculate player rating points
            string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, selectedFile));
            if (lines.Length < 2)
            {
                Console.WriteLine($"Invalid file format : {selectedFile}");
                return;
            }

            var mvpPlayer = new Players();

            switch (selectedFileName)
            {
                case "Basketball.txt":
                    mvpPlayer = new Basketball().Calculate(lines.ToList());
                    break;
                case "Handball.txt":
                    mvpPlayer = new Handball().Calculate(lines.ToList());
                    break;
                default:
                    break;
            }


            Console.WriteLine($"=======> MVP: {mvpPlayer.Name} Point : {mvpPlayer.RatingPoints}");
            Console.WriteLine($"=======> Winning Team : {mvpPlayer.WinningTeam} Point : {mvpPlayer.TeamScore}");
            Console.ReadLine();

        }

        static KeyValuePair<string, int> TucanReadFile(string path)
        {
            string line = "";
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            string[] parameters = line.Split(',');
                            return new KeyValuePair<string, int>(parameters[0], int.Parse(parameters[1]));
                        }
                    }
                }
            }
            return new KeyValuePair<string, int>();
        }
    }
}
