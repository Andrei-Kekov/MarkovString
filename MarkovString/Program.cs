using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MarkovString
{
    internal class Program
    {
        private const string defaultSampleFileName = "sample.txt";
        private const string defaultOutputFileName = "output.txt";
        private const int defaultLines = 50;
        private const string defaultSeparator = "\r\n";
        private const GeneratorType defaultGenerator = GeneratorType.OneByOne;

        private enum GeneratorType : Byte
        {
            OneByOne,
            OneByTwo,
            OneByThree
        }

        private static void Main(string[] args)
        {
            string sampleFileName;
            string outputFileName;
            int lines;
            GeneratorType generatorType;

            ProcessCommandLineArguments(args, out sampleFileName, out outputFileName, out lines, out generatorType);

            string[] separator = new string[1];
            separator[0] = defaultSeparator;
            string[] sample = ReadSample(sampleFileName).Split(separator, StringSplitOptions.RemoveEmptyEntries);
            IStringGenerator generator = InitializeGenerator(generatorType, sample);
            string[] output = new string[lines];

            try
            {
                for (int i = 0; i < lines; i++)
                {
                    output[i] = generator.GenerateString();
                }
            }
            finally
            {
                SaveOutput(output, outputFileName, sample);
            }
        }

        private static void ProcessCommandLineArguments(string[] args, out string sampleFileName, out string outputFileName, out int lines, out GeneratorType generatorType)
        {
            sampleFileName = defaultSampleFileName;
            outputFileName = defaultOutputFileName;
            lines = defaultLines;
            generatorType = defaultGenerator;

            if (args.Length == 0)
            {
                return;
            }

            int i = 0;

            while (i < args.Length - 1)
            {
                if (args[i] == "-s")
                {
                    i++;
                    sampleFileName = args[i];
                    i++;
                }
                else if (args[i] == "-o")
                {
                    i++;
                    outputFileName = args[i];
                    i++;
                }
                else if (args[i] == "-n")
                {
                    i++;
                    int temp;
                    int.TryParse(args[i], out temp);

                    if (temp > 0)
                    {
                        lines = temp;
                    }
                    else
                    {
                        Console.WriteLine($"Warning: value of \"-n\" parameter is invalid. Proceeding with default value of {defaultLines}.");
                    }

                    i++;
                }
                else if (args[i] == "-g")
                {
                    i++;

                    if (args[i] == "1")
                    {
                        generatorType = GeneratorType.OneByOne;
                    }
                    else if (args[i] == "2")
                    {
                        generatorType = GeneratorType.OneByTwo;
                    }
                    else if (args[i] == "3")
                    {
                        generatorType = GeneratorType.OneByThree;
                    }

                    i++;
                }
                else
                {
                    Console.WriteLine($"Unknown parameter: \"{args[i]}\".");
                    i++;
                }
            }
        }

        private static IStringGenerator InitializeGenerator(GeneratorType generatorType, string[] sample)
        {
            switch (generatorType)
            {
                case GeneratorType.OneByOne:
                    return new OneByOne(sample);
                case GeneratorType.OneByThree:
                    return new OneByThree(sample);
                case GeneratorType.OneByTwo:
                default:
                    return new OneByTwo(sample);
            }
        }

        private static string ReadSample(string fileName)
        {
            FileStream SampleFile = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader SampleReader = new StreamReader(SampleFile, Encoding.Unicode);
            string sample = SampleReader.ReadToEnd();
            SampleFile.Close();
            return sample;
        }

        private static double GetOverfitness(string[] sample, string[] output)
        {
            double temp = (double)output.Count(o => sample.Contains(o)) / (double)output.Length;
            return (double)output.Count(o => sample.Contains(o)) / (double)output.Length;
        }

        private static void SaveOutput(string[] output, string fileName, string[] sample)
        {
            FileStream outputFile = new FileStream(fileName, FileMode.Create);
            StreamWriter outputStream = new StreamWriter(outputFile, Encoding.Unicode);

            foreach (string line in output)
            {
                if (!(line is null))
                {
                    if (sample.Contains(line))
                    {
                        outputStream.Write('*');
                    }

                    outputStream.WriteLine(line);
                }
            }

            outputStream.WriteLine();
            outputStream.Write($"Overfitness is {GetOverfitness(sample, output)}");
            outputStream.Close();
        }
    }
}
