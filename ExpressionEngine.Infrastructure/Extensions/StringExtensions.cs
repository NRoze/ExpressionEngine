using System.Text;

namespace ExpressionEngine.Infrastructure.Extensions
{
    static public class StringExtensions
    {
        extension(string source)
        {
            public string RemoveWhitespace()
            {
                var sb = new StringBuilder(source.Length);

                foreach (var c in source)
                    if (!char.IsWhiteSpace(c))
                        sb.Append(c);

                return sb.ToString();
            }

            public string RepeatString(int times)
            {
                if (times <= 0) return string.Empty;
                if (times == 1) return source;

                var sb = new StringBuilder(source.Length * times);

                for (int i = 0; i < times; i++)
                    sb.Append(source);

                return sb.ToString();
            }

            public string RemoveSubstring(string toRemove)
            {
                if (string.IsNullOrEmpty(toRemove))
                    return source;

                var sb = new StringBuilder(source.Length);
                int pos = 0;

                while (pos < source.Length)
                {
                    int idx = source.IndexOf(toRemove, pos, StringComparison.Ordinal);

                    if (idx < 0)
                    {
                        sb.Append(source[pos..]);
                        break;
                    }

                    sb.Append(source[pos..idx]);
                    pos = idx + toRemove.Length;
                }

                return sb.ToString();
            }
        }
    }
}
