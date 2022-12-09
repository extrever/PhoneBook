using System.Text;

namespace PhoneBookConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbPath = Directory.GetCurrentDirectory() + "\\Phonebook.csv";
            string cmd = "";
            string[] cmdInput = { "/help", "/exit", "/list", "/search", "/add", "/delete", "/edit" };
            List<string> Commands = new List<string>(cmdInput);
            
            do
            {
                BasicGreeting();
                
                cmd = Console.ReadLine();
                if (Commands.Contains(cmd))
                {
                    switch(cmd)
                    {
                        case "/help":
                            Help();
                            break;

                        case "/exit":
                            break;

                        case "/list":
                            ListPhoneBook(dbPath);
                            break;

                        case "/search":
                            SearchRecord(dbPath);
                            break;
                        case "/add":
                            AddRecord();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong command, try again. Press any key to continue.");
                    Console.ReadKey();
                }

            } while (cmd != "/exit");
        }

        private static void AddRecord()
        {
            Console.Clear();
            Console.WriteLine("Pnone Book App v0.0.1 - Adding a record to the Phone Book");
            Console.WriteLine();

            Console.WriteLine("Adding a new record...");

            Console.WriteLine();
            Console.WriteLine("Pres any key to return to the main menu.");
            Console.ReadKey();
        }

        private static void SearchRecord(string Path)
        {
            //TODO: change to 'try/catch'
            if(File.Exists(Path))
            {                
                Console.Clear();
                Console.WriteLine("Pnone Book App v0.0.1 - Search for a Record");
                Console.WriteLine();

                Console.WriteLine("Choose how to search:\n" +
                    "1 - by First Name\n" +
                    "2 - by Last Name");
                                
                int searchSelection;
                bool success = int.TryParse(Console.ReadLine(), out searchSelection);

                string[,] csv_values = LoadCSV(Path);

                if (success)
                {
                    switch (searchSelection)
                    {
                        case 1:
                            Console.WriteLine("\nEnter First Name:");
                            string firstName = Console.ReadLine();
                            SearchResult(csv_values, firstName, searchSelection);
                            break;
                        case 2:
                            Console.WriteLine("\nEnter Last Name:");
                            string lastName = Console.ReadLine();
                            SearchResult(csv_values, lastName, searchSelection);
                            break;
                        default:
                            Console.WriteLine("Incorrect selection.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect selection.");
                }

                Console.WriteLine();
                Console.WriteLine("Pres any key to return to the main menu.");
                Console.ReadKey();
            }
        }

        private static void SearchResult(string[,] csv_values, string name, int searchBy)
        {
            //SearchBy: 1 - First Name, 2 - Last Name

            StringBuilder searchResultSB = new StringBuilder();

            switch (searchBy)
            {
                case 1:
                    for (int r = 1; r < csv_values.GetLength(0); r++)
                    {
                        if (csv_values[r, 0] == name)
                        {
                            searchResultSB.Append("Found:" + " " + csv_values[r, 0] + " " + csv_values[r, 1] + " " +  csv_values[r, 2] + " " + csv_values[r, 3] + "\n");
                        }
                    }
                    if (searchResultSB.Length > 0)
                    {
                        Console.WriteLine("\n" + searchResultSB);
                    }
                    else
                        Console.WriteLine("\nNothing found.");
                    break;
                case 2:
                    for (int r = 1; r < csv_values.GetLength(0); r++)
                    {
                        if (csv_values[r, 1] == name)
                        {
                            searchResultSB.Append("Found:" + " " + csv_values[r, 0] + " " + csv_values[r, 1] + " " + csv_values[r, 2] + " " + csv_values[r, 3] + "\n");
                        }
                    }
                    if (searchResultSB.Length > 0)
                    {
                        Console.WriteLine("\n" + searchResultSB);
                    }
                    else
                        Console.WriteLine("\nNothing found.");
                    break;
            }            
        }

        private static void ListPhoneBook(string Path)
        {
            //TODO: change to 'try/catch'
            if (File.Exists(Path))
            {
                Console.Clear();
                Console.WriteLine("Pnone Book App v0.0.1 - PhoneBook Listing");
                Console.WriteLine();

                string[,] csv_values = LoadCSV(Path);

                //Display header
                for (int r = 0; r < 1; r++)
                {
                    for (int c = 0; c < csv_values.GetLength(1); c++)
                    {
                        switch (c)
                        {
                            //TODO: try to get rid off fixed column number
                            case 3:
                                Console.Write("{0, -25}{1, 0}", csv_values[r, c], "|");
                                break;
                            default:
                                Console.Write("{0, -15}{1, 0}", csv_values[r, c], "|");
                                break;
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < 74; i++)
                    {
                        sb.Append("_");
                    }
                    Console.WriteLine("\n" + sb);                    
                }

                //Display rows
                for (int r = 1; r < csv_values.GetLength(0); r++)
                {
                    for (int c = 0; c < csv_values.GetLength(1); c++)
                    {
                        switch (c)
                        {
                            case 3:
                                Console.Write("{0, -25}{1, 0}", csv_values[r, c], "|");
                                break;
                            default:
                                Console.Write("{0, -15}{1, 0}", csv_values[r, c], "|");
                                break;
                        }                        
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("Pres any key to return to the main menu.");
                Console.ReadKey();
            }
        }

        private static string[,] LoadCSV(string Path)
        {
            //TODO: add 'try/catch'
            string[] lines = File.ReadAllLines(Path);

            //Check how many rows and columns in the CSV file
            int rows_num = lines.Length;
            int cols_num = lines[0].Split(',').Length;

            //Allocate the array to load the CSV file
            string[,] csv_values = new string[rows_num, cols_num];

            //Load the array
            for (int r = 0; r < rows_num; r++)
            {
                string[] line_r = lines[r].Split(',');

                for (int c = 0; c < cols_num; c++)
                {
                    csv_values[r, c] = line_r[c];
                }

            }

            return csv_values;
        }

        private static void Help()
        {
            Console.Clear();
            Console.WriteLine("Pnone Book App v0.0.1 - Help");
            Console.WriteLine();
            Console.WriteLine("List of the availabel commands:\n\n" +
                "/help - loads this screen\n" +
                "/exit - exit PhoneBook application\n" +
                "/list - list all entries from the Phone Book\n" +
                "/search - search for a record in the Phone Book by First or Last name\n" +
                "/add - add new record to the Phone Book\n" +
                "To be continue...");
            Console.WriteLine();
            Console.WriteLine("Pres any key to return to the main menu.");
            Console.ReadKey();
        }

        private static void BasicGreeting()
        {
            Console.Clear();
            Console.WriteLine("Pnone Book App v0.0.1");
            Console.WriteLine();
            Console.WriteLine($"Please enter your command or type /help for help.");
        }
    }
}