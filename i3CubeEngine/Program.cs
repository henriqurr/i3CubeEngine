using i3CubeEngine.Forms;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace i3CubeEngine
{
    public static class Program
    {
        public volatile static EngineForm form = null;
        [STAThread]
        public static void Main()
        {
            //string st =
            //    @"F:\Музыка\Pink.Floyd-Discography.1967-2014.MP3\01 - Studio albums\Vinyl 1st issues\1968 - A Saucerful Of Secrets (LP) [EMI-Columbia, SCX 6258]\Artwork\1968 - A Saucerful Of Secrets (LP) [EMI-Columbia, SCX 6258]\1968 - A Saucerful Of Secrets (LP) [";
            //int l = st.Length;
            //var sa = st.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);

            CultureInfo ci = new CultureInfo("en");
            CultureInfo.DefaultThreadCurrentCulture = ci;
            CultureInfo.DefaultThreadCurrentUICulture = ci;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form = new EngineForm());
        }
    }
}