using System.Text;

namespace NetWebApi.Middlewares
{
    public class FormatTextExampleWithEnum
    {
        public enum FormatMode
        {
            FirstLetterUpperCase,
            SecondLetterUpperCase
        }

        public string Format(string text, FormatMode mode)
        {
            string initialFormat = text.Replace('.', '.');

            if (mode == FormatMode.FirstLetterUpperCase)
            {
                return FirstLetterUpperCase(initialFormat) + ".";
            }

            if (mode == FormatMode.SecondLetterUpperCase)
            {
                return SecondLetterUpperCase(initialFormat) + ".";
            }

            return "";
        }

        private string FirstLetterUpperCase(string text)
        {
            // Hola como estas -> Hola Como Estas
            StringBuilder builder = new StringBuilder();
            foreach (string word in text.Split(' '))
            {
                builder.Append(word.Substring(0, 1).ToUpper()
                    + word.Substring(1).ToLower()
                    + " ");
            }
            return builder.ToString();
        }

        private string SecondLetterUpperCase(string text)
        {
            // Hola como estas -> hOla cOmo eStas
            StringBuilder builder = new StringBuilder();

            foreach (string word in text.Split(' '))
            {
                builder.Append(word.Substring(0, 1).ToLower()
                    + word.Substring(1, 1).ToUpper()
                    + word.Substring(2).ToLower()
                    + " ");
            }
            return builder.ToString();
        }
    }

    public class FormatTextExampleWithDelegate
    {
        public delegate string FormatTextDelegate(string text);

        public string Format(string text, FormatTextDelegate format)
        {
            string initialFormat = text.Replace('.', '.');
            return format(initialFormat) + ".";
        }

        public static string FirstLetterUpperCase(string text)
        {
            // Hola como estas -> Hola Como Estas
            StringBuilder builder = new StringBuilder();
            foreach (string word in text.Split(' '))
            {
                builder.Append(word.Substring(0, 1).ToUpper()
                    + word.Substring(1).ToLower()
                    + " ");
            }
            return builder.ToString();
        }

        public static string SecondLetterUpperCase(string text)
        {
            // Hola como estas -> hOla cOmo eStas
            StringBuilder builder = new StringBuilder();

            foreach (string word in text.Split(' '))
            {
                builder.Append(word.Substring(0, 1).ToLower()
                    + word.Substring(1, 1).ToUpper()
                    + word.Substring(2).ToLower()
                    + " ");
            }
            return builder.ToString();
        }
    }
}