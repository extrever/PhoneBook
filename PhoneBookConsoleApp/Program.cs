using System.Text;
using System.Xml.Linq;

namespace PhoneBookConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbPath = Directory.GetCurrentDirectory() + "\\Phonebook.csv";
            string cmd = "";
            List<string> Commands = new List<string>()
            {
                "/help",
                "/exit",
                "/list",
                "/search",
                "/add",
                "/delete",
                "/edit"
            };

            do
            {
                BasicGreeting();

                cmd = Console.ReadLine();
                if (Commands.Contains(cmd))
                {
                    switch (cmd)
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
                            AddRecord(dbPath);
                            break;
                        case "/edit":
                            EditRecord(dbPath);
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

        public class PhoneBookRecord
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Email { get; set; }
        }

        private static void EditRecord(string Path)
        {
            var foundRecords = new List<PhoneBookRecord>();            

            HeaderFooterMsg(HeadFootType.header, "Editing a record");
            Console.WriteLine("Choose how to search record to edit:\n" +
                    "1 - by First Name\n" +
                    "2 - by Last Name");

            bool searchTypeResult = int.TryParse(Console.ReadLine(), out int searchType);

            if (searchTypeResult)
            {
                switch (searchType)
                {
                    case 1:
                        Console.WriteLine("\nEnter First Name: ");
                        string firstName = Console.ReadLine();
                        try
                        {
                            string[,] csv_values = LoadCSV(Path);
                            foundRecords = SearchResultListOfObj(csv_values, firstName, searchType);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Programm encoureted an error: " + ex);
                        }
                        break;
                    case 2:
                        Console.WriteLine("\nEnter Last Name: ");
                        string lastName = Console.ReadLine();
                        try
                        {
                            string[,] csv_values = LoadCSV(Path);
                            foundRecords = SearchResultListOfObj(csv_values, lastName, searchType);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Programm encoureted an error: " + ex);
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong search Id, try again...");
                        break;
                }

                if (foundRecords.Count >= 1)
                {
                    Console.WriteLine("\nThese were found:");
                    //TODO: apply formating like in List method, Console.Write("{0, -25}{1, 0}", csv_values[r, c], "|");
                    Console.WriteLine("{0, -3}{1, 0}{2, -15}{3, 0}{4, -25}{5, 0}{6, -15}{7, 0}", "Id", "|", "FirstName", "|", "LastName", "|", "PhoneNumber", "|", "Email", "|");
                    foreach (var recordObj in foundRecords)
                    {
                        Console.WriteLine("{0, -3}{1, 0}{2, -15}{3, 0}{4, -25}{5, 0}{6, -15}{7, 0}", recordObj.Id, "|", recordObj.FirstName, "|", recordObj.LastName, "|", recordObj.PhoneNumber, "|", recordObj.Email);
                    }

                    Console.WriteLine("Enter Id of the record you want to edit: ");
                    bool recordToEditParseResult = int.TryParse(Console.ReadLine(), out int recordId);

                    if (recordToEditParseResult)
                    {
                        if (foundRecords.Any(x => x.Id == recordId))
                        {
                            Console.WriteLine("Enter updated record in the correct format and press enter.");
                            Console.WriteLine("Format - [FirstName],[LastName],[Phone],[Email]");
                            Console.WriteLine("If some data is unavailable enter n/a.");

                            string editedRecord = Console.ReadLine();
                            //TODO: stoped here
                            //Need to change .csv file structure to have an ID for each record
                        }
                        else
                        {
                            Console.WriteLine("You entered wrong Id, try again"); //TODO: change to do while??? recursion????
                        }
                    }
                    else
                    {
                        Console.WriteLine("You entered not a valid Id, try again");
                    }
                }
            }
            else
            {
                //TODO: Rethink... recursion?
                Console.WriteLine("Wrong search type, try again...");

            }

            HeaderFooterMsg(HeadFootType.footer);
        }

        private static void AddRecord(string Path)
        {
            HeaderFooterMsg(HeadFootType.header, "Adding a record to the Phone Book");

            Console.WriteLine("Enter new record in the correct format and press enter.");
            Console.WriteLine("Format - [FirstName],[LastName],[Phone],[Email]");
            Console.WriteLine("If some data is unavailable enter n//a.");
            string newRecord = Console.ReadLine();

            //TODO: change to 'try/catch'
            if (File.Exists(Path))
            {
                //TODO: Add more comprehensive handling for incorrect data entry
                if (newRecord != "")
                {
                    File.AppendAllText(Path, "\n" + newRecord);
                    Console.WriteLine("New record added.");
                }
                else
                {
                    Console.WriteLine("You entered empty string, try again.");
                }
            }

            HeaderFooterMsg(HeadFootType.footer);
        }

        private static void SearchRecord(string Path)
        {
            //TODO: change to 'try/catch'
            if (File.Exists(Path))
            {
                HeaderFooterMsg(HeadFootType.header, "Search for a Record");

                Console.WriteLine("Choose how to search:\n" +
                    "1 - by First Name\n" +
                    "2 - by Last Name");

                //int searchSelection;
                bool success = int.TryParse(Console.ReadLine(), out int searchSelection);

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

                HeaderFooterMsg(HeadFootType.footer);
            }
        }

        private static void SearchResult(string[,] csv_values, string name, int searchBy)
        {
            //TODO: maybe change method to return array/list of the found strings???
            //SearchBy: 1 - First Name, 2 - Last Name

            StringBuilder searchResultSB = new StringBuilder();

            switch (searchBy)
            {
                case 1:
                    for (int r = 1; r < csv_values.GetLength(0); r++)
                    {
                        if (csv_values[r, 0] == name)
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

        private static List<PhoneBookRecord> SearchResultListOfObj(string[,] csv_values, string name, int searchBy)
        {
            //Search the PhoneBook and return a list of oblects representing PhoneBook records
            //SearchBy: 1 - First Name, 2 - Last Name

            List<PhoneBookRecord> foundRecords = new List<PhoneBookRecord>();

            switch (searchBy)
            {
                case 1:
                    for (int r = 1; r < csv_values.GetLength(0); r++)
                    {
                        if (csv_values[r, 0] == name)
                        {
                            foundRecords.Add(new PhoneBookRecord
                            {
                                Id = r,
                                FirstName = csv_values[r, 0],
                                LastName = csv_values[r, 1],
                                PhoneNumber = csv_values[r, 2],
                                Email = csv_values[r, 3]
                            });
                        }
                    }
                    if (foundRecords.Count >= 1)
                    {
                        return foundRecords;
                    }
                    else
                    {
                        Console.WriteLine("\nNothing found.");
                    }
                    break;
                case 2:
                    for (int r = 1; r < csv_values.GetLength(0); r++)
                    {
                        if (csv_values[r, 1] == name)
                        {
                            foundRecords.Add(new PhoneBookRecord
                            {
                                Id = r,
                                FirstName = csv_values[r, 0],
                                LastName = csv_values[r, 1],
                                PhoneNumber = csv_values[r, 2],
                                Email = csv_values[r, 3]
                            });
                        }
                    }
                    if (foundRecords.Count >= 1)
                    {
                        return foundRecords;
                    }
                    else
                    {
                        Console.WriteLine("\nNothing found.");
                    }
                    break;                    
                default:
                    Console.WriteLine("\nIncorrect SearchBy argument.");
                    break;
            }
            return null;
        }

        private static void ListPhoneBook(string Path)
        {
            //TODO: change to 'try/catch'
            //TODO: change search method to the Search To List Of Obj method
            if (File.Exists(Path))
            {
                HeaderFooterMsg(HeadFootType.header, "PhoneBook Listing");

                string[,] csv_values = LoadCSV(Path);

                //Display header
                for (int r = 0; r < 1; r++)
                {
                    for (int c = 0; c < csv_values.GetLength(1); c++)
                    {
                        switch (c)
                        {
                            //TODO: try to get rid off fixed column number
                            case 0:
                                Console.Write("{0, -5}{1, 0}", csv_values[r, c], "|");
                                break;
                            case 4:
                                Console.Write("{0, -25}{1, 0}", csv_values[r, c], "|");
                                break;
                            default:
                                Console.Write("{0, -15}{1, 0}", csv_values[r, c], "|");
                                break;
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < 79; i++)
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
                            case 0:
                                Console.Write("{0, -5}{1, 0}", csv_values[r, c], "|");
                                break;
                            case 4:
                                Console.Write("{0, -25}{1, 0}", csv_values[r, c], "|");
                                break;
                            default:
                                Console.Write("{0, -15}{1, 0}", csv_values[r, c], "|");
                                break;
                        }
                    }
                    Console.WriteLine();
                }
                HeaderFooterMsg(HeadFootType.footer);
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
            HeaderFooterMsg(HeadFootType.header, "Help");
            Console.WriteLine("List of the available commands:\n\n" +
                "/help - loads this screen\n" +
                "/exit - exit PhoneBook application\n" +
                "/list - list all entries from the Phone Book\n" +
                "/search - search for a record in the Phone Book by First or Last name\n" +
                "/add - add new record to the Phone Book\n" +
                "/edit - edit record in the Phone Book\n" +
                "To be continue...");
            HeaderFooterMsg(HeadFootType.footer);
        }

        private static void BasicGreeting()
        {
            Console.Clear();
            Console.WriteLine("Pnone Book App v0.0.1");
            Console.WriteLine();
            Console.WriteLine($"Please enter your command or type /help for help.");
        }

        private static void HeaderFooterMsg(HeadFootType type, string addText = "")
        {
            switch (type)
            {
                case HeadFootType.header:
                    Console.Clear();
                    Console.WriteLine("Pnone Book App v0.0.1 - " + addText);
                    Console.WriteLine();
                    break;
                case HeadFootType.footer:
                    Console.WriteLine();
                    Console.WriteLine("Pres any key to return to the main menu.");
                    Console.ReadKey();
                    break;
                default:
                    break;

                    //Use Enum for argument type. Define argument type as created Enum type.
            }
        }

        public enum HeadFootType
        {
            header,
            footer
        }
    }
}