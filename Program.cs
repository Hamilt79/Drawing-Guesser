using System.Runtime.InteropServices;

namespace Drawing_Guesser
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);

            ApplicationConfiguration.Initialize();
            
            Application.Run(new Form1() { Text = "Drawing Guesser", Icon =  Properties.Resources.draw});
        }
    }
}