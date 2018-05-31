using System.Linq;

namespace Cross.Helpers
{
    public class TextHelper
    {
        public static string CapitalizeEachWord(string text)
        {
            string[] words = text.Split(' ');
            string result = string.Empty;

            foreach (string word in words)
            {
                result = string.IsNullOrEmpty(result) ? string.Empty : $"{result} ";
                result += $"{word.First().ToString().ToUpper()}{word.Substring(1).ToLower()}";
            }

            return result;
        }
    }
}
