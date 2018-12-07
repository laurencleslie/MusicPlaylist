using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAnalyzer
{
    public class Song
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Size { get; set; }
        public int Time { get; set; }
        public int Year { get; set; }
        public int Plays { get; set; }

        public string getData()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", Name, Artist, Album, Genre, Size, Time, Year, Plays);
        }

         public string song(string songName, string artistName, string albumName, string genreName, int fileSize, int songLength, int songYear, int numOfPlays)
         { 
            Name = songName; 
            Artist = artistName; 
            Album = albumName; 
            Genre = genreName; 
            Size = fileSize; 
            Time = songLength; 
            Year = songYear; 
            Plays = numOfPlays;
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", Name, Artist, Album, Genre, Size, Time, Year, Plays);
         }  
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("\nThe correct input format is: MusicPlaylistAnalyzer.exe <music_playlist_file_path> <report_file_path>");
                Environment.Exit(0);
            }
            
        List<Song> swData = new List<Song>();

            try
            {
                using (StreamReader sr = new StreamReader(args[0]))
                {
                    var lineNumber = 0;
                    var NumItemsInRow = 8;
                    sr.ReadLine();

                       while (!sr.EndOfStream)
                       {
                           string line = sr.ReadLine();
                           lineNumber++;
                           string[] values = line.Split('\t');

                             if (values.Length < 8)
                             {
                               Console.WriteLine($"\nError: Row {lineNumber} contains {values.Length} values. It is supposed to contain {NumItemsInRow}. Please go to that line, input the missing data, and then try to run the program again.");
                               Environment.Exit(0);
                             }

                             else if (values.Length > 8)
                             {
                               Console.WriteLine($"\nError: Row {lineNumber} contains {values.Length} values. It is only supposed to contain {NumItemsInRow}. Please go to that line, remove the extra data, and then try to run the program again.");
                               Environment.Exit(0);
                             }

                            try
                            {
                                var musicData = new Song
                                {
                                    Name = values[0],
                                    Artist = values[1],
                                    Album = values[2],
                                    Genre = values[3],
                                    Size = Int32.Parse(values[4]),
                                    Time = Int32.Parse(values[5]),
                                    Year = Int32.Parse(values[6]),
                                    Plays = Int32.Parse(values[7])
                                };
                              swData.Add(musicData);
                            }

                            catch (Exception)
                            {
                                Console.WriteLine($"\nError: Row {lineNumber} contains inputs of the wrong data type.");
                                Environment.Exit(0);
                            }        
                       }
                   sr.Close();
                }
            }

            // catches any other problems reading the input file
            catch (Exception reader)
            {
                Console.WriteLine("\nError: " + reader.Message);
                Environment.Exit(0);
            }
  
            try
            {
                using (StreamWriter sw = new StreamWriter(args[1]))
                {
                    Song[] songs = swData.ToArray();
                    int counter = 0;
                    sw.WriteLine("Music Playlist Report:\n");

                    var isitOver199 = from song in songs where song.Plays >= 200 select song;
                    sw.WriteLine("\n");
                    sw.WriteLine("1. Songs That Received 200 Or More Plays: ");
                    foreach (Song song in isitOver199)
                    {
                        sw.WriteLine(song.getData());
                    }
                    sw.WriteLine("\n");

                    var isitAlternative = from song in songs where song.Genre == "Alternative" select song;
                    foreach (Song song in isitAlternative)
                    {
                        counter++;
                    }
                    sw.WriteLine("2. Number Of Alternative Songs: " + counter + "\n");

                    counter = 0;
                    var isitHipHop = from song in songs where song.Genre == "Hip-Hop/Rap" select song;
                    foreach (Song song in isitHipHop)
                    {
                        counter++;
                    }
                    sw.WriteLine("\n");
                    sw.WriteLine("3. Number Of Hip-Hop/Rap Songs: " + counter + "\n");
                    sw.WriteLine("\n");

                    var isitFromTheFishbowl = from song in songs where song.Album == "Welcome to the Fishbowl" select song;
                    sw.WriteLine("\n4. Songs From The Album \"Welcome To The Fishbowl\": ");
                    foreach (Song song in isitFromTheFishbowl)
                    {
                        sw.WriteLine(song.getData());
                    }
                    sw.WriteLine("\n");

                    var isitBefore1970 = from song in songs where song.Year < 1970 select song;
                    sw.WriteLine("5. Songs From Before 1970: ");
                    foreach (Song song in isitBefore1970)
                    {
                        sw.WriteLine(song.getData());
                    }
                    sw.WriteLine("\n");

                    var isitLongerThan85 = from song in songs where song.Name.Length > 85 select song;
                    sw.WriteLine("6. Song Names Longer Than 85 Characters: ");
                    foreach (Song song in isitLongerThan85)
                    {
                        sw.WriteLine(song.getData());
                    }
                    sw.WriteLine("\n");

                    var isitTheLongest = from song in songs orderby song.Time descending select song;
                    var longestSong = isitTheLongest.First();
                    sw.WriteLine("7. Longest Song: ");
                    sw.WriteLine(longestSong.getData());

                    sw.Close();
                }

                Console.WriteLine("\nYour Music Playlist Report has been generated! It is named " + args[1] + " and can be found in the repo folder!");
                Environment.Exit(0);

            }

            // catches any problems writing to output file
             catch (Exception writer)
            {
                Console.WriteLine("\nError: " + writer.Message);
                Environment.Exit(0);
            }
          Console.ReadLine();
        }
    }
}