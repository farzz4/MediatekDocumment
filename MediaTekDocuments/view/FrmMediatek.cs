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

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        /// <summary>
        /// Afficher les abonnements arrivant à échéance à l'ouverture de l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            List<Abonnement> lesAbonnementsEcheance = controller.GetAbonnementsEcheance();
            if (lesAbonnementsEcheance.Count() != 0)
            {
                String liste = "Attention, les abonnements suivants se terminent dans moins de 30 jours ! \n \n ";
                foreach (Abonnement unAbonnement in lesAbonnementsEcheance)
                {
                    String infoAbonnement = "Revue \"" + unAbonnement.Titre + "\" : " +
                                            "expiration le " + unAbonnement.DateFinAbonnement.ToString("dd/MM/yyyy"); 
                    liste += "• " + infoAbonnement + "\n";
                }
                MessageBox.Show(liste, "Alerte de fin d'abonnement", MessageBoxButtons.OK);
            }
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
        /// Obtenir l'id d'un genre sélectionné
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        private string getIdGenreDoc(string genre)
        {
            List<Categorie> lesGenres = controller.GetAllGenres();
            foreach (Categorie unGenre in lesGenres)
            {
                if (unGenre.Libelle == genre)
                {
                    return unGenre.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtenir l'id d'un public sélectionné
        /// </summary>
        /// <param name="lepublic"></param>
        /// <returns></returns>
        private string getIdPublicDoc(string lepublic)
        {
            List<Categorie> lesPublics = controller.GetAllPublics();
            foreach (Categorie unPublic in lesPublics)
            {
                if (unPublic.Libelle == lepublic)
                {
                    return unPublic.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtenir l'id d'un rayon sélectionné 
        /// </summary>
        /// <param name="rayon"></param>
        /// <returns></returns>
        private string getIdRayonDoc(string rayon)
        {
            List<Categorie> lesRayons = controller.GetAllRayons();
            foreach (Categorie unRayon in lesRayons)
            {
                if (unRayon.Libelle == rayon)
                {
                    return unRayon.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Obtenir l'id d'un suivi 
        /// </summary>
        /// <param name="suivi">libellé du suivi</param>
        /// <returns>id du suivi</returns>
        private string getIdSuivi(string suivi)
        {
            List<Suivi> lesSuivis = controller.GetAllSuivis();
            foreach(Suivi unSuivi in lesSuivis)
            {
                if (unSuivi.Libelle == suivi)
                {
                    return unSuivi.Id;
                }
            }
            return null;
        }

        /// <summary>
        /// Vérifie si un document existe déjà avec cet id dans la bdd
        /// </summary>
        /// <param name="idLivre"></param>
        /// <returns></returns>
        private bool docExiste(string idDocument)
        {
            List<Document> lesDocs = controller.GetAllDocuments();
            foreach (Document unDoc in lesDocs)
            {
                if (unDoc.Id == idDocument)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test si une commande existe déjà avec le même num dans la bdd 
        /// </summary>
        /// <param name="num">numéro de la commande</param>
        /// <returns>true si la commande existe déjà</returns>
        private bool commandeExiste(string num)
        {
            List<Commande> lesCommandes = controller.GetAllCommandes();
            foreach (Commande uneCommande in lesCommandes)
            {
                if (uneCommande.Id == num)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test si le numéro de livre est de type Digit 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private bool isDigit(string numLivre)
        {
            bool isDigit = true;
            for (int i = 0; i < numLivre.Length; i++)
            {
                isDigit = Char.IsDigit(numLivre, i);
                if (!isDigit)
                {
                    break;
                }
            }
            return isDigit;
        }
        #endregion

        #region Onglet Livres
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
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
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
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
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
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
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
        /// affichage des informations du livre
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
        /// Préparation de l'interface pour l'ajout d'un livre
        /// </summary>
        private void encoursAjoutLivre(bool valeur)
        {
            VideLivresInfos();
            grpLivresRecherche.Enabled = !valeur;
            txbLivresNumero.ReadOnly = !valeur;
            txbLivresIsbn.ReadOnly = !valeur;
            txbLivresTitre.ReadOnly = !valeur;
            txbLivresAuteur.ReadOnly = !valeur;
            txbLivresCollection.ReadOnly = !valeur;
            txbLivresGenre.Visible = !valeur;
            cbxLivresGenre.Visible = valeur;
            cbxLivresGenre.Enabled = valeur;
            txbLivresPublic.Visible = !valeur;
            cbxLivresPublic.Visible = valeur;
            cbxLivresPublic.Enabled = valeur;
            txbLivresRayon.Visible = !valeur;
            cbxLivresRayon.Visible = valeur;
            cbxLivresRayon.Enabled = valeur;
            txbLivresImage.ReadOnly = !valeur;
            btnValModLivre.Visible = false;
            btnValAjLivre.Visible = valeur;
            btnAnnLivre.Visible = valeur;
            btnValAjLivre.Enabled = valeur;
            btnAnnLivre.Enabled = valeur;
            btnAjoutLivre.Enabled = !valeur;
            btnModifLivre.Enabled = !valeur;
            btnSupprLivre.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxLivresGenre();
                RemplirCbxLivresPublic();
                RemplirCbxLivresRayon();
            }
        }


        /// <summary>
        /// Remplir le combo avec les genres
        /// </summary>
        private void RemplirCbxLivresGenre()
        {
            if (cbxLivresGenre.Items.Count == 0)
            {
                List<Categorie> lesGenres = controller.GetAllGenres();
                foreach (Categorie unGenre in lesGenres)
                {
                    cbxLivresGenre.Items.Add(unGenre.Libelle);
                }
                if (cbxLivresGenre.Items.Count > 0)
                {
                    cbxLivresGenre.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo avec les publics 
        /// </summary>
        private void RemplirCbxLivresPublic()
        {
            if (cbxLivresPublic.Items.Count == 0)
            {
                List<Categorie> lesPublics = controller.GetAllPublics();
                foreach (Categorie unPublic in lesPublics)
                {
                    cbxLivresPublic.Items.Add(unPublic.Libelle);
                }
                if (cbxLivresPublic.Items.Count > 0)
                {
                    cbxLivresPublic.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo avec les rayons
        /// </summary>
        private void RemplirCbxLivresRayon()
        {
            if (cbxLivresRayon.Items.Count == 0)
            {
                List<Categorie> lesRayons = controller.GetAllRayons();
                foreach (Categorie unRayon in lesRayons)
                {
                    cbxLivresRayon.Items.Add(unRayon.Libelle);
                }
                if (cbxLivresRayon.Items.Count > 0)
                {
                    cbxLivresRayon.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Evenement au clic du bouton "ajouter"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutLivre_Click(object sender, EventArgs e)
        {
            encoursAjoutLivre(true);
        }

        /// <summary>
        /// Annuler l'insertion d'un nouveau livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'action ? ",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                encoursAjoutLivre(false);
                encoursModifLivre(false);
            }
        }

        /// <summary>
        /// Test si un livre existe déjà avec le même id dans la bdd 
        /// </summary>
        /// <param name="idLivre">id du livre</param>
        /// <returns>true si le livre existe déjà</returns>
        private bool livreExiste(string idLivre)
        {
            List<Livre> lesLivres = controller.GetAllLivres();
            foreach (Livre unLivre in lesLivres)
            {
                if (unLivre.Id == idLivre)
                {
                    return true;
                }
            }
            return false;
        }
        


        /// <summary>
        /// Ajouer un livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjLivre_Click(object sender, EventArgs e)
        {
            // Test si les champs nécessaires ont été renseignés 
            if (!txbLivresNumero.Text.Equals("")
                && !txbLivresTitre.Text.Equals("")
                && !cbxLivresGenre.Text.Equals("") && !cbxLivresPublic.Text.Equals("")
                && !cbxLivresRayon.Text.Equals(""))
            {
                if (isDigit(txbLivresNumero.Text))
                {
                    try
                    {
                        // Récupérer les informations saisies 
                        string id = txbLivresNumero.Text;
                        string isbn = txbLivresIsbn.Text;
                        string titre = txbLivresTitre.Text;
                        string auteur = txbLivresAuteur.Text;
                        string collection = txbLivresCollection.Text;
                        string genre = cbxLivresGenre.Text;
                        string idGenre = getIdGenreDoc(genre);
                        string Public = cbxLivresPublic.Text;
                        string idPublic = getIdPublicDoc(Public);
                        string rayon = cbxLivresRayon.Text;
                        string idRayon = getIdRayonDoc(rayon);
                        string image = txbLivresImage.Text;

                        // Créer le document et le livre 
                        Document document = new Document(id, titre, image, idGenre, genre, idPublic, Public, idRayon, rayon);
                        Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, Public, idRayon, rayon);

                        // si le livre n'existe pas déjà dans la base de données
                        if (!livreExiste(id) && !docExiste(id))
                        {
                            // si les créations de document + livres fonctionnent 
                            if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdRayon, document.IdPublic, document.IdGenre)
                                 && controller.CreerLivre(livre.Id, livre.Isbn, livre.Auteur, livre.Collection))
                            {
                                // recharger le datagridview avec les nouvelles informations
                                lesLivres = controller.GetAllLivres();
                                RemplirLivresListeComplete();
                                encoursAjoutLivre(false);
                                MessageBox.Show("Le livre nommé " + titre + " a bien été ajouté", "Infomation");
                            }
                        }
                        else { MessageBox.Show("Un document existe déjà avec ce numéro dans la base de données, veuillez en saisir un autre", "Information"); }
                    }
                    catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Information"); }
                }
                else { MessageBox.Show("Le numéro ne peut pas contenir de lettre ou caractères spéciaux", "Information"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'genre', 'public' et 'rayon' sont obligatoires", "Information"); }
        }

        /// <summary>
        /// Préparation de l'interface pour la modification d'un livre
        /// </summary>
        /// <param name="valeur">booléen indiquant si encoursModifLivre est vrai ou non</param>
        private void encoursModifLivre(bool valeur)
        {
            grpLivresRecherche.Enabled = !valeur;
            txbLivresIsbn.ReadOnly = !valeur;
            txbLivresTitre.ReadOnly = !valeur;
            txbLivresAuteur.ReadOnly = !valeur;
            txbLivresCollection.ReadOnly = !valeur;
            txbLivresGenre.Visible = !valeur;
            cbxLivresGenre.Visible = valeur;
            cbxLivresGenre.Enabled = valeur;
            txbLivresPublic.Visible = !valeur;
            cbxLivresPublic.Visible = valeur;
            cbxLivresPublic.Enabled = valeur;
            txbLivresRayon.Visible = !valeur;
            cbxLivresRayon.Visible = valeur;
            cbxLivresRayon.Enabled = valeur;
            txbLivresImage.ReadOnly = !valeur;
            btnValAjLivre.Visible = false;
            btnValModLivre.Visible = valeur;
            btnAnnLivre.Visible = valeur;
            btnValModLivre.Enabled = valeur;
            btnAnnLivre.Enabled = valeur;
            btnAjoutLivre.Enabled = !valeur;
            btnModifLivre.Enabled = !valeur;
            btnSupprLivre.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxLivresGenre();
                RemplirCbxLivresPublic();
                RemplirCbxLivresRayon();
                cbxLivresGenre.SelectedItem = txbLivresGenre.Text;
                cbxLivresPublic.SelectedItem = txbLivresPublic.Text;
                cbxLivresRayon.SelectedItem = txbLivresRayon.Text;
            }
        }

        /// <summary>
        /// Evenement au clic du bouton "modifier" un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifLivre_Click(object sender, EventArgs e)
        {
            if (txbLivresNumero.Text != "")
            {
                encoursModifLivre(true);
            }
            else
            {
                MessageBox.Show("Un livre doit être sélectionné pour être modifié", "Infomation");
            }
        }

        /// <summary>
        /// Modifier un livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValModLivre_Click(object sender, EventArgs e)
        {
            // Test si les champs nécessaires ont été renseignés 
            if (!txbLivresNumero.Text.Equals("") && !txbLivresTitre.Text.Equals("")
                && !cbxLivresGenre.Text.Equals("") && !cbxLivresPublic.Text.Equals("")
                && !cbxLivresRayon.Text.Equals(""))
            {
                try
                {
                    // Récupérer les informations saisies 
                    string id = txbLivresNumero.Text;
                    string isbn = txbLivresIsbn.Text;
                    string titre = txbLivresTitre.Text;
                    string auteur = txbLivresAuteur.Text;
                    string collection = txbLivresCollection.Text;
                    string genre = cbxLivresGenre.Text;
                    string idGenre = getIdGenreDoc(genre);
                    string Public = cbxLivresPublic.Text;
                    string idPublic = getIdPublicDoc(Public);
                    string rayon = cbxLivresRayon.Text;
                    string idRayon = getIdRayonDoc(rayon);
                    string image = txbLivresImage.Text;

                    // si les créations de document + livres fonctionnent 
                    if (controller.ModifierDocument(id, titre, image, idRayon, idPublic, idGenre)
                        && controller.ModifierLivre(id, isbn, auteur, collection))
                    {
                        // recharger le datagridview avec les nouvelles informations
                        lesLivres = controller.GetAllLivres();
                        RemplirLivresListeComplete();
                        encoursAjoutLivre(false);
                        MessageBox.Show("Le livre nommé " + titre + " a bien été modifié", "Infomation");
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la modification", "Erreur"); }
                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'genre', 'public' et 'rayon' sont obligatoires", "Information"); }
        }

        /// <summary>
        /// Supprimer un livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSupprLivre_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.Current;
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer le livre " + livre.Titre + "  ? ",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var exemplaires = controller.GetExemplairesDocument(livre.Id);
                var commandes = controller.GetCommandeDocuments(livre.Id);
                if (!exemplaires.Any() && !commandes.Any())
                {
                    if (controller.SupprimerLivre(livre.Id))
                    {
                        lesLivres = controller.GetAllLivres();
                        RemplirLivresListeComplete();
                        MessageBox.Show("Le livre nommé " + livre.Titre + " a bien été supprimé", "Information");
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la suppression", "Erreur"); }
                }
                else
                {
                    if (exemplaires.Any())
                    {
                        MessageBox.Show("Le livre est rattaché à un ou plusieurs exemplaire(s)");
                    }
                    else if (commandes.Any())
                    {
                        MessageBox.Show("Le livre est rattaché à une ou plusieurs commande(s)");
                    }
                }
            }
        }

        #endregion

        #region Onglet Dvd
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
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
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
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
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
        /// affichage des informations du dvd
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
        /// Remplir le combo box avec les genres
        /// </summary>
        private void RemplirCbxDvdGenre()
        {
            if (cbxDvdGenre.Items.Count == 0)
            {
                List<Categorie> lesGenres = controller.GetAllGenres();
                foreach (Categorie unGenre in lesGenres)
                {
                    cbxDvdGenre.Items.Add(unGenre.Libelle);
                }
                if (cbxDvdGenre.Items.Count > 0)
                {
                    cbxDvdGenre.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo box avec les publics
        /// </summary>
        private void RemplirCbxDvdPublic()
        {
            if (cbxDvdPublic.Items.Count == 0)
            {
                List<Categorie> lesPublics = controller.GetAllPublics();
                foreach (Categorie unPublic in lesPublics)
                {
                    cbxDvdPublic.Items.Add(unPublic.Libelle);
                }
                if (cbxDvdPublic.Items.Count > 0)
                {
                    cbxDvdPublic.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo box avec les rayons
        /// </summary>
        private void RemplirCbxDvdRayon()
        {
            if (cbxDvdRayon.Items.Count == 0)
            {
                List<Categorie> lesRayons = controller.GetAllRayons();
                foreach (Categorie unRayon in lesRayons)
                {
                    cbxDvdRayon.Items.Add(unRayon.Libelle);
                }
                if (cbxDvdRayon.Items.Count > 0)
                {
                    cbxDvdRayon.SelectedIndex = 0;
                }
            }

        }

        /// <summary>
        /// Préparation de l'interface pour l'ajout d'un dvd
        /// </summary>
        /// <param name="valeur">booléen indiquant si enCoursAjoutDvd est vrai ou non</param>
        private void encoursAjoutDvd(bool valeur)
        {
            VideDvdInfos();
            grpDvdRecherche.Enabled = !valeur;
            txbDvdNumero.ReadOnly = !valeur;
            txbDvdDuree.ReadOnly = !valeur;
            txbDvdTitre.ReadOnly = !valeur;
            txbDvdRealisateur.ReadOnly = !valeur;
            txbDvdSynopsis.ReadOnly = !valeur;
            txbDvdGenre.Visible = !valeur;
            cbxDvdGenre.Visible = valeur;
            cbxDvdGenre.Enabled = valeur;
            txbDvdPublic.Visible = !valeur;
            cbxDvdPublic.Visible = valeur;
            cbxDvdPublic.Enabled = valeur;
            txbDvdRayon.Visible = !valeur;
            cbxDvdRayon.Visible = valeur;
            cbxDvdRayon.Enabled = valeur;
            txbDvdImage.ReadOnly = !valeur;
            btnValModDvd.Visible = false;
            btnValAjDvd.Visible = valeur;
            btnAnnDvd.Visible = valeur;
            btnValAjDvd.Enabled = valeur;
            btnAnnDvd.Enabled = valeur;
            btnAjoutDvd.Enabled = !valeur;
            btnModifDvd.Enabled = !valeur;
            btnSupprDvd.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxDvdGenre();
                RemplirCbxDvdPublic();
                RemplirCbxDvdRayon();
            }
        }

        /// <summary>
        /// Evenement au clic du bouton ajouter un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutDvd_Click(object sender, EventArgs e)
        {
            encoursAjoutDvd(true);
        }

        /// <summary>
        /// Evenement au clic du bouton annuler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'action ?",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                encoursAjoutDvd(false);
            }
        }

        /// <summary>
        /// Test si un dvd existe déjà avec cet id dans la bdd 
        /// </summary>
        /// <param name="idDvd">id du dvd</param>
        /// <returns>true si le dvd existe déjà</returns>
        private bool dvdExiste(string idDvd)
        {
            List<Dvd> lesDvd = controller.GetAllDvd();
            foreach (Dvd unDvd in lesDvd)
            {
                if (unDvd.Id == idDvd)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evenement au clic du bouton Valider l'ajout 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjDvd_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals("") && !txbDvdTitre.Text.Equals("") && !txbDvdDuree.Text.Equals("")
                && !cbxDvdGenre.Text.Equals("") && !cbxDvdPublic.Text.Equals("")
                && !cbxDvdRayon.Text.Equals(""))
            {
                if (isDigit(txbDvdNumero.Text))
                {
                    try
                    {
                        string id = txbDvdNumero.Text;
                        int duree = int.Parse(txbDvdDuree.Text);
                        string titre = txbDvdTitre.Text;
                        string realisateur = txbDvdRealisateur.Text;
                        string synopsis = txbDvdSynopsis.Text;
                        string genre = cbxDvdGenre.Text;
                        string idGenre = getIdGenreDoc(genre);
                        string lePublic = cbxDvdPublic.Text;
                        string idPublic = getIdPublicDoc(lePublic);
                        string rayon = cbxDvdRayon.Text;
                        string idRayon = getIdRayonDoc(rayon);
                        string image = txbDvdImage.Text;

                        Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                        Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lePublic, idRayon, rayon);

                        if (!dvdExiste(id) && !docExiste(id))
                        {
                            if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdRayon, document.IdPublic, document.IdGenre)
                                && controller.CreerDvd(dvd.Id, dvd.Synopsis, dvd.Realisateur, dvd.Duree))
                            {
                                lesDvd = controller.GetAllDvd();
                                RemplirDvdListeComplete();
                                encoursAjoutDvd(false);
                                MessageBox.Show("Le dvd nommé " + titre + " a bien été ajouté", "Information");
                            }
                            else { MessageBox.Show("Une erreur est survenue lors de la création des données dans la base"); }
                        }
                        else { MessageBox.Show("Un document existe déjà avec ce numéro dans la base de donées, veuillez en saisir un autre", "Information"); }

                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Information"); }
                }
                else { MessageBox.Show("Le numéro ne peut pas contenir de lettre ou caractères spéciaux", "Information"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'duree', 'genre', 'public' et 'rayon' sont obligatoires", "Information"); }
        }

        /// <summary>
        /// Préparation de l'interface pour la modification d'un dvd
        /// </summary>
        /// <param name="valeur">booléen indiquant si enCoursModifDvd est vrai ou non</param>
        private void enCoursModifDvd(bool valeur)
        {
            grpDvdRecherche.Enabled = !valeur;
            txbDvdDuree.ReadOnly = !valeur;
            txbDvdTitre.ReadOnly = !valeur;
            txbDvdRealisateur.ReadOnly = !valeur;
            txbDvdSynopsis.ReadOnly = !valeur;
            txbDvdGenre.Visible = !valeur;
            cbxDvdGenre.Visible = valeur;
            cbxDvdGenre.Enabled = valeur;
            txbDvdPublic.Visible = !valeur;
            cbxDvdPublic.Visible = valeur;
            cbxDvdPublic.Enabled = valeur;
            txbDvdRayon.Visible = !valeur;
            cbxDvdRayon.Visible = valeur;
            cbxDvdRayon.Enabled = valeur;
            txbDvdImage.ReadOnly = !valeur;
            btnValAjDvd.Visible = false;
            btnValModDvd.Visible = valeur;
            btnAnnDvd.Visible = valeur;
            btnValModDvd.Enabled = valeur;
            btnAnnDvd.Enabled = valeur;
            btnAjoutDvd.Enabled = !valeur;
            btnModifDvd.Enabled = !valeur;
            btnSupprDvd.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxDvdGenre();
                RemplirCbxDvdPublic();
                RemplirCbxDvdRayon();
                cbxDvdGenre.SelectedItem = txbDvdGenre.Text;
                cbxDvdPublic.SelectedItem = txbDvdPublic.Text;
                cbxDvdRayon.SelectedItem = txbDvdRayon.Text;
            }
        }

        /// <summary>
        /// Evenement au clic du bouton Modifier dans l'onglet dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifDvd_Click(object sender, EventArgs e)
        {
            if (txbDvdNumero.Text != "")
            {
                enCoursModifDvd(true);
            }
            else
            {
                MessageBox.Show("Un dvd doit être sélectionné pour être modifié", "Information");
            }
        }

        /// <summary>
        /// Evenement au clic du bouton Valider la modification d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValModDvd_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumero.Text.Equals("") && !txbDvdTitre.Text.Equals("") && !txbDvdDuree.Text.Equals("")
                && !cbxDvdGenre.Text.Equals("") && !cbxDvdPublic.Text.Equals("")
                && !cbxDvdRayon.Text.Equals(""))
            {
                try
                {
                    string id = txbDvdNumero.Text;
                    int duree = int.Parse(txbDvdDuree.Text);
                    string titre = txbDvdTitre.Text;
                    string realisateur = txbDvdRealisateur.Text;
                    string synopsis = txbDvdSynopsis.Text;
                    string genre = cbxDvdGenre.Text;
                    string idGenre = getIdGenreDoc(genre);
                    string lePublic = cbxDvdPublic.Text;
                    string idPublic = getIdPublicDoc(lePublic);
                    string rayon = cbxDvdRayon.Text;
                    string idRayon = getIdRayonDoc(rayon);
                    string image = txbDvdImage.Text;

                    if (controller.ModifierDocument(id, titre, image, idRayon, idPublic, idGenre)
                        && controller.ModifierDvd(id, synopsis, realisateur, duree))
                    {
                        lesDvd = controller.GetAllDvd();
                        RemplirDvdListeComplete();
                        enCoursModifDvd(false);
                        MessageBox.Show("Le dvd nommé " + titre + " a bien été modifié", "Information");
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la modification ", "Erreur"); }

                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'duree', 'genre', 'public' et 'rayon' sont obligatoires"); }
        }

        /// <summary>
        /// Evenement au clic du bouton supprimer dans l'onglet dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprDvd_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgDvdListe.Current;
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer le dvd " + dvd.Titre + " ?",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var exemplaires = controller.GetExemplairesDocument(dvd.Id);
                var commandes = controller.GetCommandeDocuments(dvd.Id);
                if (!exemplaires.Any() && !commandes.Any())
                {
                    if (controller.SupprimerDvd(dvd.Id))
                    {
                        lesDvd = controller.GetAllDvd();
                        RemplirDvdListeComplete();
                        MessageBox.Show("Le dvd nommé " + dvd.Titre + " a bien été supprimé", "Information");
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la suppression"); }
                }
                else
                {
                    if (exemplaires.Any())
                    {
                        MessageBox.Show("Le dvd est rattaché à un ou plusieurs exemplaire(s)");
                    }
                    else if (commandes.Any())
                    {
                        MessageBox.Show("Le dvd est rattaché à une ou plusieurs commande(s)");
                    }
                }
            }
        }

        #endregion

        #region Onglet Revues
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
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
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
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
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
        /// Remplir le combo box avec les genres
        /// </summary>
        private void RemplirCbxRevueGenre()
        {
            if (cbxRevuesGenre.Items.Count == 0)
            {
                List<Categorie> lesGenres = controller.GetAllGenres();
                foreach (Categorie unGenre in lesGenres)
                {
                    cbxRevuesGenre.Items.Add(unGenre.Libelle);
                }
                if (cbxRevuesGenre.Items.Count > 0)
                {
                    cbxRevuesGenre.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo box avec les publics
        /// </summary>
        private void RemplirCbxRevuePublic()
        {
            if (cbxRevuesPublic.Items.Count == 0)
            {
                List<Categorie> lesPublics = controller.GetAllPublics();
                foreach (Categorie unPublic in lesPublics)
                {
                    cbxRevuesPublic.Items.Add(unPublic.Libelle);
                }
                if (cbxRevuesPublic.Items.Count > 0)
                {
                    cbxRevuesPublic.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Remplir le combo box avec les rayons
        /// </summary>
        private void RemplirCbxRevueRayon()
        {
            if (cbxRevuesRayon.Items.Count == 0)
            {
                List<Categorie> lesRayons = controller.GetAllRayons();
                foreach (Categorie unRayon in lesRayons)
                {
                    cbxRevuesRayon.Items.Add(unRayon.Libelle);
                }
                if (cbxRevuesRayon.Items.Count > 0)
                {
                    cbxRevuesRayon.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Préparation de l'interface pour l'ajout d'une revue
        /// </summary>
        /// <param name="valeur">booléen indiquant si enCoursAjoutRevue est vrai ou non</param>
        private void enCoursAjoutRevue(bool valeur)
        {
            VideRevuesInfos();
            grpRevuesRecherche.Enabled = !valeur;
            txbRevuesNumero.ReadOnly = !valeur;
            txbRevuesTitre.ReadOnly = !valeur;
            txbRevuesPeriodicite.ReadOnly = !valeur;
            txbRevuesDateMiseADispo.ReadOnly = !valeur;
            txbRevuesGenre.Visible = !valeur;
            cbxRevuesGenre.Visible = valeur;
            cbxRevuesGenre.Enabled = valeur;
            txbRevuesPublic.Visible = !valeur;
            cbxRevuesPublic.Visible = valeur;
            cbxRevuesPublic.Enabled = valeur;
            txbRevuesRayon.Visible = !valeur;
            cbxRevuesRayon.Visible = valeur;
            cbxRevuesRayon.Enabled = valeur;
            txbDvdImage.ReadOnly = !valeur;
            btnValModRevue.Visible = false;
            btnValAjRevue.Visible = valeur;
            btnAnnRevue.Visible = valeur;
            btnValAjRevue.Enabled = valeur;
            btnAnnRevue.Enabled = valeur;
            btnAjoutRevue.Enabled = !valeur;
            btnModifRevue.Enabled = !valeur;
            btnSupprRevue.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxRevueGenre();
                RemplirCbxRevuePublic();
                RemplirCbxRevueRayon();
            }
        }

        /// <summary>
        /// Evenement au clic du bouton a jouter une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjoutRevue_Click(object sender, EventArgs e)
        {
            enCoursAjoutRevue(true);
        }

        /// <summary>
        /// Evenement au clic du bouton annuler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'action?",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursAjoutRevue(false);
            }
        }

        /// <summary>
        /// Test si une revue existe déjà avec cet id dans la bdd
        /// </summary>
        /// <param name="idRevue">id de la revue</param>
        /// <returns>true si la revue existe déjà</returns>
        private bool revueExiste(string idRevue)
        {
            List<Revue> lesRevues = controller.GetAllRevues();
            foreach (Revue uneRevue in lesRevues)
            {
                if (uneRevue.Id == idRevue)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evenement au clic du bouton Valider l'ajout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals("") && !txbRevuesTitre.Text.Equals("") && !txbRevuesDateMiseADispo.Text.Equals("")
                && !cbxRevuesGenre.Text.Equals("") && !cbxRevuesPublic.Text.Equals("")
                && !cbxRevuesRayon.Text.Equals(""))
            {
                if (isDigit(txbRevuesNumero.Text))
                {
                    try
                    {
                        string id = txbRevuesNumero.Text;
                        string titre = txbRevuesTitre.Text;
                        string periodicite = txbRevuesPeriodicite.Text;
                        int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                        string genre = cbxRevuesGenre.Text;
                        string idGenre = getIdGenreDoc(genre);
                        string lePublic = cbxRevuesPublic.Text;
                        string idPublic = getIdPublicDoc(lePublic);
                        string rayon = cbxRevuesRayon.Text;
                        string idRayon = getIdRayonDoc(rayon);
                        string image = txbRevuesImage.Text;

                        Document document = new Document(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon);
                        Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon, periodicite, delaiMiseADispo);

                        if (!revueExiste(id) && !docExiste(id))
                        {
                            if (controller.CreerDocument(document.Id, document.Titre, document.Image, document.IdRayon, document.IdPublic, document.IdGenre)
                                && controller.CreerRevue(revue.Id, revue.Periodicite, revue.DelaiMiseADispo))
                            {
                                lesRevues = controller.GetAllRevues();
                                RemplirRevuesListeComplete();
                                enCoursAjoutRevue(false);
                                MessageBox.Show("La revue nommée " + titre + " a bien été ajoutée", "Information");
                            }
                            else { MessageBox.Show("Une erreur est survenue lors de la création des données dans la base"); }
                        }
                        else { MessageBox.Show("Une revue existe déjà avec ce numéro dans la base de données, veuillez en saisir un autre", "Information"); }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Information"); }
                }
                else { MessageBox.Show("Le numéro ne peut pas contenir de lettre ou caractères spéciaux", "Information"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'delai' 'genre', 'public' et 'rayon' sont obligatoires", "Information"); }
        }

        /// <summary>
        /// Préparation de l'interface pour la modification d'une revue 
        /// </summary>
        /// <param name="valeur">booléen indiquant si enCoursModifRevue est vrai ou non</param>
        private void enCoursModifRevue(bool valeur)
        {
            grpRevuesRecherche.Enabled = !valeur;
            txbRevuesTitre.ReadOnly = !valeur;
            txbRevuesPeriodicite.ReadOnly = !valeur;
            txbRevuesDateMiseADispo.ReadOnly = !valeur;
            txbRevuesGenre.Visible = !valeur;
            cbxRevuesGenre.Visible = valeur;
            cbxRevuesGenre.Enabled = valeur;
            txbRevuesPublic.Visible = !valeur;
            cbxRevuesPublic.Visible = valeur;
            cbxRevuesPublic.Enabled = valeur;
            txbRevuesRayon.Visible = !valeur;
            cbxRevuesRayon.Visible = valeur;
            cbxRevuesRayon.Enabled = valeur;
            txbRevuesImage.ReadOnly = !valeur;
            btnValAjRevue.Visible = false;
            btnValModRevue.Visible = valeur;
            btnAnnRevue.Visible = valeur;
            btnValModRevue.Enabled = valeur;
            btnAnnRevue.Enabled = valeur;
            btnAjoutRevue.Enabled = !valeur;
            btnModifRevue.Enabled = !valeur;
            btnSupprRevue.Enabled = !valeur;
            if (valeur)
            {
                RemplirCbxRevueGenre();
                RemplirCbxRevuePublic();
                RemplirCbxRevueRayon();
                cbxRevuesGenre.SelectedItem = txbRevuesGenre.Text;
                cbxRevuesPublic.SelectedItem = txbRevuesPublic.Text;
                cbxRevuesRayon.SelectedItem = txbRevuesRayon.Text;
            }
        }

        /// <summary>
        /// Evenement au clic du bouton Modifier dans l'onglet Revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifRevue_Click(object sender, EventArgs e)
        {
            if (txbRevuesNumero.Text != "")
            {
                enCoursModifRevue(true);
            }
            else
            {
                MessageBox.Show("Une revue doit être sélectionnée pour être modifiée", "Information");
            }
        }

        /// <summary>
        /// Evenement au clic du bouton Valider la modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValModRevue_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumero.Text.Equals("") && !txbRevuesTitre.Text.Equals("") && !txbRevuesDateMiseADispo.Text.Equals("")
                && !cbxRevuesGenre.Text.Equals("") && !cbxRevuesPublic.Text.Equals("")
                && !cbxRevuesRayon.Text.Equals(""))
            {
                try
                {
                    string id = txbRevuesNumero.Text;
                    string titre = txbRevuesTitre.Text;
                    string periodicite = txbRevuesPeriodicite.Text;
                    int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
                    string genre = cbxRevuesGenre.Text;
                    string idGenre = getIdGenreDoc(genre);
                    string lePublic = cbxRevuesPublic.Text;
                    string idPublic = getIdPublicDoc(lePublic);
                    string rayon = cbxRevuesRayon.Text;
                    string idRayon = getIdRayonDoc(rayon);
                    string image = txbRevuesImage.Text;

                    if (controller.ModifierDocument(id, titre, image, idRayon, idPublic, idGenre)
                        && controller.ModifierRevue(id, periodicite, delaiMiseADispo))
                    {
                        lesRevues = controller.GetAllRevues();
                        RemplirRevuesListeComplete();
                        enCoursModifRevue(false);
                        MessageBox.Show("La revue nommée " + titre + " a bien été modifiée", "Information");
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la modification", "Erreur"); }
                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Les champs 'numero', 'titre', 'delai', 'genre', 'public' et 'rayon' sont obligatoires"); }
        }

        /// <summary>
        /// Evenement au clic du bouton supprimer dans l'onglet revue 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprRevue_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.Current;
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer la revue " + revue.Titre + " ?",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var exemplaires = controller.GetExemplairesDocument(revue.Id);
                var commandes = controller.GetCommandeDocuments(revue.Id);
                if (!exemplaires.Any() && !commandes.Any())
                {
                    if (controller.SupprimerRevue(revue.Id))
                    {
                        lesRevues = controller.GetAllRevues();
                        RemplirRevuesListeComplete();
                        MessageBox.Show("La revue nommée " + revue.Titre + " a bien été supprimée", "Information");
                    }
                    else { MessageBox.Show("Une erreur est survenur lors de la suppression"); }
                }
                else
                {
                    if (exemplaires.Any())
                    {
                        MessageBox.Show("La revue est rattachée à un ou plusieurs exemplaire(s)");
                    }
                    else if (commandes.Any())
                    {
                        MessageBox.Show("La revue est rattachée à une ou plusieurs commande(s)");
                    }
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
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
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


        #endregion

        #region Onglet CommandesLivres
        private readonly BindingSource bdgCommandesLivreListe = new BindingSource();
        private List<CommandeDocument> lesCommandesDocument = new List<CommandeDocument>();

        /// <summary>
        /// Ouverture de l'onglet Commande de livre : 
        /// Gestion de l'interface 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivre_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();
            grpInfosCommandeLivre.Enabled = false;
            grpSuiviCommandeLivre.Enabled = false;
            cbxRechercheLivreCommande.Items.Clear();
            remplirCbxRechercheLivre();
        }

        /// <summary>
        /// Remplir le combo box permettant la recherche d'un livre 
        /// </summary>
        private void remplirCbxRechercheLivre()
        {
            List<Livre> lesLivres = controller.GetAllLivres();
            foreach(Livre unLivre in lesLivres)
            {
                cbxRechercheLivreCommande.Items.Add(unLivre.Id + " : " + unLivre.Titre);
            }
            if (cbxRechercheLivreCommande.Items.Count > 0)
            {
                cbxRechercheLivreCommande.SelectedIndex = 0;
            }
           
        }


        /// <summary>
        /// Recherche et affiche le livre dont le numéro a été choisi 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheLivre_Click(object sender, EventArgs e)
        {
            if (!cbxRechercheLivreCommande.Text.Equals(""))
            {
                string livreCherche = cbxRechercheLivreCommande.Text;
                Livre livre = getLivreCommande(livreCherche);
                if (livre != null)
                {
                    lesCommandesDocument = controller.GetCommandeDocuments(livre.Id);
                    remplirGrpInfosLivre(livre);
                    RemplirCommandesLivreListe(lesCommandesDocument);

                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Chercher l'objet Livre depuis le livre sélectionné dans la liste 
        /// </summary>
        /// <param name="livreSelected">livre sélectionné</param>
        /// <returns>objet Livre</returns>
        private Livre getLivreCommande(string livreSelected)
        {
            string[] result = livreSelected.Split(':');
            string idLivre = result[0].TrimEnd(' ');
            Livre livre = lesLivres.Find(x => x.Id.Equals(idLivre));
            return livre;
        }

        /// <summary>
        /// Remplir les informations du livre recherché
        /// </summary>
        /// <param name="livre"></param>
        private void remplirGrpInfosLivre(Livre livre)
        {
            txbComLivresTitre.Text = livre.Titre;
            txbComLivresIsbn.Text = livre.Isbn;
            txbComLivresAuteur.Text = livre.Auteur;
            txbComLivresCollection.Text = livre.Collection;
            txbComLivresGenre.Text = livre.Genre;
            txbComLivresPublic.Text = livre.Public;
            txbComLivresRayon.Text = livre.Rayon;
            txbComLivresCheminImage.Text = livre.Image;
            string image = livre.Image;
            try
            {
                pbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pbLivresImage.Image = null;
            }

        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètres
        /// </summary>
        /// <param name="lesCommandesDocument"></param>
        private void RemplirCommandesLivreListe(List<CommandeDocument> lesCommandesDocument)
        {
            bdgCommandesLivreListe.DataSource = lesCommandesDocument;
            dgvCommandesLivre.DataSource = bdgCommandesLivreListe;
            dgvCommandesLivre.Columns["idLivreDvd"].Visible = false;
            dgvCommandesLivre.Columns["idSuivi"].Visible = false;
            dgvCommandesLivre.Columns["id"].Visible = false;
            dgvCommandesLivre.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCommandesLivre.Columns["dateCommande"].DisplayIndex = 0;
            dgvCommandesLivre.Columns["montant"].DisplayIndex = 2;
            dgvCommandesLivre.Columns["dateCommande"].HeaderCell.Value = "Date de la commande";
            dgvCommandesLivre.Columns["nbExemplaire"].HeaderCell.Value = "Nombre d'exemplaires";
            dgvCommandesLivre.Columns["libelle"].HeaderCell.Value = "Suivi";
        }

        /// <summary>
        /// Actualise toutes les informations 
        /// </summary>
        private void RemplirCommandesLivreListeComplete()
        {
            RemplirCommandesLivreListe(lesCommandesDocument);
            VideCommandeLivreZone();
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesLivre_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesLivre.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date de la commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            RemplirCommandesLivreListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid 
        /// Affichage des informations de la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesLivre_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandesLivre.CurrentCell != null)
            {
                try
                {
                    CommandeDocument commandeDocument = (CommandeDocument)bdgCommandesLivreListe.List[bdgCommandesLivreListe.Position];
                    AffichageCommandeLivreInfos(commandeDocument);
                }
                catch
                {
                    VideCommandeLivreZone();
                }
            }
            else
            {
                VideCommandeLivreZone();
            }
        }

        /// <summary>
        /// Vide la zone avec les informations d'une commande d'un livre
        /// </summary>
        private void VideCommandeLivreZone()
        {
            txbNumCommandeLivre.Text = "";
            txbMontantCommandeLivre.Text = "";
            txbNbExemplairesCommandeLivre.Text = "";
            dtpDateCommandeLivre.Value = DateTime.Now;

        }
        
        /// <summary>
        /// Affiche les informations sur la commande d'un livre
        /// </summary>
        /// <param name="commandeDocument"></param>
        private void AffichageCommandeLivreInfos(CommandeDocument commandeDocument)
        {
            grpInfosCommandeLivre.Enabled = true;
            txbNumCommandeLivre.Text = commandeDocument.Id;
            txbMontantCommandeLivre.Text = commandeDocument.Montant.ToString();
            txbNbExemplairesCommandeLivre.Text = commandeDocument.NbExemplaire.ToString();
            dtpDateCommandeLivre.Value = commandeDocument.DateCommande;
            txbEtapeActuelleCommandeLivre.Text = commandeDocument.Libelle;
            // Si commande livrée, ne doit pas pouvoir être supprimée
            if (commandeDocument.IdSuivi == "2")
            {
                btnSupprCommandeLivre.Enabled = false;
            }
            // Si commande réglée, ne doit pas pouvoir modifier l'étape ni supprimer 
            else if (commandeDocument.IdSuivi == "3")
            {
                btnSupprCommandeLivre.Enabled = false;
                btnGererSuiviCommandeLivre.Enabled = false;
            }
            else
            {
                btnSupprCommandeLivre.Enabled = true;
                btnGererSuiviCommandeLivre.Enabled = true;
            }
            remplirCbxModifSuiviCommandeLivre(commandeDocument.IdSuivi);
        }
        
        /// <summary>
        /// Remplir le comboBox selon les étapes de suivi 
        /// </summary>
        private void remplirCbxModifSuiviCommandeLivre(string suivi)
        {
            cbxNvEtapeCommandeLivre.Items.Clear();
            // Commande "en cours"
            if (suivi == "1")
            {
                cbxNvEtapeCommandeLivre.Items.Add("livrée");
                cbxNvEtapeCommandeLivre.Items.Add("relancée");
                cbxNvEtapeCommandeLivre.SelectedIndex = 0;
            }   
            // Commande "livrée"
            else if (suivi == "2")
            {
                cbxNvEtapeCommandeLivre.Items.Add("réglée");
                cbxNvEtapeCommandeLivre.SelectedIndex = 0;
            }
            // Commande "réglée" 
            else if (suivi == "3")
            {
                cbxNvEtapeCommandeLivre.Text = "";
            }
            // Commande "relancée" 
            else if (suivi == "4")
            {
                cbxNvEtapeCommandeLivre.Items.Add("en cours");
                cbxNvEtapeCommandeLivre.Items.Add("livrée");
                cbxNvEtapeCommandeLivre.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// méthode évènementielle au clic du bouton ajouter une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjCommandeLivre_Click(object sender, EventArgs e)
        {
            enCoursAjoutCommandeLivre(true);
        }

        /// <summary>
        /// Modification de l'affichage du formulaire pour l'ajout d'une commande de livre
        /// </summary>
        /// <param name="valeur"></param>
        private void enCoursAjoutCommandeLivre(bool valeur)
        {
            VideCommandeLivreZone();
            lblSelectionLivre.Enabled = !valeur;
            cbxRechercheLivreCommande.Enabled = !valeur;
            btnRechercheLivre.Enabled = !valeur;
            grpInfosLivre.Enabled = !valeur;
            dgvCommandesLivre.Enabled = !valeur;
            txbNumCommandeLivre.ReadOnly = !valeur;
            txbMontantCommandeLivre.ReadOnly = !valeur;
            txbNbExemplairesCommandeLivre.ReadOnly = !valeur;
            dtpDateCommandeLivre.Enabled = valeur;
            btnAjCommandeLivre.Visible = !valeur;
            btnSupprCommandeLivre.Visible = !valeur;
            btnGererSuiviCommandeLivre.Visible = !valeur;
            btnValAjCommandeLivre.Visible = valeur;
            btnAnnAjCommandeLivre.Visible = valeur;
        }

        /// <summary>
        /// Annule l'ajout d'une commande pour un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnAjCommandeLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'ajout ? ", 
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursAjoutCommandeLivre(false);
            }
        }

        /// <summary>
        /// Ajouter une commande de livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjCommandeLivre_Click(object sender, EventArgs e)
        {
            // Test si les champs ont été renseignés 
            if (!txbNumCommandeLivre.Text.Equals("") && !txbMontantCommandeLivre.Text.Equals("") 
                && !txbNbExemplairesCommandeLivre.Text.Equals("") && !dtpDateCommandeLivre.Value.Equals(""))
            {
                try
                {
                    // Récupérer les informations
                    string num = txbNumCommandeLivre.Text;
                    double montant = double.Parse(txbMontantCommandeLivre.Text);
                    int nbExemplaires = int.Parse(txbNbExemplairesCommandeLivre.Text);
                    DateTime dateCommande = dtpDateCommandeLivre.Value;

                    string livreCherche = cbxRechercheLivreCommande.Text;
                    Livre livre = getLivreCommande(livreCherche);

                    // Créer les objets 
                    Commande commande = new Commande(num, dateCommande, montant);
                    CommandeDocument commandeDocument = new CommandeDocument(num, dateCommande, montant, nbExemplaires, livre.Id, "1", "en cours");

                    if (!commandeExiste(num))
                    {
                        if (controller.CreerCommande(commande)
                            && controller.CreerCommandeDocument(commandeDocument.Id, commandeDocument.NbExemplaire, commandeDocument.IdLivreDvd, commandeDocument.IdSuivi))
                        {
                            lesCommandesDocument = controller.GetCommandeDocuments(livre.Id);
                            RemplirCommandesLivreListeComplete();
                            enCoursAjoutCommandeLivre(false);
                            MessageBox.Show("La commande numéro " + num + " a bien été ajoutée", "Infomation");
                        }
                    }
                    else { MessageBox.Show("Ce numéro de commande existe déjà dans la base de données", "Erreur"); }
                    
                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Tous les champs du formulaire doivent être renseignés (numéro, montant, nombre d'exemplaire et date)", "Erreur"); }
        }

        /// <summary>
        /// Accède à la partie de modification du suivi d'une commande d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGererSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            enCoursModifSuiviLivre(true);
        }

        /// <summary>
        /// Adapte les zones pour la modification du suivi d'un livre
        /// </summary>
        /// <param name="valeur">true ou false si en cours de modif ou non</param>
        private void enCoursModifSuiviLivre(bool valeur)
        {
            cbxRechercheLivreCommande.Enabled = !valeur;
            grpInfosLivre.Enabled = !valeur;
            dgvCommandesLivre.Enabled = !valeur;
            grpInfosCommandeLivre.Enabled = !valeur;
            grpSuiviCommandeLivre.Enabled = valeur;
        }

        /// <summary>
        /// Annuler la modification du suivi d'une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnModifSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler la modification ? ",
                    "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursModifSuiviLivre(false);
            }
        }

        /// <summary>
        /// Modifier le suivi d'une commande de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txbNumCommandeLivre.Text;
                string suiviActuel = txbEtapeActuelleCommandeLivre.Text;
                string nvSuivi = cbxNvEtapeCommandeLivre.Text;
                string idSuivi = getIdSuivi(nvSuivi);

                if (MessageBox.Show("Êtes-vous sûr(e) de vouloir modifier l'étape " + suiviActuel + " pour l'étape " + nvSuivi + " ?",
                                    "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.ModifierSuiviCommandeDocument(id, idSuivi))
                    {
                        MessageBox.Show("L'étape de suivi de la commande a bien été modifiée", "Information");
                        enCoursAjoutCommandeLivre(false);
                        enCoursModifSuiviLivre(false);
                        string livreCherche = cbxRechercheLivreCommande.Text;
                        Livre livre = getLivreCommande(livreCherche);
                        lesCommandesDocument = controller.GetCommandeDocuments(livre.Id);
                        RemplirCommandesLivreListe(lesCommandesDocument);
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la modification du suivi", "Erreur");  }
                }
            }
            catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur");  }
        }

        /// <summary>
        /// Supprimer une commande d'un livre dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCommandeLivre_Click(object sender, EventArgs e)
        {
            string idCommande = txbNumCommandeLivre.Text;
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer la commande n°" + idCommande + "  ? ",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
               
                if (controller.SupprimerCommandeDocument(idCommande))
                {
                    MessageBox.Show("La commande a bien été supprimée", "Information");
                    enCoursAjoutCommandeLivre(false);
                    enCoursModifSuiviLivre(false);
                    string livreCherche = cbxRechercheLivreCommande.Text;
                    Livre livre = getLivreCommande(livreCherche);
                    lesCommandesDocument = controller.GetCommandeDocuments(livre.Id);
                    RemplirCommandesLivreListe(lesCommandesDocument);
                }
            }
        }
        #endregion


        #region Onglet CommandesDvd
        private readonly BindingSource bdgCommandesDvdListe = new BindingSource();

        /// <summary>
        /// Remplir le combo box permettant la recherche d'un dvd
        /// </summary>
        private void remplirCbxRechercheDvd()
        {
            List<Dvd> lesDvd = controller.GetAllDvd();
            foreach (Dvd unDvd in lesDvd)
            {
                cbxRechercheDvdCommande.Items.Add(unDvd.Id + " : " + unDvd.Titre);
            }
            if (cbxRechercheDvdCommande.Items.Count > 0)
            {
                cbxRechercheDvdCommande.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Ouverture de l'onglet Commande de Dvd :
        /// Gestion de l'interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            grpInfosCommandeDvd.Enabled = false;
            grpSuiviCommandeDvd.Enabled = false;
            cbxRechercheDvdCommande.Items.Clear();
            remplirCbxRechercheDvd();
        }

        /// <summary>
        /// Chercher l'objet Dvd depuis le dvd sélectionné dans la liste
        /// </summary>
        /// <param name="dvdSelected"></param>
        /// <returns></returns>
        private Dvd getDvdCommande(string dvdSelected)
        {
            string[] result = dvdSelected.Split(':');
            string idDvd = result[0].TrimEnd(' ');
            Dvd dvd = lesDvd.Find(x => x.Id.Equals(idDvd));
            return dvd;
        }

        /// <summary>
        /// Recherche et affiche le dvd dont le numéro a été choisi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheDvd_Click(object sender, EventArgs e)
        {
            if (!cbxRechercheDvdCommande.Text.Equals(""))
            {
                string dvdCherche = cbxRechercheDvdCommande.Text;
                Dvd dvd = getDvdCommande(dvdCherche);
                if (dvd != null)
                {
                    lesCommandesDocument = controller.GetCommandeDocuments(dvd.Id);
                    remplirGrpInfosDvd(dvd);
                    remplirCommandesDvdListe(lesCommandesDocument);
                }
            }
        }
        
        /// <summary>
        /// Remplir les informations du dvd recherché
        /// </summary>
        /// <param name="dvd"></param>
        private void remplirGrpInfosDvd(Dvd dvd)
        {
            txbComDvdTitre.Text = dvd.Titre;
            txbComDvdDuree.Text = dvd.Duree.ToString();
            txbComDvdRealisateur.Text = dvd.Realisateur;
            txbComDvdSynopsis.Text = dvd.Synopsis;
            txbComDvdGenre.Text = dvd.Genre;
            txbComDvdPublic.Text = dvd.Public;
            txbComDvdRayon.Text = dvd.Rayon;
            txbComDvdChemin.Text = dvd.Image;
            string image = dvd.Image;
            try
            {
                pbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste reçue en paramètres
        /// </summary>
        /// <param name="lesCommandesDocument"></param>
        private void remplirCommandesDvdListe(List<CommandeDocument> lesCommandesDocument)
        {
            bdgCommandesDvdListe.DataSource = lesCommandesDocument;
            dgvCommandesDvd.DataSource = bdgCommandesDvdListe;
            dgvCommandesDvd.Columns["idLivreDvd"].Visible = false;
            dgvCommandesDvd.Columns["idSuivi"].Visible = false;
            dgvCommandesDvd.Columns["id"].Visible = false;
            dgvCommandesDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvCommandesDvd.Columns["dateCommande"].DisplayIndex = 0;
            dgvCommandesDvd.Columns["montant"].DisplayIndex = 2;
            dgvCommandesDvd.Columns["dateCommande"].HeaderCell.Value = "Date de la commande";
            dgvCommandesDvd.Columns["nbExemplaire"].HeaderCell.Value = "Nombre d'exemplaires";
            dgvCommandesDvd.Columns["libelle"].HeaderCell.Value = "Suivi";
        }

        
        private void RemplirCommandesDvdListeComplete()
        {
            remplirCommandesDvdListe(lesCommandesDocument);
            VideCommandeDvdZone();
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandesDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Date de la commande":
                    sortedList = lesCommandesDocument.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Nombre d'exemplaires":
                    sortedList = lesCommandesDocument.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Montant).ToList();
                    break;
                case "Suivi":
                    sortedList = lesCommandesDocument.OrderBy(o => o.Libelle).ToList();
                    break;
            }
            remplirCommandesDvdListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid 
        /// Affichage des informations de la commande 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandesDvd_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandesDvd.CurrentCell != null)
            {
                try
                {
                    CommandeDocument CommandeDocument = (CommandeDocument)bdgCommandesDvdListe.List[bdgCommandesDvdListe.Position];
                    AffichageCommandeDvdInfos(CommandeDocument);
                }
                catch
                {
                    VideCommandeDvdZone();
                }
            }
            else
            {
                VideCommandeDvdZone();
            }
        }


        /// <summary>
        /// Vide la zone avec les informations d'une commande d'un dvd
        /// </summary>
        private void VideCommandeDvdZone()
        {
            txbNumCommandeDvd.Text = "";
            txbMontantCommandeDvd.Text = "";
            txbNbExemplairesCommandeDvd.Text = "";
            dtpDateCommandeDvd.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations sur la commande d'un dvd
        /// </summary>
        /// <param name="commandeDocument"></param>
        private void AffichageCommandeDvdInfos(CommandeDocument commandeDocument)
        {
            grpInfosCommandeDvd.Enabled = true;
            txbNumCommandeDvd.Text = commandeDocument.Id;
            txbMontantCommandeDvd.Text = commandeDocument.Montant.ToString();
            txbNbExemplairesCommandeDvd.Text = commandeDocument.NbExemplaire.ToString();
            dtpDateCommandeDvd.Value = commandeDocument.DateCommande;
            txbEtapeActuelleCommandeDvd.Text = commandeDocument.Libelle;
            if (commandeDocument.IdSuivi == "2")
            {
                btnSupprCommandeDvd.Enabled = false;
            }
            else if (commandeDocument.IdSuivi == "3")
            {
                btnSupprCommandeDvd.Enabled = false;
                btnGererSuiviCommandeDvd.Enabled = false;
            }
            else
            {
                btnSupprCommandeDvd.Enabled = true;
                btnGererSuiviCommandeDvd.Enabled = true;
            }
            remplirCbxModifSuiviCommandeDvd(commandeDocument.IdSuivi);
        }

        /// <summary>
        /// Remplit le combobox avec les différentes étapes de suivi d'une commande de dvd
        /// </summary>
        /// <param name="idSuivi"></param>
        private void remplirCbxModifSuiviCommandeDvd(string idSuivi)
        {
            cbxNvEtapeCommandeDvd.Items.Clear();
            if (idSuivi == "1")
            {
                cbxNvEtapeCommandeDvd.Items.Add("livrée");
                cbxNvEtapeCommandeDvd.Items.Add("relancée");
                cbxNvEtapeCommandeDvd.SelectedIndex = 0;
            }
            else if (idSuivi == "2")
            {
                cbxNvEtapeCommandeDvd.Items.Add("réglée");
                cbxNvEtapeCommandeDvd.SelectedIndex = 0;
            }
            else if (idSuivi == "3")
            {
                cbxNvEtapeCommandeDvd.Text = "";
            }
            else if (idSuivi == "4")
            {
                cbxNvEtapeCommandeDvd.Items.Add("en cours");
                cbxNvEtapeCommandeDvd.Items.Add("livrée");
                cbxNvEtapeCommandeDvd.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// méthode évènementielle au clic du bouton ajouter une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjCommandeDvd_Click(object sender, EventArgs e)
        {
            enCoursAjoutCommandeDvd(true);
        }

        /// <summary>
        /// Modification de l'affichage du formulaire pour l'ajout d'une commande de dvd
        /// </summary>
        /// <param name="valeur"></param>
        private void enCoursAjoutCommandeDvd(bool valeur)
        {
            VideCommandeDvdZone();
            lblSelectionDvd.Enabled = !valeur;
            cbxRechercheDvdCommande.Enabled = !valeur;
            btnRechercheDvd.Enabled = !valeur;
            grpInfosDvd.Enabled = !valeur;
            dgvCommandesDvd.Enabled = !valeur;
            txbNumCommandeDvd.ReadOnly = !valeur;
            txbMontantCommandeDvd.ReadOnly = !valeur;
            txbNbExemplairesCommandeDvd.ReadOnly = !valeur;
            dtpDateCommandeDvd.Enabled = valeur;
            btnAjCommandeDvd.Visible = !valeur;
            btnSupprCommandeDvd.Visible = !valeur;
            btnGererSuiviCommandeDvd.Visible = !valeur;
            btnValAjCommandeDvd.Visible = valeur;
            btnAnnAjCommandeDvd.Visible = valeur;
        }

        /// <summary>
        /// Annule l'ajout d'une commande pour un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnAjCommandeDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'ajout ? ",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursAjoutCommandeDvd(false);
            }
        }
        
        /// <summary>
        /// Ajouter une commande de dvd dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjCommandeDvd_Click(object sender, EventArgs e)
        {
            if (!txbNumCommandeDvd.Text.Equals("") && !txbMontantCommandeDvd.Text.Equals("")
                && !txbNbExemplairesCommandeDvd.Text.Equals("") && !dtpDateCommandeDvd.Value.Equals(""))
            {
                try
                {
                    string num = txbNumCommandeDvd.Text;
                    double montant = double.Parse(txbMontantCommandeDvd.Text);
                    int nbExemplaires = int.Parse(txbNbExemplairesCommandeDvd.Text);
                    DateTime dateCommande = dtpDateCommandeDvd.Value;

                    string dvdCherche = cbxRechercheDvdCommande.Text;
                    Dvd dvd = getDvdCommande(dvdCherche);

                    Commande commande = new Commande(num, dateCommande, montant);
                    CommandeDocument commandeDocument = new CommandeDocument(num, dateCommande, montant, nbExemplaires, dvd.Id, "1", "en cours");

                    if (!commandeExiste(num))
                    {
                        if (controller.CreerCommande(commande)
                            && controller.CreerCommandeDocument(commandeDocument.Id, commandeDocument.NbExemplaire, commandeDocument.IdLivreDvd, commandeDocument.IdSuivi)){
                            lesCommandesDocument = controller.GetCommandeDocuments(dvd.Id);
                            RemplirCommandesDvdListeComplete();
                            enCoursAjoutCommandeDvd(false);
                            MessageBox.Show("La commande numéro " + num + " a bien été ajoutée", "Information");
                        }
                    }
                    else { MessageBox.Show("Ce numéro de commande existe déjà dans la base de données", "Erreur"); }
                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Tous les champs du formulaire doivent être renseignés (numéro, montant, nombre d'exemplaire et date)", "Erreur"); }
        }

        /// <summary>
        /// Accède à la partie de modification du suivi d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGererSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            enCoursModifSuiviDvd(true);
        }

        /// <summary>
        /// Adapte les zones pour la modification du suivi d'un dvd
        /// </summary>
        /// <param name="valeur"></param>
        private void enCoursModifSuiviDvd(bool valeur)
        {
            cbxRechercheDvdCommande.Enabled = !valeur;
            grpInfosDvd.Enabled = !valeur;
            dgvCommandesDvd.Enabled = !valeur;
            grpInfosCommandeDvd.Enabled = !valeur;
            grpSuiviCommandeDvd.Enabled = valeur;
        }

        /// <summary>
        /// Annule la modification du suivi d'une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnModifSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler la modification ? ", 
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursModifSuiviDvd(false);
            }
        }

        /// <summary>
        /// Modifie le suivi d'une commande de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifSuiviCommandeDvd_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txbNumCommandeDvd.Text;
                string suiviActuel = txbEtapeActuelleCommandeDvd.Text;
                string nvSuivi = cbxNvEtapeCommandeDvd.Text;
                string idSuivi = getIdSuivi(nvSuivi);
                if (MessageBox.Show("Êtes-vous sûr(e) de vouloir modifier l'étape " + suiviActuel + " pour l'étape " + nvSuivi + " ?",
                                                    "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controller.ModifierSuiviCommandeDocument(id, idSuivi))
                    {
                        MessageBox.Show("L'étape de suivi de la commande a bien été modifiée", "Information");
                        enCoursAjoutCommandeDvd(false);
                        enCoursModifSuiviDvd(false);
                        string dvdCherche = cbxRechercheDvdCommande.Text;
                        Dvd dvd = getDvdCommande(dvdCherche);
                        lesCommandesDocument = controller.GetCommandeDocuments(dvd.Id);
                        remplirCommandesDvdListe(lesCommandesDocument);
                    }
                    else { MessageBox.Show("Une erreur est survenue lors de la modification du suivi", "Erreur"); }
                }
            }
            catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
        }


        /// <summary>
        /// Supprime une commande d'un dvd dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCommandeDvd_Click(object sender, EventArgs e)
        {
            string idCommande = txbNumCommandeDvd.Text;
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer la commande n°" + idCommande + " ? ", 
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controller.SupprimerCommandeDocument(idCommande))
                {
                    MessageBox.Show("La commande a bien été supprimée", "Information");
                    enCoursAjoutCommandeDvd(false);
                    enCoursModifSuiviDvd(false);
                    string dvdCherche = cbxRechercheDvdCommande.Text;
                    Dvd dvd = getDvdCommande(dvdCherche);
                    lesCommandesDocument = controller.GetCommandeDocuments(dvd.Id);
                    remplirCommandesDvdListe(lesCommandesDocument);
                }
            }
        }

        #endregion

        #region Onglet CommandesRevue
        private readonly BindingSource bdgAbonnementsRevueListe = new BindingSource();
        private List<Abonnement> lesAbonnementsRevue = new List<Abonnement>();

        /// <summary>
        /// Remplir le combo box permettant la recherche d'une revue
        /// </summary>
        private void remplirCbxRechercheRevue()
        {
            List<Revue> lesRevues = controller.GetAllRevues();
            foreach(Revue uneRevue in lesRevues)
            {
                cbxRechercheRevueCommande.Items.Add(uneRevue.Id + " : " + uneRevue.Titre);
            }
            if (cbxRechercheRevueCommande.Items.Count > 0)
            {
                cbxRechercheRevueCommande.SelectedIndex = 0;
            }
        }
        
        /// <summary>
        /// 0uverture de l'onglet Commande de revue : 
        /// Gestion de l'interface 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            cbxRechercheRevueCommande.Items.Clear();
            remplirCbxRechercheRevue();
        }

        /// <summary>
        /// Chercher l'objet Revue depuis la revue sélectionnée dans la liste
        /// </summary>
        /// <param name="revueSelected"></param>
        /// <returns></returns>
        private Revue getRevueCommande(string revueSelected)
        {
            string[] result = revueSelected.Split(':');
            string idRevue = result[0].TrimEnd(' ');
            Revue revue = lesRevues.Find(x => x.Id.Equals(idRevue));
            return revue;
        }

        /// <summary>
        /// Recherche et affiche la revue dont le numéro a été choisi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercheRevue_Click(object sender, EventArgs e)
        {
            if (!cbxRechercheRevueCommande.Text.Equals(""))
            {
                string revueCherche = cbxRechercheRevueCommande.Text;
                Revue revue = getRevueCommande(revueCherche);
                if (revue != null)
                {
                    lesAbonnementsRevue = controller.GetAbonnementsRevue(revue.Id);
                    remplirGrpInfosRevue(revue);
                    remplirAbonnementsRevueListe(lesAbonnementsRevue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Replir les informations de la revue recherchée
        /// </summary>
        /// <param name="revue"></param>
        private void remplirGrpInfosRevue(Revue revue)
        {
            txbComRevueTitre.Text = revue.Titre;
            txbComRevuePeriod.Text = revue.Periodicite;
            txbComRevueDelai.Text = revue.DelaiMiseADispo.ToString();
            txbComRevueGenre.Text = revue.Genre;
            txbComRevuePublic.Text = revue.Public;
            txbComRevueRayon.Text = revue.Rayon;
            txbComRevueChemin.Text = revue.Image;
            string image = revue.Image;
            try
            {
                pbRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pbRevueImage.Image = null;
            }
        }

        /// <summary>
        /// Remplit le datagrid avec la liste des commandes de revue en paramètres
        /// </summary>
        /// <param name="lesAbonnementsRevue"></param>
        private void remplirAbonnementsRevueListe(List<Abonnement> lesAbonnementsRevue)
        {
            bdgAbonnementsRevueListe.DataSource = lesAbonnementsRevue;
            dgvAbonnementsRevue.DataSource = bdgAbonnementsRevueListe;
            dgvAbonnementsRevue.Columns["id"].Visible = false;
            dgvAbonnementsRevue.Columns["idRevue"].Visible = false;
            dgvAbonnementsRevue.Columns["titre"].Visible = false;
            dgvAbonnementsRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvAbonnementsRevue.Columns["dateCommande"].DisplayIndex = 0;
            dgvAbonnementsRevue.Columns["montant"].DisplayIndex = 1;
            dgvAbonnementsRevue.Columns["dateFinAbonnement"].DisplayIndex = 2;
            dgvAbonnementsRevue.Columns["dateCommande"].HeaderCell.Value = "Date de la commande";
            dgvAbonnementsRevue.Columns["dateFinAbonnement"].HeaderCell.Value = "Date de fin d'abonnement";
        }

        /// <summary>
        /// Actualise toutes les informations
        /// </summary>
        private void remplirAbonnementsRevueListeComplete()
        {
            remplirAbonnementsRevueListe(lesAbonnementsRevue);
            VideCommandeRevueZone();
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsRevue_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvAbonnementsRevue.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Date de la commande":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.Montant).ToList();
                    break;
                case "Date de fin d'abonnement":
                    sortedList = lesAbonnementsRevue.OrderBy(o => o.Montant).Reverse().ToList();
                    break;
            }
            remplirAbonnementsRevueListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid 
        /// Affichage des informations de l'abonnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsRevue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAbonnementsRevue.CurrentCell != null)
            {
                try
                {
                    Abonnement abonnement = (Abonnement)bdgAbonnementsRevueListe.List[bdgAbonnementsRevueListe.Position];
                    AffichageCommandeRevueInfos(abonnement);
                }
                catch
                {
                    VideCommandeRevueZone();
                }
            }
            else
            {
                VideCommandeRevueZone();
            }
        }

        /// <summary>
        /// Vide la zone avec les informations d'une commande d'une revue
        /// </summary>
        private void VideCommandeRevueZone()
        {
            txbNumCommandeRevue.Text = "";
            txbMontantCommandeRevue.Text = "";
            dtpDateCommandeRevue.Value = DateTime.Now;
            dtpFinAbonnementRevue.Value = DateTime.Now;
        }

        /// <summary>
        /// Affiche les informations sur l'abonnement d'une revue
        /// </summary>
        /// <param name="abonnementRevue"></param>
        private void AffichageCommandeRevueInfos(Abonnement abonnementRevue)
        {
            grpInfosCommandeRevue.Enabled = true;
            txbNumCommandeRevue.Text = abonnementRevue.Id;
            txbMontantCommandeRevue.Text = abonnementRevue.Montant.ToString();
            dtpDateCommandeRevue.Value = abonnementRevue.DateCommande;
            dtpFinAbonnementRevue.Value = abonnementRevue.DateFinAbonnement;
        }

        /// <summary>
        /// méthode évènementielle au clic du bouton ajouter un abonnement d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjCommandeRevue_Click(object sender, EventArgs e)
        {
            enCoursAjoutAbonnementRevue(true);
        }


        /// <summary>
        /// Modification de l'affichage du formulaire pour l'ajout d'un abonnement de revue
        /// </summary>
        private void enCoursAjoutAbonnementRevue(bool valeur)
        {
            VideCommandeRevueZone();
            lblSelectionRevue.Enabled = !valeur;
            cbxRechercheRevueCommande.Enabled = !valeur;
            btnRechercheRevue.Enabled = !valeur;
            grpInfosRevue.Enabled = !valeur;
            dgvAbonnementsRevue.Enabled = !valeur;
            txbNumCommandeRevue.ReadOnly = !valeur;
            txbMontantCommandeRevue.ReadOnly = !valeur;
            dtpDateCommandeRevue.Enabled = valeur;
            dtpFinAbonnementRevue.Enabled = valeur;
            btnAjCommandeRevue.Visible = !valeur;
            btnSupprCommandeRevue.Visible = !valeur;
            btnValAjCommandeRevue.Visible = valeur;
            btnAnnAjCommandeRevue.Visible = valeur;
        }

        /// <summary>
        /// Annuler l'ajout d'un abonnement pour une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnAjCommandeRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr(e) de vouloir annuler l'ajout ? ",
                                "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                enCoursAjoutAbonnementRevue(false);
            }
        }


        /// <summary>
        /// Ajouter un abonnement de revue dans la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValAjCommandeRevue_Click(object sender, EventArgs e)
        {
            if (!txbNumCommandeRevue.Text.Equals("") && !txbMontantCommandeRevue.Text.Equals("")
                && !dtpDateCommandeRevue.Value.Equals("") && !dtpFinAbonnementRevue.Value.Equals(""))
            {
                try
                {
                    string num = txbNumCommandeRevue.Text;
                    double montant = double.Parse(txbMontantCommandeRevue.Text);
                    DateTime dateCommande = dtpDateCommandeRevue.Value;
                    DateTime dateFinAbonnement = dtpFinAbonnementRevue.Value;

                    string revueCherche = cbxRechercheRevueCommande.Text;
                    Revue revue = getRevueCommande(revueCherche);

                    Commande commande = new Commande(num, dateCommande, montant);
                    Abonnement abonnement = new Abonnement(num, dateCommande, montant, dateFinAbonnement, revue.Id, revue.Titre);

                    if (!commandeExiste(num))
                    {
                        if (controller.CreerCommande(commande)
                            && controller.CreerAbonnementRevue(abonnement.Id, abonnement.DateFinAbonnement, abonnement.IdRevue))
                        {
                            lesAbonnementsRevue = controller.GetAbonnementsRevue(revue.Id);
                            remplirAbonnementsRevueListeComplete();
                            enCoursAjoutAbonnementRevue(false);
                            MessageBox.Show("La commande numéro " + num + " a bien été ajoutée", "Infomation");
                        }
                    }
                    else { MessageBox.Show("Ce numéro de commande existe déjà dans la base de données", "Erreur"); }
                }
                catch { MessageBox.Show("Une erreur est survenue lors de la récupération des données", "Erreur"); }
            }
            else { MessageBox.Show("Tous les champs du formulaire doivent être renseignés (numéro, montant, nombre d'exemplaire et date)", "Erreur"); }
        }


        /// <summary>
        /// Vérifie si un ou plusieurs exemplaires n'est pas rattaché à un abonnement de revue
        /// </summary>
        /// <param name="abonnement">Objet abonnement</param>
        /// <returns>true si aucun exemplaire est rattaché à la commande de revue</returns>
        private bool verifExemplaireAbonnement (Abonnement abonnement)
        {
            List<Exemplaire> lesExemplaires = controller.GetExemplairesRevue(abonnement.IdRevue);
            bool verif = true;
            foreach (Exemplaire unExemplaire in lesExemplaires)
            {
                Console.WriteLine(abonnement.DateCommande + " " + abonnement.DateFinAbonnement + " " + unExemplaire.DateAchat);
                if (controller.ParutionDansAbonnement(abonnement.DateCommande, abonnement.DateFinAbonnement, unExemplaire.DateAchat))
                {
                    verif = false;
                }
            }
            return verif;
        }

        /// <summary>
        /// Methode évènementielle pour supprimer un abonnement d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCommandeRevue_Click(object sender, EventArgs e)
        {
            if (dgvAbonnementsRevue.SelectedRows.Count > 0)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnementsRevueListe.Current;
                if (MessageBox.Show("Êtes-vous sûr(e) de vouloir supprimer l'abonnement n°" + abonnement.Id + " ? ",
                                    "Demande de confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (verifExemplaireAbonnement(abonnement))
                    {
                        if (controller.SupprimerAbonnementRevue(abonnement.Id))
                        {
                            string revueCherche = cbxRechercheRevueCommande.Text;
                            Revue revue = getRevueCommande(revueCherche);
                            lesAbonnementsRevue = controller.GetAbonnementsRevue(revue.Id);
                            remplirAbonnementsRevueListe(lesAbonnementsRevue);
                            MessageBox.Show("L'abonnement n°" + abonnement.Id + " a bien été supprimé", "Information");
                        }
                    }
                    else { MessageBox.Show("Cet abonnement est rattaché à un ou plusieurs exemplaire(s)", "Erreur"); }
                }
            }
            else { MessageBox.Show("Une ligne doit être sélectionnée", "Erreur"); }
        }


        #endregion


    }
}
