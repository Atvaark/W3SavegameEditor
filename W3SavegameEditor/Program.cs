using System;
using System.Collections.Generic;
using System.IO;
using W3SavegameEditor.ChunkedLz4;
using W3SavegameEditor.Savegame;

namespace W3SavegameEditor
{
    static class Program
    {
        private const string InputOption = "-i";
        private const string OutputOption = "-o";

        static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> parsedArgs = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i += 2)
            {
                var option = args[i];
                if (i % 2 == 1)
                {
                    throw new ArgumentException("missing argument for option: " + option);
                }

                switch (option)
                {
                    case InputOption:
                    case OutputOption:
                        parsedArgs[option] = args[i + 1];
                        break;
                    default:
                        throw new ArgumentException("unknown option: " + option);
                }
            }

            return parsedArgs;
        }


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("W3SavegameEditor by Atvaark\n" +
                                  "Description:\n" +
                                  "  Unpacking tool for Witcher 3 savegame files.\n" +
                                  "Usage:\n" +
                                  "  W3SavegameEditor -i input_path [-o output_path]\n" +
                                  "Options:\n" +
                                  "  -i  Path to the input file (default location is %USERPROFILE%\\Documents\\The Witcher 3\\gamesaves)\n" +
                                  "  -o  (optional) Path to the output file.\n");
                return;
            }

            Dictionary<string, string> parsedArgs;
            try
            {
                parsedArgs = ParseArgs(args);
            }
            catch (ArgumentException e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }
            
            string inputSaveFilePath;
            if (!parsedArgs.TryGetValue(InputOption, out inputSaveFilePath))
            {
                Console.Error.WriteLine("error: missing input file");
                return;
            }
            
            string outputSaveFilePath;
            if (!parsedArgs.TryGetValue(OutputOption, out outputSaveFilePath))
            {
                string saveName = Path.GetFileNameWithoutExtension(inputSaveFilePath);
                string saveDirectory = Path.GetDirectoryName(inputSaveFilePath);
                if (saveDirectory == null)
                {
                    Console.Error.WriteLine("error: unable to read input file directory");
                    return;
                }

                outputSaveFilePath = Path.Combine(saveDirectory, saveName + "_dec.sav");
            }

            try
            {
                UnpackSaveFile(inputSaveFilePath, outputSaveFilePath);
                Console.WriteLine("done");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("error: failed unpacking the savegame\n{0}", e);
            }
        }

        private static void UnpackSaveFile(string inputSaveFilePath, string outputSaveFilePath)
        {
            using (var compressedInputStream = File.OpenRead(inputSaveFilePath))
            using (var inputStream = ChunkedLz4File.Decompress(compressedInputStream))
            using (var outputStream = File.OpenWrite(outputSaveFilePath))
            {
                var savegame = SavegameFile.Read(inputStream);

                //inputStream.CopyTo(outputStream);
            }
        }
    }
}
