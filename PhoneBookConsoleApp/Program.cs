namespace PhoneBookConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                    }
                }
                else
                {
                    Console.WriteLine("Wrong command, try again. Press any key to continue.");
                    Console.ReadKey();
                }

            } while (cmd != "/exit");
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