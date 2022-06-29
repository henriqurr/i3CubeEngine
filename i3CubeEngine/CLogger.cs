using System;
using System.Windows.Forms;

namespace i3CubeEngine
{
    public class CLogger
    {
        public static void Exception(Exception ex)
        {
            MessageBox.Show($"{ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            WriteLine(ex.ToString());
        }

        public static void Error(string text)
        {
            MessageBox.Show(text, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            WriteLine(text);  
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}