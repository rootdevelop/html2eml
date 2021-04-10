using System;
using System.IO;

namespace Html2Eml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to HTML2EML");
            Console.WriteLine(string.Empty);

            if (args.Length is 0 or > 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: No or to many HTML file(s) given, please append the file you would like to convert, e.g. html2eml filename.html");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(string.Empty);
                return;
            }

            string html;

            try
            {
                html = File.ReadAllText(args[0]);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Unable to read file");
                Console.WriteLine(string.Empty);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Input file is empty");
                Console.WriteLine(string.Empty);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Html2EmlConvert.ConvertHtml(html, $"{args[0].Replace(".html","")}.eml");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("SUCCESS: Conversion complete");
            Console.WriteLine(string.Empty);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
