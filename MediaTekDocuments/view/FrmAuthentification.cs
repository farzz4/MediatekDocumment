using MediaTekDocuments.controller;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaTekDocuments.view
{
    public partial class FrmAuthentification : Form
    {
        private readonly FrmAuthentificationController controller;  

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        public FrmAuthentification()
        {
            InitializeComponent();
            this.controller = new FrmAuthentificationController();
        }

        /// <summary>
        /// Connexion à l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (!txbLogin.Text.Equals("") && !txbPwd.Text.Equals(""))
            {
                string login = txbLogin.Text;
                string pwd = txbPwd.Text;
                Utilisateur utilisateur = controller.GetUtilisateur(login, pwd);
                if (utilisateur == null)
                {
                    MessageBox.Show("Authentification incorrecte");
                    txbPwd.Text = "";
                }
                else if (utilisateur.Libelle == "culture")
                {
                    MessageBox.Show("Vous n'avez pas les droits suffisants pour accéder à l'application");
                    Application.Exit();
                }
                else
                {
                    this.Hide();
                    MessageBox.Show("Vous êtes connecté(e) à l'application");
                    FrmMediatek frmMediatek = new FrmMediatek(utilisateur.Libelle);
                    frmMediatek.ShowDialog();
                    
                }
            }
            else { MessageBox.Show("Tous les champs doivent être remplis"); }
        }
    }
}
