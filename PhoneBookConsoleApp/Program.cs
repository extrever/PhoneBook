namespace PhoneBookConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cmd = "";

            do
            {
                BasicGreeting();

                cmd = Console.ReadLine();
            } while (cmd != "/exit");
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