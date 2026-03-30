using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();

        private readonly BindingSource bdgGenresSaisie = new BindingSource();
        private readonly BindingSource bdgPublicsSaisie = new BindingSource();
        private readonly BindingSource bdgRayonsSaisie = new BindingSource();
        private readonly BindingSource bdgLivresExemplaires = new BindingSource();
        private readonly BindingSource bdgLivresEtats = new BindingSource();

        private readonly BindingSource bdgGenresDvdSaisie = new BindingSource();
        private readonly BindingSource bdgPublicsDvdSaisie = new BindingSource();
        private readonly BindingSource bdgRayonsDvdSaisie = new BindingSource();
        private readonly BindingSource bdgDvdExemplaires = new BindingSource();
        private readonly BindingSource bdgDvdEtats = new BindingSource();

        private readonly BindingSource bdgGenresRevuesSaisie = new BindingSource();
        private readonly BindingSource bdgPublicsRevuesSaisie = new BindingSource();
        private readonly BindingSource bdgRayonsRevuesSaisie = new BindingSource();

        private readonly BindingSource bdgReceptionEtats = new BindingSource();

        private readonly Utilisateur utilisateur;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.utilisateur = utilisateur;
            GererAcces();
            // ouverture de la fenêtre d'alerte au démarrage
            AfficheAlerteAbonnements();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Ouvre la fenêtre d'alerte si des abonnements expirent bientôt
        /// </summary>
        private void AfficheAlerteAbonnements()
        {
            if (utilisateur.IdService == "00001")
            {
                List<Abonnement> lesAbonnements = controller.GetAbonnementsExpirantBientot();
                if (lesAbonnements.Count > 0)
                {
                    FrmAlerte frmAlerte = new FrmAlerte(lesAbonnements);
                    frmAlerte.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Gère les accès selon le service de l'utilisateur connecté
        /// </summary>
        private void GererAcces()
        {
            // service Prêts : accès en consultation seulement
            if (utilisateur.IdService == "00002")
            {
                // onglets commandes invisibles
                tabOngletsApplication.TabPages.Remove(tabCommandesLivres);
                tabOngletsApplication.TabPages.Remove(tabCommandesDvd);
                tabOngletsApplication.TabPages.Remove(tabCommandesRevues);
                // boutons ajout/modif/suppression invisibles dans livres
                btnLivresAjouter.Visible = false;
                btnLivresModifier.Visible = false;
                btnLivresSupprimer.Visible = false;
                btnLivresEnregistrer.Visible = false;
                btnLivresAnnuler.Visible = false;
                // boutons ajout/modif/suppression invisibles dans dvd
                btnDvdAjouter.Visible = false;
                btnDvdModifier.Visible = false;
                btnDvdSupprimer.Visible = false;
                btnDvdEnregistrer.Visible = false;
                btnDvdAnnuler.Visible = false;
                // boutons ajout/modif/suppression invisibles dans revues
                btnRevuesAjouter.Visible = false;
                btnRevuesModifier.Visible = false;
                btnRevuesSupprimer.Visible = false;
                btnRevuesEnregistrer.Visible = false;
                btnRevuesAnnuler.Visible = false;
                // groupbox exemplaires invisibles
                grbLivresEtatExemplaire.Visible = false;
                grbDvdEtat.Visible = false;
                grbParutionsEtat.Visible = false;
            }
        }
        #endregion

        #region Onglet Livres
        private bool modeAjoutLivre = false;

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresSaisie, cbxLivresGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsSaisie, cbxLivresPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsSaisie, cbxLivresRayon);

            bdgLivresEtats.DataSource = controller.GetAllEtats();
            cbxLivresEtat.DataSource = bdgLivresEtats;
            cbxLivresEtat.DisplayMember = "Libelle";
            cbxLivresEtat.ValueMember = "Id";
            cbxLivresEtat.SelectedIndex = -1;

            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche avec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            cbxLivresGenre.Text = livre.Genre;
            cbxLivresPublic.Text = livre.Public;
            cbxLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            cbxLivresGenre.SelectedIndex = -1;
            cbxLivresPublic.SelectedIndex = -1;
            cbxLivresRayon.SelectedIndex = -1;
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre et ses exemplaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                    RemplirLivresExemplaires(livre.Id);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Active ou désactive les champs de saisie des livres
        /// </summary>
        /// <param name="actif">true = saisie possible, false = lecture seule</param>
        private void LivresChampsModifiables(bool actif)
        {
            txbLivresNumero.ReadOnly = !actif;
            txbLivresTitre.ReadOnly = !actif;
            txbLivresAuteur.ReadOnly = !actif;
            txbLivresIsbn.ReadOnly = !actif;
            txbLivresCollection.ReadOnly = !actif;
            txbLivresImage.ReadOnly = !actif;
            cbxLivresGenre.Enabled = actif;
            cbxLivresPublic.Enabled = actif;
            cbxLivresRayon.Enabled = actif;
            btnLivresEnregistrer.Visible = actif;
            btnLivresAnnuler.Visible = actif;
            btnLivresAjouter.Enabled = !actif;
            btnLivresModifier.Enabled = !actif;
            btnLivresSupprimer.Enabled = !actif;
        }

        /// <summary>
        /// Clic sur Ajouter : vide le formulaire et active les champs
        /// </summary>
        private void btnLivresAjouter_Click(object sender, EventArgs e)
        {
            modeAjoutLivre = true;
            VideLivresInfos();
            LivresChampsModifiables(true);
            txbLivresNumero.Focus();
        }

        /// <summary>
        /// Clic sur Modifier : active les champs pour modifier le livre sélectionné
        /// </summary>
        private void btnLivresModifier_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un livre à modifier.", "Information");
                return;
            }
            modeAjoutLivre = false;
            LivresChampsModifiables(true);
            // l'id ne doit pas être modifiable
            txbLivresNumero.ReadOnly = true;
            txbLivresTitre.Focus();
        }

        /// <summary>
        /// Clic sur Supprimer : supprime le livre sélectionné
        /// </summary>
        private void btnLivresSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un livre à supprimer.", "Information");
                return;
            }

            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer le livre : " + livre.Titre + " ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerLivre(livre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    MessageBox.Show("Livre supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce livre.\nIl possède peut-être des exemplaires ou des commandes.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Clic sur Enregistrer : crée le livre dans la BDD
        /// </summary>
        private void btnLivresEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbLivresTitre.Text.Equals("") || txbLivresAuteur.Text.Equals("") ||
                txbLivresIsbn.Text.Equals("") ||
                cbxLivresGenre.SelectedIndex < 0 || cbxLivresPublic.SelectedIndex < 0 ||
                cbxLivresRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }
            // si mode ajout, on vérifie aussi que le numéro est rempli
            if (modeAjoutLivre && txbLivresNumero.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir le numéro.", "Information");
                return;
            }

            string id = txbLivresNumero.Text;
            string titre = txbLivresTitre.Text;
            string image = txbLivresImage.Text;
            string isbn = txbLivresIsbn.Text;
            string auteur = txbLivresAuteur.Text;
            string collection = txbLivresCollection.Text;

            Genre genre = (Genre)cbxLivresGenre.SelectedItem;
            Public lePublic = (Public)cbxLivresPublic.SelectedItem;
            Rayon rayon = (Rayon)cbxLivresRayon.SelectedItem;

            Livre livre = new Livre(id, titre, image, isbn, auteur, collection,
                genre.Id, genre.Libelle, lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);

            if (modeAjoutLivre)
            {
                // mode ajout
                if (controller.CreerLivre(livre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    LivresChampsModifiables(false);
                    MessageBox.Show("Livre ajouté avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout. Vérifiez que le numéro n'existe pas déjà.", "Erreur");
                }
            }
            else
            {
                // mode modification
                if (controller.ModifierLivre(livre))
                {
                    lesLivres = controller.GetAllLivres();
                    RemplirLivresListeComplete();
                    LivresChampsModifiables(false);
                    MessageBox.Show("Livre modifié avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Clic sur Annuler : vide les champs et repasse en lecture seule
        /// </summary>
        private void btnLivresAnnuler_Click(object sender, EventArgs e)
        {
            VideLivresInfos();
            LivresChampsModifiables(false);
        }

        /// <summary>
        /// Remplit le datagrid des exemplaires du livre sélectionné
        /// </summary>
        private void RemplirLivresExemplaires(string idLivre)
        {
            List<Exemplaire> lesExemplaires = controller.GetExemplairesRevue(idLivre);
            bdgLivresExemplaires.DataSource = lesExemplaires;
            dgvLivresExemplaires.DataSource = bdgLivresExemplaires;
            dgvLivresExemplaires.Columns["Id"].Visible = false;
            dgvLivresExemplaires.Columns["Photo"].Visible = false;
            dgvLivresExemplaires.Columns["IdEtat"].Visible = false;
            dgvLivresExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Tri sur les colonnes des exemplaires
        /// </summary>
        private void dgvLivresExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvLivresExemplaires.Columns[e.ColumnIndex].HeaderText;
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            List<Exemplaire> lesExemplaires = controller.GetExemplairesRevue(livre.Id);
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Libelle":
                    sortedList = lesExemplaires.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            bdgLivresExemplaires.DataSource = sortedList;
            dgvLivresExemplaires.DataSource = bdgLivresExemplaires;
        }

        /// <summary>
        /// Modifie l'état de l'exemplaire sélectionné
        /// </summary>
        private void btnLivresModifierEtat_Click(object sender, EventArgs e)
        {
            if (dgvLivresExemplaires.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }
            if (cbxLivresEtat.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un état.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgLivresExemplaires.List[bdgLivresExemplaires.Position];
            Etat etat = (Etat)cbxLivresEtat.SelectedItem;
            exemplaire.IdEtat = etat.Id;

            if (controller.ModifierEtatExemplaire(exemplaire))
            {
                Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                RemplirLivresExemplaires(livre.Id);
                MessageBox.Show("État modifié avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification de l'état.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime l'exemplaire sélectionné
        /// </summary>
        private void btnLivresSupprimerExemplaire_Click(object sender, EventArgs e)
        {
            if (dgvLivresExemplaires.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgLivresExemplaires.List[bdgLivresExemplaires.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet exemplaire ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    RemplirLivresExemplaires(livre.Id);
                    MessageBox.Show("Exemplaire supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Dvd
        private bool modeAjoutDvd = false;

        private readonly BindingSource bdgDvdListe = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxDvdRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresDvdSaisie, cbxDvdGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsDvdSaisie, cbxDvdPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsDvdSaisie, cbxDvdRayon);

            bdgDvdEtats.DataSource = controller.GetAllEtats();
            cbxDvdEtat.DataSource = bdgDvdEtats;
            cbxDvdEtat.DisplayMember = "Libelle";
            cbxDvdEtat.ValueMember = "Id";
            cbxDvdEtat.SelectedIndex = -1;

            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            cbxDvdGenre.Text = dvd.Genre;
            cbxDvdPublic.Text = dvd.Public;
            cbxDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            cbxDvdGenre.SelectedIndex = -1;
            cbxDvdPublic.SelectedIndex = -1;
            cbxDvdRayon.SelectedIndex = -1;
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd et ses exemplaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                    RemplirDvdExemplaires(dvd.Id);
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        /// <summary>
        /// Active ou désactive les champs de saisie des dvd
        /// </summary>
        private void DvdChampsModifiables(bool actif)
        {
            txbDvdNumero.ReadOnly = !actif;
            txbDvdTitre.ReadOnly = !actif;
            txbDvdRealisateur.ReadOnly = !actif;
            txbDvdSynopsis.ReadOnly = !actif;
            txbDvdDuree.ReadOnly = !actif;
            txbDvdImage.ReadOnly = !actif;
            cbxDvdGenre.Enabled = actif;
            cbxDvdPublic.Enabled = actif;
            cbxDvdRayon.Enabled = actif;
            btnDvdEnregistrer.Visible = actif;
            btnDvdAnnuler.Visible = actif;
            btnDvdAjouter.Enabled = !actif;
            btnDvdModifier.Enabled = !actif;
            btnDvdSupprimer.Enabled = !actif;
        }

        /// <summary>
        /// Clic sur Ajouter : vide le formulaire et active les champs
        /// </summary>
        private void btnDvdAjouter_Click(object sender, EventArgs e)
        {
            modeAjoutDvd = true;
            VideDvdInfos();
            DvdChampsModifiables(true);
            txbDvdNumero.Focus();
        }

        /// <summary>
        /// Clic sur Modifier : active les champs pour modifier le dvd sélectionné
        /// </summary>
        private void btnDvdModifier_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un DVD à modifier.", "Information");
                return;
            }
            modeAjoutDvd = false;
            DvdChampsModifiables(true);
            // l'id ne doit pas être modifiable
            txbDvdNumero.ReadOnly = true;
            txbDvdTitre.Focus();
        }

        /// <summary>
        /// Clic sur Supprimer : supprime le dvd sélectionné
        /// </summary>
        private void btnDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un DVD à supprimer.", "Information");
                return;
            }

            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer le DVD : " + dvd.Titre + " ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerDvd(dvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    MessageBox.Show("DVD supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce DVD.\nIl possède peut-être des exemplaires ou des commandes.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Clic sur Enregistrer : crée le dvd dans la BDD
        /// </summary>
        private void btnDvdEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbDvdTitre.Text.Equals("") || txbDvdRealisateur.Text.Equals("") ||
                cbxDvdGenre.SelectedIndex < 0 || cbxDvdPublic.SelectedIndex < 0 ||
                cbxDvdRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }
            if (modeAjoutDvd && txbDvdNumero.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir le numéro.", "Information");
                return;
            }

            string id = txbDvdNumero.Text;
            string titre = txbDvdTitre.Text;
            string image = txbDvdImage.Text;
            string realisateur = txbDvdRealisateur.Text;
            string synopsis = txbDvdSynopsis.Text;
            int duree = 0;
            if (!txbDvdDuree.Text.Equals(""))
            {
                int.TryParse(txbDvdDuree.Text, out duree);
            }

            Genre genre = (Genre)cbxDvdGenre.SelectedItem;
            Public lePublic = (Public)cbxDvdPublic.SelectedItem;
            Rayon rayon = (Rayon)cbxDvdRayon.SelectedItem;

            Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, 
                genre.Id, genre.Libelle, lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);

            if (modeAjoutDvd)
            {
                if (controller.CreerDvd(dvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    DvdChampsModifiables(false);
                    MessageBox.Show("DVD ajouté avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout. Vérifiez que le numéro n'existe pas déjà.", "Erreur");
                }
            }
            else
            {
                if (controller.ModifierDvd(dvd))
                {
                    lesDvd = controller.GetAllDvd();
                    RemplirDvdListeComplete();
                    DvdChampsModifiables(false);
                    MessageBox.Show("DVD modifié avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Clic sur Annuler : vide les champs et repasse en lecture seule
        /// </summary>
        private void btnDvdAnnuler_Click(object sender, EventArgs e)
        {
            VideDvdInfos();
            DvdChampsModifiables(false);
        }

        /// <summary>
        /// Remplit le datagrid des exemplaires du dvd sélectionné
        /// </summary>
        private void RemplirDvdExemplaires(string idDvd)
        {
            List<Exemplaire> lesExemplaires = controller.GetExemplairesRevue(idDvd);
            bdgDvdExemplaires.DataSource = lesExemplaires;
            dgvDvdExemplaires.DataSource = bdgDvdExemplaires;
            dgvDvdExemplaires.Columns["Id"].Visible = false;
            dgvDvdExemplaires.Columns["Photo"].Visible = false;
            dgvDvdExemplaires.Columns["IdEtat"].Visible = false;
            dgvDvdExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Tri sur les colonnes des exemplaires
        /// </summary>
        private void dgvDvdExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvDvdExemplaires.Columns[e.ColumnIndex].HeaderText;
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            List<Exemplaire> lesExemplaires = controller.GetExemplairesRevue(dvd.Id);
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Libelle":
                    sortedList = lesExemplaires.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            bdgDvdExemplaires.DataSource = sortedList;
            dgvDvdExemplaires.DataSource = bdgDvdExemplaires;
        }

        /// <summary>
        /// Modifie l'état de l'exemplaire sélectionné
        /// </summary>
        private void btnDvdModiferEtat_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplaires.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }
            if (cbxDvdEtat.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un état.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];
            Etat etat = (Etat)cbxDvdEtat.SelectedItem;
            exemplaire.IdEtat = etat.Id;

            if (controller.ModifierEtatExemplaire(exemplaire))
            {
                Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                RemplirDvdExemplaires(dvd.Id);
                MessageBox.Show("État modifié avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification de l'état.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime l'exemplaire sélectionné
        /// </summary>
        private void btnDvdSupprimerExemplaire_Click(object sender, EventArgs e)
        {
            if (dgvDvdExemplaires.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet exemplaire ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    RemplirDvdExemplaires(dvd.Id);
                    MessageBox.Show("Exemplaire supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Revues
        private bool modeAjoutRevue = false;

        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            RemplirComboCategorie(controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);

            RemplirComboCategorie(controller.GetAllGenres(), bdgGenresRevuesSaisie, cbxRevuesGenre);
            RemplirComboCategorie(controller.GetAllPublics(), bdgPublicsRevuesSaisie, cbxRevuesPublic);
            RemplirComboCategorie(controller.GetAllRayons(), bdgRayonsRevuesSaisie, cbxRevuesRayon);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            cbxRevuesGenre.Text = revue.Genre;
            cbxRevuesPublic.Text = revue.Public;
            cbxRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            cbxRevuesGenre.SelectedIndex = -1;
            cbxRevuesPublic.SelectedIndex = -1;
            cbxRevuesRayon.SelectedIndex = -1;
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        /// <summary>
        /// Active ou désactive les champs de saisie des revues
        /// </summary>
        private void RevuesChampsModifiables(bool actif)
        {
            txbRevuesNumero.ReadOnly = !actif;
            txbRevuesTitre.ReadOnly = !actif;
            txbRevuesPeriodicite.ReadOnly = !actif;
            txbRevuesDateMiseADispo.ReadOnly = !actif;
            txbRevuesImage.ReadOnly = !actif;
            cbxRevuesGenre.Enabled = actif;
            cbxRevuesPublic.Enabled = actif;
            cbxRevuesRayon.Enabled = actif;
            btnRevuesEnregistrer.Visible = actif;
            btnRevuesAnnuler.Visible = actif;
            btnRevuesAjouter.Enabled = !actif;
            btnRevuesModifier.Enabled = !actif;
            btnRevuesSupprimer.Enabled = !actif;
        }

        /// <summary>
        /// Clic sur Ajouter : vide le formulaire et active les champs
        /// </summary>
        private void btnRevuesAjouter_Click(object sender, EventArgs e)
        {
            modeAjoutRevue = true;
            VideRevuesInfos();
            RevuesChampsModifiables(true);
            txbRevuesNumero.Focus();
        }

        /// <summary>
        /// Clic sur Modifier : active les champs pour modifier la revue sélectionnée
        /// </summary>
        private void btnRevuesModifier_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une revue à modifier.", "Information");
                return;
            }
            modeAjoutRevue = false;
            RevuesChampsModifiables(true);
            // l'id ne doit pas être modifiable
            txbRevuesNumero.ReadOnly = true;
            txbRevuesTitre.Focus();
        }

        /// <summary>
        /// Clic sur Supprimer : supprime la revue sélectionnée
        /// </summary>
        private void btnRevuesSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une revue à supprimer.", "Information");
                return;
            }

            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer la revue : " + revue.Titre + " ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerRevue(revue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    MessageBox.Show("Revue supprimée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer cette revue.\nElle possède peut-être des exemplaires ou des abonnements.", "Erreur");
                }
            }
        }

        /// <summary>
        /// Clic sur Annuler : vide les champs et repasse en lecture seule
        /// </summary>
        private void btnRevuesAnnuler_Click(object sender, EventArgs e)
        {
            VideRevuesInfos();
            RevuesChampsModifiables(false);
        }

        /// <summary>
        /// Clic sur Enregistrer : crée la revue dans la BDD
        /// </summary>
        private void btnRevuesEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbRevuesTitre.Text.Equals("") || txbRevuesPeriodicite.Text.Equals("") ||
                cbxRevuesGenre.SelectedIndex < 0 || cbxRevuesPublic.SelectedIndex < 0 ||
                cbxRevuesRayon.SelectedIndex < 0)
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }
            if (modeAjoutRevue && txbRevuesNumero.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir le numéro.", "Information");
                return;
            }

            string id = txbRevuesNumero.Text;
            string titre = txbRevuesTitre.Text;
            string image = txbRevuesImage.Text;
            string periodicite = txbRevuesPeriodicite.Text;
            int delaiMiseADispo = 0;
            if (!txbRevuesDateMiseADispo.Text.Equals(""))
            {
                int.TryParse(txbRevuesDateMiseADispo.Text, out delaiMiseADispo);
            }

            Genre genre = (Genre)cbxRevuesGenre.SelectedItem;
            Public lePublic = (Public)cbxRevuesPublic.SelectedItem;
            Rayon rayon = (Rayon)cbxRevuesRayon.SelectedItem;

            Revue revue = new Revue(id, titre, image, genre.Id, genre.Libelle,
                lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle, periodicite, delaiMiseADispo);

            if (modeAjoutRevue)
            {
                if (controller.CreerRevue(revue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    RevuesChampsModifiables(false);
                    MessageBox.Show("Revue ajoutée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout. Vérifiez que le numéro n'existe pas déjà.", "Erreur");
                }
            }
            else
            {
                if (controller.ModifierRevue(revue))
                {
                    lesRevues = controller.GetAllRevues();
                    RemplirRevuesListeComplete();
                    RevuesChampsModifiables(false);
                    MessageBox.Show("Revue modifiée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la modification.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Parutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";

            bdgReceptionEtats.DataSource = controller.GetAllEtats();
            cbxReceptionEtat.DataSource = bdgReceptionEtats;
            cbxReceptionEtat.DisplayMember = "Libelle";
            cbxReceptionEtat.ValueMember = "Id";
            cbxReceptionEtat.SelectedIndex = -1;
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.Columns["Photo"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Libelle":
                    sortedList = lesExemplaires.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        /// <summary>
        /// Modifie l'état de l'exemplaire sélectionné dans les parutions
        /// </summary>
        private void btnReceptionModifierEtat_Click(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }
            if (cbxReceptionEtat.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un état.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
            Etat etat = (Etat)cbxReceptionEtat.SelectedItem;
            exemplaire.IdEtat = etat.Id;

            if (controller.ModifierEtatExemplaire(exemplaire))
            {
                AfficheReceptionExemplairesRevue();
                MessageBox.Show("État modifié avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification de l'état.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime l'exemplaire sélectionné dans les parutions
        /// </summary>
        private void btnReceptionSupprimerExemplaire_Click(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un exemplaire.", "Information");
                return;
            }

            Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet exemplaire ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                {
                    AfficheReceptionExemplairesRevue();
                    MessageBox.Show("Exemplaire supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur");
                }
            }
        }
        #endregion

        #region Onglet Commandes Livres

        private readonly BindingSource bdgCommandesLivres = new BindingSource();
        private readonly BindingSource bdgSuivisLivres = new BindingSource();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();

        /// <summary>
        /// Ouverture de l'onglet commandes livres : vide les champs
        /// </summary>
        private void tabCommandesLivres_Enter(object sender, EventArgs e)
        {
            RemplirComboCategorie(controller.GetAllSuivis(), bdgSuivisLivres, cbxCommandesLivresSuivi);
            VideCommandesLivresInfos();
        }

        /// <summary>
        /// Vide les informations du livre
        /// </summary>
        private void VideCommandesLivresInfos()
        {
            txbCommandesLivresISBN.Text = "";
            txbCommandesLivresTitre.Text = "";
            txbCommandesLivresAuteur.Text = "";
            txbCommandesLivresCollection.Text = "";
            txbCommandesLivresGenre.Text = "";
            txbCommandesLivresPublic.Text = "";
            txbCommandesLivresRayon.Text = "";
            txbCommandesLivresImage.Text = "";
            pcbCommandesLivresImage.Image = null;
            bdgCommandesLivres.DataSource = null;
            dgvCommandesLivres.DataSource = null;
            VideCommandesLivresSaisie();
        }

        /// <summary>
        /// Vide les champs de saisie d'une nouvelle commande
        /// </summary>
        private void VideCommandesLivresSaisie()
        {
            txbCommandesLivresNumCmd.Text = "";
            txbCommandesLivresMontant.Text = "";
            txbCommandesLivresNbExemplaires.Text = "";
            dtpCommandesLivresDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations du livre
        /// </summary>
        private void AfficheCommandesLivresInfos(Livre livre)
        {
            txbCommandesLivresISBN.Text = livre.Isbn;
            txbCommandesLivresTitre.Text = livre.Titre;
            txbCommandesLivresAuteur.Text = livre.Auteur;
            txbCommandesLivresCollection.Text = livre.Collection;
            txbCommandesLivresGenre.Text = livre.Genre;
            txbCommandesLivresPublic.Text = livre.Public;
            txbCommandesLivresRayon.Text = livre.Rayon;
            txbCommandesLivresImage.Text = livre.Image;
            try
            {
                pcbCommandesLivresImage.Image = Image.FromFile(livre.Image);
            }
            catch
            {
                pcbCommandesLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste des commandes
        /// </summary>
        private void RemplirCommandesLivresListe(List<CommandeDocument> commandes)
        {
            bdgCommandesLivres.DataSource = commandes;
            dgvCommandesLivres.DataSource = bdgCommandesLivres;
            dgvCommandesLivres.Columns["IdLivreDvd"].Visible = false;
            dgvCommandesLivres.Columns["IdSuivi"].Visible = false;
            dgvCommandesLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Recherche un livre par son numéro et affiche ses informations et commandes
        /// </summary>
        private void btnCommandesLivresRechercher_Click(object sender, EventArgs e)
        {
            if (txbCommandesLivresNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de livre.", "Information");
                return;
            }
            Livre livre = lesLivres.Find(x => x.Id.Equals(txbCommandesLivresNumero.Text));
            if (livre != null)
            {
                AfficheCommandesLivresInfos(livre);
                lesCommandesLivres = controller.GetCommandesDocument(livre.Id);
                RemplirCommandesLivresListe(lesCommandesLivres);
            }
            else
            {
                MessageBox.Show("Numéro de livre introuvable.", "Information");
                VideCommandesLivresInfos();
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid
        /// </summary>
        private void dgvCommandesLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesLivres.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesLivres.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesLivres.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesLivres.OrderBy(o => o.Suivi).ToList();
                    break;
            }
            RemplirCommandesLivresListe(sortedList);
        }

        /// <summary>
        /// Ajoute une nouvelle commande de livre
        /// </summary>
        private void btnCommandesLivresAjouter_Click(object sender, EventArgs e)
        {
            if (txbCommandesLivresNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord rechercher un livre.", "Information");
                return;
            }
            if (txbCommandesLivresNumCmd.Text.Equals("") || txbCommandesLivresMontant.Text.Equals("") ||
                txbCommandesLivresNbExemplaires.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }

            string id = txbCommandesLivresNumCmd.Text;
            DateTime dateCommande = dtpCommandesLivresDate.Value;
            double montant = 0;
            if (!double.TryParse(txbCommandesLivresMontant.Text, out montant))
            {
                MessageBox.Show("Le montant doit être un nombre.", "Information");
                return;
            }
            int nbExemplaire = 0;
            if (!int.TryParse(txbCommandesLivresNbExemplaires.Text, out nbExemplaire))
            {
                MessageBox.Show("Le nombre d'exemplaires doit être un entier.", "Information");
                return;
            }

            string idLivreDvd = txbCommandesLivresNumero.Text;
            // l'étape de suivi est "en cours" par défaut
            string idSuivi = "00001";
            string suivi = "en cours";

            CommandeDocument commande = new CommandeDocument(id, dateCommande, montant,
                nbExemplaire, idLivreDvd, idSuivi, suivi);

            if (controller.CreerCommandeDocument(commande))
            {
                lesCommandesLivres = controller.GetCommandesDocument(idLivreDvd);
                RemplirCommandesLivresListe(lesCommandesLivres);
                VideCommandesLivresSaisie();
                MessageBox.Show("Commande ajoutée avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout de la commande.", "Erreur");
            }
        }

        /// <summary>
        /// Modifie le suivi d'une commande sélectionnée
        /// </summary>
        private void btnCommandesLivresSuivi_Click(object sender, EventArgs e)
        {
            if (dgvCommandesLivres.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }
            if (cbxCommandesLivresSuivi.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un suivi.", "Information");
                return;
            }

            CommandeDocument commande = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];
            Categorie nouveauSuivi = (Categorie)cbxCommandesLivresSuivi.SelectedItem;

            // vérification des règles de suivi
            // une commande livrée ou réglée ne peut pas revenir en arrière
            if ((commande.IdSuivi == "00003" || commande.IdSuivi == "00004") &&
                (nouveauSuivi.Id == "00001" || nouveauSuivi.Id == "00002"))
            {
                MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente.", "Erreur");
                return;
            }
            // une commande ne peut pas être réglée si elle n'est pas livrée
            if (nouveauSuivi.Id == "00004" && commande.IdSuivi != "00003")
            {
                MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée.", "Erreur");
                return;
            }

            CommandeDocument commandeModifiee = new CommandeDocument(commande.Id, commande.DateCommande,
                commande.Montant, commande.NbExemplaire, commande.IdLivreDvd, nouveauSuivi.Id, nouveauSuivi.Libelle);

            if (controller.ModifierSuiviCommande(commandeModifiee))
            {
                lesCommandesLivres = controller.GetCommandesDocument(commande.IdLivreDvd);
                RemplirCommandesLivresListe(lesCommandesLivres);
                MessageBox.Show("Suivi modifié avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification du suivi.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime une commande sélectionnée (uniquement si pas encore livrée)
        /// </summary>
        private void btnCommandesLivresSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesLivres.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            CommandeDocument commande = (CommandeDocument)bdgCommandesLivres.List[bdgCommandesLivres.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette commande ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerCommandeDocument(commande))
                {
                    lesCommandesLivres = controller.GetCommandesDocument(commande.IdLivreDvd);
                    RemplirCommandesLivresListe(lesCommandesLivres);
                    MessageBox.Show("Commande supprimée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer cette commande.\nElle est peut-être déjà livrée.", "Erreur");
                }
            }
        }

        #endregion

        #region Onglet Commandes DVD

        private readonly BindingSource bdgCommandesDvd = new BindingSource();
        private readonly BindingSource bdgSuivisDvd = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();

        /// <summary>
        /// Ouverture de l'onglet commandes DVD : vide les champs
        /// </summary>
        private void tabCommandesDvd_Enter(object sender, EventArgs e)
        {
            RemplirComboCategorie(controller.GetAllSuivis(), bdgSuivisDvd, cbxCommandesDvdSuivi);
            VideCommandesDvdInfos();
        }

        /// <summary>
        /// Vide les informations du DVD
        /// </summary>
        private void VideCommandesDvdInfos()
        {
            txbCommandesDvdDuree.Text = "";
            txbCommandesDvdTitre.Text = "";
            txbCommandesDvdReal.Text = "";
            txbCommandesDvdSynopsis.Text = "";
            txbCommandesDvdGenre.Text = "";
            txbCommandesDvdPublic.Text = "";
            txbCommandesDvdRayon.Text = "";
            txbCommandesDvdImage.Text = "";
            pcbCommandesDvdImage.Image = null;
            bdgCommandesDvd.DataSource = null;
            dgvCommandesDvd.DataSource = null;
            VideCommandesDvdSaisie();
        }

        /// <summary>
        /// Vide les champs de saisie d'une nouvelle commande
        /// </summary>
        private void VideCommandesDvdSaisie()
        {
            txbCommandesDvdNumCmd.Text = "";
            txbCommandesDvdMontant.Text = "";
            txbCommandesDvdNbExemplaires.Text = "";
            dtpCommandesDvdDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations du DVD
        /// </summary>
        private void AfficheCommandesDvdInfos(Dvd dvd)
        {
            txbCommandesDvdDuree.Text = dvd.Duree.ToString();
            txbCommandesDvdTitre.Text = dvd.Titre;
            txbCommandesDvdReal.Text = dvd.Realisateur;
            txbCommandesDvdSynopsis.Text = dvd.Synopsis;
            txbCommandesDvdGenre.Text = dvd.Genre;
            txbCommandesDvdPublic.Text = dvd.Public;
            txbCommandesDvdRayon.Text = dvd.Rayon;
            txbCommandesDvdImage.Text = dvd.Image;
            try
            {
                pcbCommandesDvdImage.Image = Image.FromFile(dvd.Image);
            }
            catch
            {
                pcbCommandesDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste des commandes
        /// </summary>
        private void RemplirCommandesDvdListe(List<CommandeDocument> commandes)
        {
            bdgCommandesDvd.DataSource = commandes;
            dgvCommandesDvd.DataSource = bdgCommandesDvd;
            dgvCommandesDvd.Columns["IdLivreDvd"].Visible = false;
            dgvCommandesDvd.Columns["IdSuivi"].Visible = false;
            dgvCommandesDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Recherche un DVD par son numéro et affiche ses informations et commandes
        /// </summary>
        private void btnCommandesDvdRechercher_Click(object sender, EventArgs e)
        {
            if (txbCommandesDvdNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de DVD.", "Information");
                return;
            }
            Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbCommandesDvdNumero.Text));
            if (dvd != null)
            {
                AfficheCommandesDvdInfos(dvd);
                lesCommandesDvd = controller.GetCommandesDocument(dvd.Id);
                RemplirCommandesDvdListe(lesCommandesDvd);
            }
            else
            {
                MessageBox.Show("Numéro de DVD introuvable.", "Information");
                VideCommandesDvdInfos();
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid
        /// </summary>
        private void dgvCommandesDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandesDvd.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandesDvd.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDvd.OrderBy(o => o.Suivi).ToList();
                    break;
            }
            RemplirCommandesDvdListe(sortedList);
        }

        /// <summary>
        /// Ajoute une nouvelle commande de DVD
        /// </summary>
        private void btnCommandesDvdAjouter_Click(object sender, EventArgs e)
        {
            if (txbCommandesDvdNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord rechercher un DVD.", "Information");
                return;
            }
            if (txbCommandesDvdNumCmd.Text.Equals("") || txbCommandesDvdMontant.Text.Equals("") ||
                txbCommandesDvdNbExemplaires.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }

            string id = txbCommandesDvdNumCmd.Text;
            DateTime dateCommande = dtpCommandesDvdDate.Value;
            double montant = 0;
            if (!double.TryParse(txbCommandesDvdMontant.Text, out montant))
            {
                MessageBox.Show("Le montant doit être un nombre.", "Information");
                return;
            }
            int nbExemplaire = 0;
            if (!int.TryParse(txbCommandesDvdNbExemplaires.Text, out nbExemplaire))
            {
                MessageBox.Show("Le nombre d'exemplaires doit être un entier.", "Information");
                return;
            }

            string idLivreDvd = txbCommandesDvdNumero.Text;
            string idSuivi = "00001";
            string suivi = "en cours";

            CommandeDocument commande = new CommandeDocument(id, dateCommande, montant,
                nbExemplaire, idLivreDvd, idSuivi, suivi);

            if (controller.CreerCommandeDocument(commande))
            {
                lesCommandesDvd = controller.GetCommandesDocument(idLivreDvd);
                RemplirCommandesDvdListe(lesCommandesDvd);
                VideCommandesDvdSaisie();
                MessageBox.Show("Commande ajoutée avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout de la commande.", "Erreur");
            }
        }

        /// <summary>
        /// Modifie le suivi d'une commande sélectionnée
        /// </summary>
        private void btnCommandesDvdSuivi_Click(object sender, EventArgs e)
        {
            if (dgvCommandesDvd.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }
            if (cbxCommandesDvdSuivi.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner un suivi.", "Information");
                return;
            }

            CommandeDocument commande = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];
            Categorie nouveauSuivi = (Categorie)cbxCommandesDvdSuivi.SelectedItem;

            if ((commande.IdSuivi == "00003" || commande.IdSuivi == "00004") &&
                (nouveauSuivi.Id == "00001" || nouveauSuivi.Id == "00002"))
            {
                MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente.", "Erreur");
                return;
            }
            if (nouveauSuivi.Id == "00004" && commande.IdSuivi != "00003")
            {
                MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée.", "Erreur");
                return;
            }

            CommandeDocument commandeModifiee = new CommandeDocument(commande.Id, commande.DateCommande,
                commande.Montant, commande.NbExemplaire, commande.IdLivreDvd, nouveauSuivi.Id, nouveauSuivi.Libelle);

            if (controller.ModifierSuiviCommande(commandeModifiee))
            {
                lesCommandesDvd = controller.GetCommandesDocument(commande.IdLivreDvd);
                RemplirCommandesDvdListe(lesCommandesDvd);
                MessageBox.Show("Suivi modifié avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de la modification du suivi.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime une commande sélectionnée (uniquement si pas encore livrée)
        /// </summary>
        private void btnCommandesDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesDvd.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }

            CommandeDocument commande = (CommandeDocument)bdgCommandesDvd.List[bdgCommandesDvd.Position];

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette commande ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerCommandeDocument(commande))
                {
                    lesCommandesDvd = controller.GetCommandesDocument(commande.IdLivreDvd);
                    RemplirCommandesDvdListe(lesCommandesDvd);
                    MessageBox.Show("Commande supprimée avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer cette commande.\nElle est peut-être déjà livrée.", "Erreur");
                }
            }
        }

        #endregion

        #region Onglet Commandes Revues

        private readonly BindingSource bdgCommandesRevues = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();

        /// <summary>
        /// Ouverture de l'onglet commandes revues : vide les champs
        /// </summary>
        private void tabCommandesRevues_Enter(object sender, EventArgs e)
        {
            VideCommandesRevuesInfos();
        }

        /// <summary>
        /// Vide les informations de la revue
        /// </summary>
        private void VideCommandesRevuesInfos()
        {
            txbCommandesRevuesTitre.Text = "";
            txbCommandesRevuesPeriodicite.Text = "";
            txbCommandesRevuesDelai.Text = "";
            txbCommandesRevuesGenre.Text = "";
            txbCommandesRevuesPublic.Text = "";
            txbCommandesRevuesRayon.Text = "";
            txbCommandesRevuesImage.Text = "";
            pcbCommandesRevuesImage.Image = null;
            bdgCommandesRevues.DataSource = null;
            dgvCommandesRevues.DataSource = null;
            VideCommandesRevuesSaisie();
        }

        /// <summary>
        /// Vide les champs de saisie d'un nouvel abonnement
        /// </summary>
        private void VideCommandesRevuesSaisie()
        {
            txbCommandesRevuesNumCmd.Text = "";
            txbCommandesRevuesMontant.Text = "";
            dtpCommandesRevuesDate.Value = DateTime.Now;
            dtpCommandesRevuesDateFinAbonnement.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations de la revue
        /// </summary>
        private void AfficheCommandesRevuesInfos(Revue revue)
        {
            txbCommandesRevuesTitre.Text = revue.Titre;
            txbCommandesRevuesPeriodicite.Text = revue.Periodicite;
            txbCommandesRevuesDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCommandesRevuesGenre.Text = revue.Genre;
            txbCommandesRevuesPublic.Text = revue.Public;
            txbCommandesRevuesRayon.Text = revue.Rayon;
            txbCommandesRevuesImage.Text = revue.Image;
            try
            {
                pcbCommandesRevuesImage.Image = Image.FromFile(revue.Image);
            }
            catch
            {
                pcbCommandesRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste des abonnements
        /// </summary>
        private void RemplirCommandesRevuesListe(List<Abonnement> abonnements)
        {
            bdgCommandesRevues.DataSource = abonnements;
            dgvCommandesRevues.DataSource = bdgCommandesRevues;
            dgvCommandesRevues.Columns["IdRevue"].Visible = false;
            dgvCommandesRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        /// <summary>
        /// Recherche une revue par son numéro et affiche ses informations et abonnements
        /// </summary>
        private void btnCommandesRevuesRechercher_Click(object sender, EventArgs e)
        {
            if (txbCommandesRevuesNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de revue.", "Information");
                return;
            }
            Revue revue = lesRevues.Find(x => x.Id.Equals(txbCommandesRevuesNumero.Text));
            if (revue != null)
            {
                AfficheCommandesRevuesInfos(revue);
                lesAbonnements = controller.GetAbonnementsRevue(revue.Id);
                RemplirCommandesRevuesListe(lesAbonnements);
            }
            else
            {
                MessageBox.Show("Numéro de revue introuvable.", "Information");
                VideCommandesRevuesInfos();
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid
        /// </summary>
        private void dgvCommandesRevues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesRevues.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesAbonnements.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesAbonnements.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnements.OrderBy(o => o.Montant).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = lesAbonnements.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
            }
            RemplirCommandesRevuesListe(sortedList);
        }

        /// <summary>
        /// Ajoute un nouvel abonnement
        /// </summary>
        private void btnCommandesRevuesAjouter_Click(object sender, EventArgs e)
        {
            if (txbCommandesRevuesNumero.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord rechercher une revue.", "Information");
                return;
            }
            if (txbCommandesRevuesNumCmd.Text.Equals("") || txbCommandesRevuesMontant.Text.Equals(""))
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires.", "Information");
                return;
            }

            string id = txbCommandesRevuesNumCmd.Text;
            DateTime dateCommande = dtpCommandesRevuesDate.Value;
            DateTime dateFinAbonnement = dtpCommandesRevuesDateFinAbonnement.Value;
            double montant = 0;
            if (!double.TryParse(txbCommandesRevuesMontant.Text, out montant))
            {
                MessageBox.Show("Le montant doit être un nombre.", "Information");
                return;
            }
            // vérification que la date de fin est après la date de commande
            if (dateFinAbonnement <= dateCommande)
            {
                MessageBox.Show("La date de fin d'abonnement doit être après la date de commande.", "Information");
                return;
            }

            string idRevue = txbCommandesRevuesNumero.Text;
            Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);

            if (controller.CreerAbonnement(abonnement))
            {
                lesAbonnements = controller.GetAbonnementsRevue(idRevue);
                RemplirCommandesRevuesListe(lesAbonnements);
                VideCommandesRevuesSaisie();
                MessageBox.Show("Abonnement ajouté avec succès.", "Information");
            }
            else
            {
                MessageBox.Show("Erreur lors de l'ajout de l'abonnement.", "Erreur");
            }
        }

        /// <summary>
        /// Supprime un abonnement sélectionné si aucun exemplaire n'est rattaché
        /// </summary>
        private void btnCommandesRevuesSupprimer_Click(object sender, EventArgs e)
        {
            if (dgvCommandesRevues.CurrentCell == null)
            {
                MessageBox.Show("Veuillez sélectionner un abonnement.", "Information");
                return;
            }

            Abonnement abonnement = (Abonnement)bdgCommandesRevues.List[bdgCommandesRevues.Position];

            // on récupère les exemplaires de la revue
            List<Exemplaire> lesExemplairesRevue = controller.GetExemplairesRevue(abonnement.IdRevue);

            // on vérifie si un exemplaire est dans la période de l'abonnement
            bool exemplaireTrouve = false;
            foreach (Exemplaire exemplaire in lesExemplairesRevue)
            {
                if (GestionAbonnement.ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, exemplaire.DateAchat))
                {
                    exemplaireTrouve = true;
                    break;
                }
            }

            if (exemplaireTrouve)
            {
                MessageBox.Show("Impossible de supprimer cet abonnement.\nDes exemplaires sont rattachés à cette période.", "Erreur");
                return;
            }

            DialogResult confirmation = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet abonnement ?",
                "Confirmation",
                MessageBoxButtons.YesNo
            );

            if (confirmation == DialogResult.Yes)
            {
                if (controller.SupprimerAbonnement(abonnement))
                {
                    lesAbonnements = controller.GetAbonnementsRevue(abonnement.IdRevue);
                    RemplirCommandesRevuesListe(lesAbonnements);
                    MessageBox.Show("Abonnement supprimé avec succès.", "Information");
                }
                else
                {
                    MessageBox.Show("Erreur lors de la suppression.", "Erreur");
                }
            }
        }

        #endregion
    }
}
