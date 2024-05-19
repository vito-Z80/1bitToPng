using System;
using System.Collections.Generic;
using System.Linq;
using _1bitToPng.Converter;

namespace _1bitToPng
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var argument = new ArgsParser();
            argument.Help();
            var loader = new LoadBin();
            if (args.Length == 0)
            {
                var files = loader.GetLocalBinFiles();
                MoreFilesProcess(argument, loader, files);
            }
            else
            {
                argument.Parse(args);
                OneFileProcess(argument, loader);
            }

            Console.ReadLine();
        }

        static void MoreFilesProcess(ArgsParser argument, LoadBin loader, IEnumerable<string> files)
        {
            foreach (var filePath in files)
            {
                var fileName = filePath.Split('\\').Last();
                var argsFromFileName = fileName.Split(' ');
                argument.Parse(argsFromFileName);
                argument.FileName = fileName;
                OneFileProcess(argument, loader);
            }
        }


        static void OneFileProcess(ArgsParser argument, LoadBin loader)
        {

            if (argument.w == 0 || argument.h == 0)
            {
                Console.Out.WriteLine($"The dimensions of the sprite file were not specified: {argument.FileName}");
                Console.Out.WriteLine("");
                return;
            }
            
            if (argument.FileName.Contains("."))
            {
                var bytes = loader.GetFileBytes(argument.FileName);
                var converter = new BinToPng(bytes);
                Info(argument);
                converter.CreateSprites(argument.w, argument.h, argument.attr, argument.inv, argument.trans);
                converter.Save(argument.atlas, argument.FileName.Split(' ').Last().Split('.').First());
            }
            else
            {
                Console.Out.WriteLine("Please provide a file name.");
            }

            Console.Out.WriteLine("");
        }


        static void Info(ArgsParser argument)
        {
            Console.Out.WriteLine($"File name: {argument.FileName}");
            Console.Out.WriteLine($"Sprite width: {argument.w}");
            Console.Out.WriteLine($"Sprite height: {argument.h}");
            Console.Out.WriteLine($"Build to atlas: {argument.atlas}");
            Console.Out.WriteLine($"Backdrop transparency: {argument.trans}");
            Console.Out.WriteLine($"Include attributes: {argument.attr}");
            Console.Out.WriteLine($"Invert colors: {argument.inv}");
        }
    }
}