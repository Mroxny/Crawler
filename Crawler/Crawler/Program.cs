using System.Text.RegularExpressions;

namespace Crawler
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length <= 0)
            {
                throw new ArgumentNullException();
            }

            if (args.Length > 1)
            {
                throw new ArgumentException();
            }

            HashSet<string> emails = await GetEmails(args[0]);


            foreach (string s in emails) {
                Console.WriteLine(s);
            }
        }

        private static async Task<HashSet<string>> GetEmails(string url) {
            string urlPattern = "https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_\\+.~#?&//=]*)";
            Regex urlRegex = new(urlPattern);

            // invalid url
            if (!urlRegex.IsMatch(url)) {
                throw new ArgumentException();
            }

            using HttpClient httpClient = new();
            HttpResponseMessage result = await httpClient.GetAsync(url);

            // invalid http response
            if (!result.IsSuccessStatusCode) {
                throw new Exception("Bląd " + result.StatusCode + "przy pobieraniu strony");
            }

            string htmlContent = await result.Content.ReadAsStringAsync();
            string emailPattern = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
            Regex emailRegex = new(emailPattern, RegexOptions.IgnoreCase);
            MatchCollection matchEmails =  emailRegex.Matches(htmlContent);

            // no emails found
            if (matchEmails.Count <= 0) {
                throw new Exception("Na stronie nie znaleziono adresów email");
            }

            HashSet<string> uniqueEmails = new();
            foreach (var email in matchEmails) {
                uniqueEmails.Add(email.ToString());
            }

            return uniqueEmails;

        }
    }
}