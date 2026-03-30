using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        private readonly FrmMediatekController controller;
        /// <summary>
        /// Utilisateur connecté, accessible depuis l'extérieur
        /// </summary>
        public Utilisateur utilisateurConnecte = null;

        public FrmAuthentification()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        /// <summary>
        /// Clic sur Se connecter : vérifie les identifiants
        /// </summary>
        private void btnAuthentification_Click(object sender, EventArgs e)
        {
            if (txbLogin.Text.Equals("") || txbPwd.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un login et un mot de passe.", "Information");
                return;
            }

            Utilisateur utilisateur = controller.GetUtilisateur(txbLogin.Text, txbPwd.Text);

            if (utilisateur != null)
            {
                utilisateurConnecte = utilisateur;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Login ou mot de passe incorrect.", "Erreur");
                txbPwd.Text = "";
                txbLogin.Focus();
            }
        }
    }
}
