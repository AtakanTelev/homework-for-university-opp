using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LibrarySystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string filePath = "books.txt";
            List<Book> physicalBooks = new List<Book>();
            List<EBook> downloadableEBooks = new List<EBook>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("books.txt not found!");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    string type = parts[0].Trim().ToUpper();
                    string title = parts[1].Trim();
                    string author = parts[2].Trim();

                    if (type == "BOOK")
                    {
                        physicalBooks.Add(new Book(title, author));
                    }
                    else if (type == "EBOOK" && parts.Length == 4)
                    {
                        string url = parts[3].Trim();
                        downloadableEBooks.Add(new EBook(title, author, url));
                    }
                }
            }

            Console.WriteLine("=== LIBRARY SYSTEM ===");

            while (true)
            {
                Console.WriteLine("\n--- Available Physical Books ---");
                foreach (var book in physicalBooks)
                {
                    book.DisplayInfo();
                }

                Console.WriteLine("\n--- Downloadable EBooks ---");
                for (int i = 0; i < downloadableEBooks.Count; i++)
                {
                    Console.WriteLine($"[{i + 1}] {downloadableEBooks[i].Title} by {downloadableEBooks[i].Author}");
                }

                Console.WriteLine("\nType the number of the EBook to download, or type 'exit' to quit:");
                string input = Console.ReadLine();

                if (input.Trim().ToLower() == "exit")
                {
                    break;
                }

                if (int.TryParse(input, out int index))
                {
                    if (index >= 1 && index <= downloadableEBooks.Count)
                    {
                        await downloadableEBooks[index - 1].DownloadAsync();
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }

            Console.WriteLine($"\nTotal EBooks Downloaded: {EBook.TotalDownloads}");
            Console.WriteLine("Program ended.");
        }
    }
}

