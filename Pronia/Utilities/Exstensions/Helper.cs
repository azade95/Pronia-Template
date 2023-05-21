using System.Text.RegularExpressions;

namespace Pronia.Utilities.Exstensions
{
    public static class Helper
    {
        public static string Capitalize(this string word)
        {
            word = word.ToLower();
            word = word[0].ToString().ToUpper() + word.Substring(1);
            return word;
        }

        public static bool CheckEmail(this string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (regex.IsMatch(email)) return true;
            return false;
        }

    }
}
