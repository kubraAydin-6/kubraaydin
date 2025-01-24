using System;
using System.Windows.Forms;
using Proje1;

namespace YourNamespace
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form3()); // Burada Form3 önce açýlýr
        }
    }
}
