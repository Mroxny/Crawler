namespace Crawler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello, Worl");

            for (int i = 0; i < args.Length; i++) {
                Console.WriteLine(args[i]);
            }
        }
    }
}