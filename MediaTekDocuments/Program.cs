using MediaTekDocuments.view;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmConnexion frmConnexion = new FrmConnexion();
            if (frmConnexion.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmMediatek(frmConnexion.UtilisateurConnecte));
            }
        }
    }
}
