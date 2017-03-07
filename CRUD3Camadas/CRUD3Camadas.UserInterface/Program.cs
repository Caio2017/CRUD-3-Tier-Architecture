//User Interface Layer = UIL, ou Graphical User Interface = GUI (camada de visão/apresentação)
using System;
using System.Windows.Forms;

namespace CRUD3Camadas
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());           
        }
    }
}
