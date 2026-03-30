using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    public partial class FrmAlerte : Form
    {
        private readonly BindingSource bdgAlerte = new BindingSource();

        /// <summary>
        /// Constructeur : reçoit la liste des abonnements expirant bientôt
        /// </summary>
        public FrmAlerte(List<Abonnement> lesAbonnements)
        {
            InitializeComponent();
            RemplirListe(lesAbonnements);
        }

        /// <summary>
        /// Remplit le datagrid avec la liste des abonnements
        /// </summary>
        private void RemplirListe(List<Abonnement> lesAbonnements)
        {
            bdgAlerte.DataSource = lesAbonnements;
            dgvAlerte.DataSource = bdgAlerte;
            dgvAlerte.Columns["Id"].Visible = false;
            dgvAlerte.Columns["IdRevue"].Visible = false;
            dgvAlerte.Columns["Montant"].Visible = false;
            dgvAlerte.Columns["DateCommande"].Visible = false;
            dgvAlerte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Ferme la fenêtre
        /// </summary>
        private void btnAlerteOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
