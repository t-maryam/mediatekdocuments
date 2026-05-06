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
        private readonly Utilisateur utilisateurConnecte;

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.utilisateurConnecte = utilisateur;
            this.Load += FrmMediatek_Load;
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
        #endregion

        #region Onglet Paarutions
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

        #region Onglet Commandes livres
        private readonly BindingSource bdgCmdLivresSuivis = new BindingSource();
        private readonly BindingSource bdgCmdLivresListe = new BindingSource();
        private List<CommandeDocument> lesCommandesLivres = new List<CommandeDocument>();
        private Livre leLivreCourant = null;

        /// <summary>
        /// Ouverture de l'onglet : remplit le combo des suivis et la datagrid avec toutes les commandes
        /// </summary>
        private void TabCommandesLivres_Enter(object sender, EventArgs e)
        {
            RemplirComboCategorie(controller.GetAllSuivis(), bdgCmdLivresSuivis, cbxCmdLivresSuivi);
            if (cbxCmdLivresSuivi.Items.Count > 0)
                cbxCmdLivresSuivi.SelectedIndex = 0;
            RafraichirCmdLivresListe();
            grpCmdLivresCommande.Enabled = false;
        }

        /// <summary>
        /// Recharge toutes les commandes dans la datagridview
        /// </summary>
        private void RafraichirCmdLivresListe()
        {
            List<string> idsLivres = controller.GetAllLivres().Select(l => l.Id).ToList();
            lesCommandesLivres = controller.GetCommandesLivre("").Where(c => idsLivres.Contains(c.idLivreDvd)).ToList();
            RemplirCmdLivresListe(lesCommandesLivres);
        }

        /// <summary>
        /// Recherche du livre par numéro et affichage de ses infos
        /// </summary>
        private void BtnCmdLivresRecherche_Click(object sender, EventArgs e)
        {
            if (txbCmdLivresNumRecherche.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de document.", "Information");
                return;
            }
            List<Livre> livres = controller.GetAllLivres();
            leLivreCourant = livres.Find(x => x.Id.Equals(txbCmdLivresNumRecherche.Text));
            if (leLivreCourant != null)
            {
                AfficheCmdLivresInfos(leLivreCourant);
            }
            else
            {
                MessageBox.Show("Numéro introuvable.", "Information");
                VideCmdLivresInfos();
            }
        }

        /// <summary>
        /// Affiche les infos du livre dans le groupbox infos
        /// </summary>
        private void AfficheCmdLivresInfos(Livre livre)
        {
            txbCmdLivresNum.Text = livre.Id;
            txbCmdLivresTitre.Text = livre.Titre;
            txbCmdLivresAuteur.Text = livre.Auteur;
            txbCmdLivresCollection.Text = livre.Collection;
            txbCmdLivresGenre.Text = livre.Genre;
            txbCmdLivresPublic.Text = livre.Public;
            txbCmdLivresRayon.Text = livre.Rayon;
            txbCmdLivresISBN.Text = livre.Isbn;
        }

        /// <summary>
        /// Vide les zones d'affichage des infos du livre et de la liste des commandes
        /// </summary>
        private void VideCmdLivresInfos()
        {
            txbCmdLivresNum.Text = "";
            txbCmdLivresTitre.Text = "";
            txbCmdLivresAuteur.Text = "";
            txbCmdLivresCollection.Text = "";
            txbCmdLivresGenre.Text = "";
            txbCmdLivresPublic.Text = "";
            txbCmdLivresRayon.Text = "";
            txbCmdLivresISBN.Text = "";
        }

        

        /// <summary>
        /// Remplit le datagrid des commandes
        /// </summary>
        private void RemplirCmdLivresListe(List<CommandeDocument> commandes)
        {
            bdgCmdLivresListe.DataSource = commandes;
            dgvCmdLivres.DataSource = bdgCmdLivresListe;
            dgvCmdLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (dgvCmdLivres.Columns["idLivreDvd"] != null)
            {
                dgvCmdLivres.Columns["idLivreDvd"].HeaderText = "Numéro de document";
                dgvCmdLivres.Columns["idLivreDvd"].DisplayIndex = 1;
            }
            if (dgvCmdLivres.Columns["idSuivi"] != null)
                dgvCmdLivres.Columns["idSuivi"].Visible = false;
            if (dgvCmdLivres.Columns["suivi"] != null)
                dgvCmdLivres.Columns["suivi"].HeaderText = "État de la commande";
        }

        /// <summary>
        /// Sélection d'une commande dans le datagrid : affichage dans les champs de saisie
        /// </summary>
        private void DgvCmdLivres_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdLivres.CurrentCell != null && bdgCmdLivresListe.Count > 0)
            {
                grpCmdLivresCommande.Enabled = true;
                dtpCmdLivresDate.Enabled = false;
                txbCmdLivresMontant.ReadOnly = true;
                nudCmdLivresExemplaires.Enabled = false;
                CommandeDocument cmd = (CommandeDocument)bdgCmdLivresListe.List[bdgCmdLivresListe.Position];
                txbCmdLivresId.Text = cmd.id;
                txbCmdLivresNumDoc.Text = cmd.idLivreDvd;
                txbCmdLivresMontant.Text = cmd.montant.ToString();
                nudCmdLivresExemplaires.Value = cmd.nbExemplaire;
                dtpCmdLivresDate.Value = cmd.dateCommande;
                int index = (cbxCmdLivresSuivi.Items.Cast<Categorie>()).ToList().FindIndex(x => x.Id.Equals(cmd.idSuivi));
                cbxCmdLivresSuivi.SelectedIndex = index;
            }
        }

        /// <summary>
        /// Tri sur les colonnes du datagrid des commandes
        /// </summary>
        private void DgvCmdLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdLivres.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id": sortedList = lesCommandesLivres.OrderBy(o => o.id).ToList(); break;
                case "DateCommande": sortedList = lesCommandesLivres.OrderBy(o => o.dateCommande).Reverse().ToList(); break;
                case "Montant": sortedList = lesCommandesLivres.OrderBy(o => o.montant).ToList(); break;
                case "NbExemplaire": sortedList = lesCommandesLivres.OrderBy(o => o.nbExemplaire).ToList(); break;
                case "État de la commande": sortedList = lesCommandesLivres.OrderBy(o => o.suivi).ToList(); break;
            }
            RemplirCmdLivresListe(sortedList);
        }

        /// <summary>
        /// Chargement des infos du livre sélectionné dans le groupbox commande
        /// </summary>
        private void BtnCmdLivresAjouter_Click(object sender, EventArgs e)
        {
            if (leLivreCourant == null)
            {
                MessageBox.Show("Veuillez d'abord rechercher un livre.", "Information");
                return;
            }
            grpCmdLivresCommande.Enabled = true;
            dtpCmdLivresDate.Enabled = true;
            txbCmdLivresMontant.ReadOnly = false;
            nudCmdLivresExemplaires.Enabled = true;
            txbCmdLivresNumDoc.Text = leLivreCourant.Id;
            txbCmdLivresId.Text = new Random().Next(10000, 99999).ToString();
            txbCmdLivresMontant.Text = "";
            nudCmdLivresExemplaires.Value = 1;
            dtpCmdLivresDate.Value = DateTime.Now;
            cbxCmdLivresSuivi.SelectedIndex = 0;
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande
        /// </summary>
        private void BtnCmdLivresEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbCmdLivresId.Text.Equals("") || txbCmdLivresNumDoc.Text.Equals("") || txbCmdLivresMontant.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord cliquer sur Ajouter et remplir tous les champs.", "Information");
                return;
            }
            Categorie suivi = (Categorie)cbxCmdLivresSuivi.SelectedItem;
            if (!suivi.Id.Equals("00001"))
            {
                MessageBox.Show("Une nouvelle commande doit obligatoirement être en cours.", "Information");
                return;
            }
            try
            {
                double montant = double.Parse(txbCmdLivresMontant.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                CommandeDocument cmd = new CommandeDocument(
                    txbCmdLivresId.Text,
                    dtpCmdLivresDate.Value,
                    montant,
                    (int)nudCmdLivresExemplaires.Value,
                    txbCmdLivresNumDoc.Text,
                    "00001",
                    "en cours"
                );
                if (controller.CreerCommandeLivre(cmd))
                {
                    RafraichirCmdLivresListe();
                    grpCmdLivresCommande.Enabled = false;
                    txbCmdLivresId.Text = "";
                    txbCmdLivresNumDoc.Text = "";
                    txbCmdLivresMontant.Text = "";
                    nudCmdLivresExemplaires.Value = 1;
                    dtpCmdLivresDate.Value = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'enregistrement.", "Erreur");
                }
            }
            catch
            {
                MessageBox.Show("Le montant doit être une valeur numérique.", "Information");
            }
        }

        /// <summary>
        /// Modification du suivi d'une commande avec contrôle des règles métier
        /// </summary>
        private void BtnCmdLivresModifier_Click(object sender, EventArgs e)
        {
            if (txbCmdLivresId.Text.Equals("") || cbxCmdLivresSuivi.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande et une étape de suivi.", "Information");
                return;
            }
            Categorie nouveauSuivi = (Categorie)cbxCmdLivresSuivi.SelectedItem;
            CommandeDocument cmd = (CommandeDocument)bdgCmdLivresListe.List[bdgCmdLivresListe.Position];

            if ((cmd.idSuivi.Equals("00003") || cmd.idSuivi.Equals("00004")) &&
                (nouveauSuivi.Id.Equals("00001") || nouveauSuivi.Id.Equals("00002")))
            {
                MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente.", "Information");
                return;
            }
            if (nouveauSuivi.Id.Equals("00004") && !cmd.idSuivi.Equals("00003"))
            {
                MessageBox.Show("Une commande ne peut être réglée que si elle est livrée.", "Information");
                return;
            }
            cmd.idSuivi = nouveauSuivi.Id;
            cmd.suivi = nouveauSuivi.Libelle;
            if (controller.ModifierSuiviCommande(cmd))
            {
                RafraichirCmdLivresListe();
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la modification.", "Erreur");
            }
        }

        /// <summary>
        /// Suppression d'une commande si elle n'est pas encore livrée
        /// </summary>
        private void BtnCmdLivresSupprimer_Click(object sender, EventArgs e)
        {
            if (txbCmdLivresId.Text.Equals(""))
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }
            CommandeDocument cmd = (CommandeDocument)bdgCmdLivresListe.List[bdgCmdLivresListe.Position];
            if (cmd.idSuivi.Equals("00003") || cmd.idSuivi.Equals("00004"))
            {
                MessageBox.Show("Impossible de supprimer une commande livrée ou réglée.", "Information");
                return;
            }
            if (controller.SupprimerCommandeLivre(cmd.id))
            {
                RafraichirCmdLivresListe();
                grpCmdLivresCommande.Enabled = false;
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la suppression.", "Erreur");
            }
        }
        #endregion
        #region Onglet Commandes DVD
        private readonly BindingSource bdgCmdDvdSuivis = new BindingSource();
        private readonly BindingSource bdgCmdDvdListe = new BindingSource();
        private List<CommandeDocument> lesCommandesDvd = new List<CommandeDocument>();
        private Dvd leDvdCourant = null;

        /// <summary>
        /// Ouverture de l'onglet : remplit le combo des suivis et la datagrid avec toutes les commandes
        /// </summary>
        private void TabCommandesDvd_Enter(object sender, EventArgs e)
        {
            RemplirComboCategorie(controller.GetAllSuivis(), bdgCmdDvdSuivis, cbxCmdDvdSuivi);
            if (cbxCmdDvdSuivi.Items.Count > 0)
                cbxCmdDvdSuivi.SelectedIndex = 0;
            RafraichirCmdDvdListe();
            grpCmdDvdCommande.Enabled = false;
        }

        private void RafraichirCmdDvdListe()
        {
            List<string> idsDvd = controller.GetAllDvd().Select(d => d.Id).ToList();
            lesCommandesDvd = controller.GetCommandesLivre("").Where(c => idsDvd.Contains(c.idLivreDvd)).ToList();
            RemplirCmdDvdListe(lesCommandesDvd);
        }

        private void BtnCmdDvdRecherche_Click(object sender, EventArgs e)
        {
            if (txbCmdDvdNumRecherche.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de document.", "Information");
                return;
            }
            List<Dvd> dvds = controller.GetAllDvd();
            leDvdCourant = dvds.Find(x => x.Id.Equals(txbCmdDvdNumRecherche.Text));
            if (leDvdCourant != null)
            {
                AfficheCmdDvdInfos(leDvdCourant);
            }
            else
            {
                MessageBox.Show("Numéro introuvable.", "Information");
                VideCmdDvdInfos();
            }
        }

        private void AfficheCmdDvdInfos(Dvd dvd)
        {
            txbCmdDvdNum.Text = dvd.Id;
            txbCmdDvdTitre.Text = dvd.Titre;
            txbCmdDvdRealisateur.Text = dvd.Realisateur;
            txbCmdDvdDuree.Text = dvd.Duree.ToString();
            txbCmdDvdSynopsis.Text = dvd.Synopsis;
            txbCmdDvdGenre.Text = dvd.Genre;
            txbCmdDvdPublic.Text = dvd.Public;
            txbCmdDvdRayon.Text = dvd.Rayon;
        }

        private void VideCmdDvdInfos()
        {
            txbCmdDvdNum.Text = "";
            txbCmdDvdTitre.Text = "";
            txbCmdDvdRealisateur.Text = "";
            txbCmdDvdDuree.Text = "";
            txbCmdDvdSynopsis.Text = "";
            txbCmdDvdGenre.Text = "";
            txbCmdDvdPublic.Text = "";
            txbCmdDvdRayon.Text = "";
        }

        private void RemplirCmdDvdListe(List<CommandeDocument> commandes)
        {
            bdgCmdDvdListe.DataSource = commandes;
            dgvCmdDvd.DataSource = bdgCmdDvdListe;
            dgvCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (dgvCmdDvd.Columns["idLivreDvd"] != null)
            {
                dgvCmdDvd.Columns["idLivreDvd"].HeaderText = "Numéro de document";
                dgvCmdDvd.Columns["idLivreDvd"].DisplayIndex = 1;
            }
            if (dgvCmdDvd.Columns["idSuivi"] != null)
                dgvCmdDvd.Columns["idSuivi"].Visible = false;
            if (dgvCmdDvd.Columns["suivi"] != null)
                dgvCmdDvd.Columns["suivi"].HeaderText = "État de la commande";
        }

        private void DgvCmdDvd_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdDvd.CurrentCell != null && bdgCmdDvdListe.Count > 0)
            {
                grpCmdDvdCommande.Enabled = true;
                dtpCmdDvdDate.Enabled = false;
                txbCmdDvdMontant.ReadOnly = true;
                nudCmdDvdExemplaires.Enabled = false;
                CommandeDocument cmd = (CommandeDocument)bdgCmdDvdListe.List[bdgCmdDvdListe.Position];
                txbCmdDvdId.Text = cmd.id;
                txbCmdDvdNumDoc.Text = cmd.idLivreDvd;
                txbCmdDvdMontant.Text = cmd.montant.ToString();
                nudCmdDvdExemplaires.Value = cmd.nbExemplaire;
                dtpCmdDvdDate.Value = cmd.dateCommande;
                int index = (cbxCmdDvdSuivi.Items.Cast<Categorie>()).ToList().FindIndex(x => x.Id.Equals(cmd.idSuivi));
                cbxCmdDvdSuivi.SelectedIndex = index;
            }
        }

        private void DgvCmdDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdDvd.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "Id": sortedList = lesCommandesDvd.OrderBy(o => o.id).ToList(); break;
                case "DateCommande": sortedList = lesCommandesDvd.OrderBy(o => o.dateCommande).Reverse().ToList(); break;
                case "Montant": sortedList = lesCommandesDvd.OrderBy(o => o.montant).ToList(); break;
                case "NbExemplaire": sortedList = lesCommandesDvd.OrderBy(o => o.nbExemplaire).ToList(); break;
                case "État de la commande": sortedList = lesCommandesDvd.OrderBy(o => o.suivi).ToList(); break;
            }
            RemplirCmdDvdListe(sortedList);
        }

        private void BtnCmdDvdAjouter_Click(object sender, EventArgs e)
        {
            if (leDvdCourant == null)
            {
                MessageBox.Show("Veuillez d'abord rechercher un DVD.", "Information");
                return;
            }
            grpCmdDvdCommande.Enabled = true;
            dtpCmdDvdDate.Enabled = true;
            txbCmdDvdMontant.ReadOnly = false;
            nudCmdDvdExemplaires.Enabled = true;
            txbCmdDvdNumDoc.Text = leDvdCourant.Id;
            txbCmdDvdId.Text = new Random().Next(10000, 99999).ToString();
            txbCmdDvdMontant.Text = "";
            nudCmdDvdExemplaires.Value = 1;
            dtpCmdDvdDate.Value = DateTime.Now;
            cbxCmdDvdSuivi.SelectedIndex = 0;
        }

        private void BtnCmdDvdEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbCmdDvdId.Text.Equals("") || txbCmdDvdNumDoc.Text.Equals("") || txbCmdDvdMontant.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord cliquer sur Ajouter et remplir tous les champs.", "Information");
                return;
            }
            Categorie suivi = (Categorie)cbxCmdDvdSuivi.SelectedItem;
            if (!suivi.Id.Equals("00001"))
            {
                MessageBox.Show("Une nouvelle commande doit obligatoirement être en cours.", "Information");
                return;
            }
            try
            {
                double montant = double.Parse(txbCmdDvdMontant.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                CommandeDocument cmd = new CommandeDocument(
                    txbCmdDvdId.Text,
                    dtpCmdDvdDate.Value,
                    montant,
                    (int)nudCmdDvdExemplaires.Value,
                    txbCmdDvdNumDoc.Text,
                    "00001",
                    "en cours"
                );
                if (controller.CreerCommandeLivre(cmd))
                {
                    RafraichirCmdDvdListe();
                    grpCmdDvdCommande.Enabled = false;
                    txbCmdDvdId.Text = "";
                    txbCmdDvdNumDoc.Text = "";
                    txbCmdDvdMontant.Text = "";
                    nudCmdDvdExemplaires.Value = 1;
                    dtpCmdDvdDate.Value = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'enregistrement.", "Erreur");
                }
            }
            catch
            {
                MessageBox.Show("Le montant doit être une valeur numérique.", "Information");
            }
        }

        private void BtnCmdDvdModifier_Click(object sender, EventArgs e)
        {
            if (txbCmdDvdId.Text.Equals("") || cbxCmdDvdSuivi.SelectedIndex < 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande et une étape de suivi.", "Information");
                return;
            }
            Categorie nouveauSuivi = (Categorie)cbxCmdDvdSuivi.SelectedItem;
            CommandeDocument cmd = (CommandeDocument)bdgCmdDvdListe.List[bdgCmdDvdListe.Position];

            if ((cmd.idSuivi.Equals("00003") || cmd.idSuivi.Equals("00004")) &&
                (nouveauSuivi.Id.Equals("00001") || nouveauSuivi.Id.Equals("00002")))
            {
                MessageBox.Show("Une commande livrée ou réglée ne peut pas revenir à une étape précédente.", "Information");
                return;
            }
            if (nouveauSuivi.Id.Equals("00004") && !cmd.idSuivi.Equals("00003"))
            {
                MessageBox.Show("Une commande ne peut être réglée que si elle est livrée.", "Information");
                return;
            }
            cmd.idSuivi = nouveauSuivi.Id;
            cmd.suivi = nouveauSuivi.Libelle;
            if (controller.ModifierSuiviCommande(cmd))
            {
                RafraichirCmdDvdListe();
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la modification.", "Erreur");
            }
        }

        private void BtnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {
            if (txbCmdDvdId.Text.Equals(""))
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }
            CommandeDocument cmd = (CommandeDocument)bdgCmdDvdListe.List[bdgCmdDvdListe.Position];
            if (cmd.idSuivi.Equals("00003") || cmd.idSuivi.Equals("00004"))
            {
                MessageBox.Show("Impossible de supprimer une commande livrée ou réglée.", "Information");
                return;
            }
            if (controller.SupprimerCommandeLivre(cmd.id))
            {
                RafraichirCmdDvdListe();
                grpCmdDvdCommande.Enabled = false;
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la suppression.", "Erreur");
            }
        }
        #endregion

        #region Onglet Commandes Revues
        private readonly BindingSource bdgCmdRevuesListe = new BindingSource();
        private List<Abonnement> lesCommandesRevues = new List<Abonnement>();
        private Revue laRevueCourante = null;

        /// <summary>
        /// Ouverture de l'onglet
        /// </summary>
        private void TabCommandesRevues_Enter(object sender, EventArgs e)
        {
            RafraichirCmdRevuesListe();
            grpCmdRevuesCommande.Enabled = false;
        }

        /// <summary>
        /// Recharge la datagrid avec uniquement les abonnements de revues
        /// </summary>
        private void RafraichirCmdRevuesListe()
        {
            lesCommandesRevues = controller.GetCommandesRevue("");
            RemplirCmdRevuesListe(lesCommandesRevues);
        }

        /// <summary>
        /// Recherche d'une revue par numéro
        /// </summary>
        private void BtnCmdRevuesRecherche_Click(object sender, EventArgs e)
        {
            if (txbCmdRevuesNumRecherche.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un numéro de revue.", "Information");
                return;
            }
            List<Revue> revues = controller.GetAllRevues();
            laRevueCourante = revues.Find(x => x.Id.Equals(txbCmdRevuesNumRecherche.Text));
            if (laRevueCourante != null)
            {
                AfficheCmdRevuesInfos(laRevueCourante);
            }
            else
            {
                MessageBox.Show("Numéro introuvable.", "Information");
                VideCmdRevuesInfos();
            }
        }

        private void AfficheCmdRevuesInfos(Revue revue)
        {
            txbCmdRevuesNumDoc.Text = revue.Id;
            txbCmdRevuesTitre.Text = revue.Titre;
            txbCmdRevuesPeriodicite.Text = revue.Periodicite;
            txbCmdRevuesDelai.Text = revue.DelaiMiseADispo.ToString();
            txbCmdRevuesGenre.Text = revue.Genre;
            txbCmdRevuesPublic.Text = revue.Public;
            txbCmdRevuesRayon.Text = revue.Rayon;
        }

        private void VideCmdRevuesInfos()
        {
            txbCmdRevuesNumDoc.Text = "";
            txbCmdRevuesTitre.Text = "";
            txbCmdRevuesPeriodicite.Text = "";
            txbCmdRevuesDelai.Text = "";
            txbCmdRevuesGenre.Text = "";
            txbCmdRevuesPublic.Text = "";
            txbCmdRevuesRayon.Text = "";
        }

        private void RemplirCmdRevuesListe(List<Abonnement> commandes)
        {
            bdgCmdRevuesListe.DataSource = commandes;
            dgvCmdRevues.DataSource = bdgCmdRevuesListe;
            dgvCmdRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            if (dgvCmdRevues.Columns["idRevue"] != null)
            {
                dgvCmdRevues.Columns["idRevue"].HeaderText = "Numéro de revue";
                dgvCmdRevues.Columns["idRevue"].DisplayIndex = 1;
            }
        }

        /// <summary>
        /// Sélection d'une commande : affichage en lecture seule, sauf date fin
        /// </summary>
        private void DgvCmdRevues_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdRevues.CurrentCell != null && bdgCmdRevuesListe.Count > 0)
            {
                grpCmdRevuesCommande.Enabled = true;
                Abonnement abo = (Abonnement)bdgCmdRevuesListe.List[bdgCmdRevuesListe.Position];
                txbCmdRevuesId.Text = abo.id;
                txbCmdRevuesNumDoc.Text = abo.idRevue;
                txbCmdRevuesMontant.Text = abo.montant.ToString();
                dtpCmdRevuesDateCommande.Value = abo.dateCommande;
                dtpCmdRevuesDateFin.Value = abo.dateFinAbonnement;
                txbCmdRevuesId.ReadOnly = true;
                txbCmdRevuesMontant.ReadOnly = true;
                txbCmdRevuesNumDoc.ReadOnly = true;
                dtpCmdRevuesDateCommande.Enabled = false;
                dtpCmdRevuesDateFin.Enabled = true;
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        private void DgvCmdRevues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCmdRevues.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Id": sortedList = lesCommandesRevues.OrderBy(o => o.id).ToList(); break;
                case "DateCommande": sortedList = lesCommandesRevues.OrderBy(o => o.dateCommande).Reverse().ToList(); break;
                case "Montant": sortedList = lesCommandesRevues.OrderBy(o => o.montant).ToList(); break;
                case "DateFinAbonnement": sortedList = lesCommandesRevues.OrderBy(o => o.dateFinAbonnement).ToList(); break;
                case "Numéro de revue": sortedList = lesCommandesRevues.OrderBy(o => o.idRevue).ToList(); break;
            }
            RemplirCmdRevuesListe(sortedList);
        }

        /// <summary>
        /// Bouton Ajouter : prépare la saisie d'une nouvelle commande
        /// </summary>
        private void BtnCmdRevuesAjouter_Click(object sender, EventArgs e)
        {
            if (laRevueCourante == null)
            {
                MessageBox.Show("Veuillez d'abord rechercher une revue.", "Information");
                return;
            }
            grpCmdRevuesCommande.Enabled = true;
            txbCmdRevuesId.ReadOnly = true;
            txbCmdRevuesMontant.ReadOnly = false;
            dtpCmdRevuesDateCommande.Enabled = true;
            dtpCmdRevuesDateFin.Enabled = true;
            txbCmdRevuesNumDoc.ReadOnly = true;
            txbCmdRevuesNumDoc.Text = laRevueCourante.Id;
            txbCmdRevuesId.Text = new Random().Next(10000, 99999).ToString();
            txbCmdRevuesMontant.Text = "";
            dtpCmdRevuesDateCommande.Value = DateTime.Now;
            dtpCmdRevuesDateFin.Value = DateTime.Now.AddYears(1);
        }

        /// <summary>
        /// Enregistrement d'une nouvelle commande de revue
        /// </summary>
        private void BtnCmdRevuesEnregistrer_Click(object sender, EventArgs e)
        {
            if (txbCmdRevuesId.Text.Equals("") || txbCmdRevuesNumDoc.Text.Equals("") || txbCmdRevuesMontant.Text.Equals(""))
            {
                MessageBox.Show("Veuillez d'abord cliquer sur Ajouter et remplir tous les champs.", "Information");
                return;
            }
            if (dtpCmdRevuesDateFin.Value <= dtpCmdRevuesDateCommande.Value)
            {
                MessageBox.Show("La date de fin d'abonnement doit être postérieure à la date de commande.", "Information");
                return;
            }
            try
            {
                double montant = double.Parse(txbCmdRevuesMontant.Text.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                Abonnement abo = new Abonnement(
                    txbCmdRevuesId.Text,
                    dtpCmdRevuesDateCommande.Value,
                    montant,
                    dtpCmdRevuesDateFin.Value,
                    txbCmdRevuesNumDoc.Text
                );
                if (controller.CreerCommandeRevue(abo))
                {
                    RafraichirCmdRevuesListe();
                    grpCmdRevuesCommande.Enabled = false;
                    txbCmdRevuesId.Text = "";
                    txbCmdRevuesNumDoc.Text = "";
                    txbCmdRevuesMontant.Text = "";
                }
                else
                {
                    MessageBox.Show("Une erreur est survenue lors de l'enregistrement.", "Erreur");
                }
            }
            catch
            {
                MessageBox.Show("Le montant doit être une valeur numérique.", "Information");
            }
        }

        /// <summary>
        /// Renouvellement d'un abonnement (modification de la date de fin uniquement)
        /// </summary>
        private void BtnCmdRevuesRenouveler_Click(object sender, EventArgs e)
        {
            if (txbCmdRevuesId.Text.Equals(""))
            {
                MessageBox.Show("Veuillez sélectionner un abonnement à renouveler.", "Information");
                return;
            }
            Abonnement abo = (Abonnement)bdgCmdRevuesListe.List[bdgCmdRevuesListe.Position];
            if (dtpCmdRevuesDateFin.Value <= abo.dateFinAbonnement)
            {
                MessageBox.Show("La nouvelle date de fin doit être ultérieure à la date actuelle de fin d'abonnement.", "Information");
                return;
            }
            abo.dateFinAbonnement = dtpCmdRevuesDateFin.Value;
            if (controller.RenouvelerAbonnement(abo))
            {
                RafraichirCmdRevuesListe();
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors du renouvellement.", "Erreur");
            }
        }

        /// <summary>
        /// Suppression d'une commande de revue (uniquement si aucune parution rattachée)
        /// </summary>
        private void BtnCmdRevuesSupprimer_Click(object sender, EventArgs e)
        {
            if (txbCmdRevuesId.Text.Equals(""))
            {
                MessageBox.Show("Veuillez sélectionner une commande.", "Information");
                return;
            }
            Abonnement abo = (Abonnement)bdgCmdRevuesListe.List[bdgCmdRevuesListe.Position];
            // vérification : aucun exemplaire ne doit être rattaché à cet abonnement
            List<Exemplaire> exemplaires = controller.GetExemplairesRevue(abo.idRevue);
            foreach (Exemplaire ex in exemplaires)
            {
                if (ParutionDansAbonnement(abo.dateCommande, abo.dateFinAbonnement, ex.DateAchat))
                {
                    MessageBox.Show("Impossible de supprimer : au moins une parution est rattachée à cet abonnement.", "Information");
                    return;
                }
            }
            if (controller.SupprimerCommandeRevue(abo.id))
            {
                RafraichirCmdRevuesListe();
                grpCmdRevuesCommande.Enabled = false;
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la suppression.", "Erreur");
            }
        }

        /// <summary>
        /// Vérifie si une date de parution est comprise entre la date de commande et la date de fin d'abonnement
        /// </summary>
        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }
        #endregion

        /// <summary>
        /// Au chargement de l'application : alerte des abonnements se terminant dans moins de 30 jours
        /// </summary>
        private void FrmMediatek_Load(object sender, EventArgs e)
        {
            AdapterInterfaceSelonService();
            if (utilisateurConnecte.idService.Equals("00001") || utilisateurConnecte.idService.Equals("00004"))
            {
                List<Abonnement> abonnementsExpirant = controller.GetAbonnementsExpirantBientot();
                if (abonnementsExpirant.Count > 0)
                {
                    List<Revue> toutesRevues = controller.GetAllRevues();
                    string message = "Abonnements se terminant dans moins de 30 jours :\n\n";
                    foreach (Abonnement abo in abonnementsExpirant)
                    {
                        Revue revue = toutesRevues.Find(r => r.Id.Equals(abo.idRevue));
                        string titre = revue != null ? revue.Titre : abo.idRevue;
                        message += "- " + titre + " (fin : " + abo.dateFinAbonnement.ToShortDateString() + ")\n";
                    }
                    MessageBox.Show(message, "Alerte abonnements");
                }
            }
        }

        /// <summary>
        /// Adapte l'interface selon le service de l'utilisateur connecté
        /// </summary>
        private void AdapterInterfaceSelonService()
        {
            // Service Prêts : pas d'accès aux commandes ni à la réception
            if (utilisateurConnecte.idService.Equals("00002"))
            {
                tabOngletsApplication.TabPages.Remove(tabCommandesLivres);
                tabOngletsApplication.TabPages.Remove(tabCommandesDvd);
                tabOngletsApplication.TabPages.Remove(tabAbonnementsRevues);
                tabOngletsApplication.TabPages.Remove(tabReceptionRevue);
            }
        }

        private void tabCommandesLivres_Click(object sender, EventArgs e)
        {

        }

        private void btnCmdDvdSupprimer_Click(object sender, EventArgs e)
        {

        }

        private void btnCmdDvdEnregistrer_Click(object sender, EventArgs e)
        {

        }

        private void btnCmdDvdModifier_Click(object sender, EventArgs e)
        {

        }

        private void btnCmdDvdRecherche_Click(object sender, EventArgs e)
        {

        }

        private void tabCommandesDVD_Click(object sender, EventArgs e)
        {

        }

        private void tabAbonnementsRevues_Click(object sender, EventArgs e)
        {

        }
    }
}
