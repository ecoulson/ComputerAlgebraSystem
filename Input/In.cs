using System;
namespace Input
{
    public static class In
    {
        public static double GetDouble()
        {
            try
            {
                return double.Parse(Console.ReadLine());
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to parse double from STDIN", exception);
            }
        }

        public static int GetInt()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to parse int from STDIN", exception);
            }
        }

        public static string GetString()
        {
            return Console.ReadLine();
        }
    }
}
