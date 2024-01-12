using System.Text;
using ChikoRokoBot.Sorter.Extensions;

namespace ChikoRokoBot.Sorter.Helpers
{
    public class TelegramMarkdownSanitizer
    {
        public static string Sanitize(string content)
        {
            StringBuilder builder = new StringBuilder(content.LimitTo(1024));

            builder
                .Replace("-", "\\-")
                .Replace(".", "\\.")
                .Replace("!", "\\!")
                .Replace("<u>", string.Empty)
                .Replace("</u>", string.Empty)
                .Replace("`", "\\`")
                .Replace("=", "\\=")
                .Replace("(", "\\(")
                .Replace(")", "\\)");

            return builder.ToString();
        }
    }
}

