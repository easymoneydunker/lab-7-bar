using System;
using System.Windows.Forms;

namespace MmfLab
{
    internal static class Program
    {
        // Точка входа в программу
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
