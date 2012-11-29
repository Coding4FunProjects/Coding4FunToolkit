namespace Coding4Fun.Toolkit.Tool.XamlMerger
{
    public static class StringExtentions
    {
        public static string TrimStart(this string target, string value)
        {
            var result = target;

            while (result.StartsWith(value))
            {
                result = result.Substring(value.Length);
            }

            return result;
        }

        public static string TrimEnd(this string target, string value)
        {
            var result = target;

            while (result.EndsWith(value))
            {
                result = result.Substring(0, result.Length - value.Length);
            }

            return result;
        }
    }
}
