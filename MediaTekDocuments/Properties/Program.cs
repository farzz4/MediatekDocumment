using MediaTekDocuments.view;
using MediaTekDocuments.model;
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

            // ouverture de la fenêtre d'authentification
            FrmAuthentification frmAuthentification = new FrmAuthentification();
            if (frmAuthentification.ShowDialog() == DialogResult.OK)
            {
                Utilisateur utilisateur = frmAuthentification.utilisateurConnecte;

                // service Culture : pas d'accès
                if (utilisateur.IdService == "00003")
                {
                    MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à cette application.", "Accès refusé");
                    return;
                }

                // ouverture de l'application principale
                FrmMediatek frmMediatek = new FrmMediatek(utilisateur);
                Application.Run(frmMediatek);
            }
        }
    }
}
