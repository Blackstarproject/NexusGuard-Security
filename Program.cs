using System;
using System.Windows.Forms;

namespace FuturisticAntivirus
{
    #region Main Application Entry Point
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    #endregion
}