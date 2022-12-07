namespace PhoneBookConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string DbPath = Directory.GetCurrentDirectory() + "\\Phonebook.csv";
            string cmd = "";
            string[] CmdInput = { "/help", "/exit", "/list", "/search", "/add", "/delete", "/edit" };
            List<string> Commands = new List<string>(CmdInput);
            
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
                            ListPhoneBook(DbPath);
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

        private static void ListPhoneBook(string Path)
        {           
            if (File.Exists(Path))
            {
                Console.Clear();
                Console.WriteLine("Pnone Book App v0.0.1 - PhoneBook Listing");
                Console.WriteLine();                                

                string[] lines= File.ReadAllLines(Path);
                
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

                for (int r = 0; r < csv_values.GetLength(0); r++)
                {
                    for (int c = 0; c < csv_values.GetLength(1); c++)
                    {
                        // TODO: fix string formating - pipes and alignment
                        Console.Write("{0, -15} {1, 5}", csv_values[r, c], "|");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("Pres any key to return to the main menu.");
                Console.ReadKey();
            }
        }

        private static void Help()
        {
            Console.Clear();
            Console.WriteLine("Pnone Book App v0.0.1 - Help");
            Console.WriteLine();
            Console.WriteLine("Here will be a help text...");
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