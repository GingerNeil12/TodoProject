namespace TodoProject.Helpers
{
    public static class StringHelpers
    {
        public static string CapitalizeFirstLetter(this string data)
        {
            return char.ToUpper(data[0]) + data.Substring(1);
        }
    }
}
