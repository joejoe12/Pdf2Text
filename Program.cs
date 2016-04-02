using System;
using System.IO;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Pdf2Text
{
	class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            args = new string[2];
            args[0] = @"L:\temp\9912514615219.pdf";
            args[1] = @"L:\temp\fff.txt";
            DateTime start = DateTime.Now;
			if (args.Length < 2)
			{
				Console.WriteLine("Usage: PDF2TEXT <input filename (PDF)> <output filename (text)>");
				return;
			}

			using (StreamWriter sw = new StreamWriter(args[1]))
			{
				sw.WriteLine(parseUsingPDFBox(args[0]));
			}

			Console.WriteLine("Done. Took " + (DateTime.Now - start));

            //			Console.ReadLine();

		}

		private static string parseUsingPDFBox(string input)
		{
		    PDDocument doc = null;

            try
            {
                doc = PDDocument.load(input);
                PDFTextStripper stripper = new PDFTextStripper();
                return stripper.getText(doc);
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
		}


        public void Recognize(Bitmap bitmap)
        {
            bitmap.Save("temp.png", ImageFormat.Png);
            var startInfo = new ProcessStartInfo("tesseract.exe", "temp.png temp hocr");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(startInfo);
            process.WaitForExit();

            GetWords(File.ReadAllText("temp.html"));

            // Futher actions with words
        }

        public Dictionary<Rectangle, string> GetWords(string tesseractHtml)
        {
            var xml = XDocument.Parse(tesseractHtml);

            var rectsWords = new Dictionary<System.Drawing.Rectangle, string>();

            //var ocr_words = xml.Descendants("span").L.Where(element => element.Attribute("class").Value == "ocr_word").ToList();
            //foreach (var ocr_word in ocr_words)
            //{
            //    var strs = ocr_word.Attribute("title").Value.Split(' ');
            //    int left = int.Parse(strs[1]);
            //    int top = int.Parse(strs[2]);
            //    int width = int.Parse(strs[3]) - left + 1;
            //    int height = int.Parse(strs[4]) - top + 1;
            //    rectsWords.Add(new Rectangle(left, top, width, height), ocr_word.Value);
            //}

            return rectsWords;
        }
    }
}
