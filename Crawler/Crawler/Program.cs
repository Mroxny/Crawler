namespace Crawler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            if (args.Length > 1)
            {
                throw new ArgumentException();
            }

            string[] emails = getEmails(args[0]);

            foreach (string s in emails) {
                Console.WriteLine(s);
            }
        }

        static async string[] getEmails(string url) {
            using HttpClient httpClient = new();

            HttpResponseMessage resul = await httpClient.GetAsync(url).Result;
            

            //httpClient.Dispose();
            return new string[0];

        }
    }
}