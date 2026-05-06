using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;


namespace MediaTekDocuments.view
{
    /// <summary>
    /// Fenêtre de connexion : authentifie l'utilisateur avant d'ouvrir l'application
    /// </summary>
    public partial class FrmConnexion : Form
    {
        private readonly FrmMediatekController controller;
        public Utilisateur UtilisateurConnecte { get; private set; }

        public FrmConnexion()
        {
            InitializeComponent();
            controller = new FrmMediatekController();
        }

        private void btnConnexion_Click(object sender, EventArgs e)
        {
            if (txbLogin.Text.Equals("") || txbPwd.Text.Equals(""))
            {
                MessageBox.Show("Veuillez saisir un login et un mot de passe.", "Information");
                return;
            }
            Utilisateur utilisateur = controller.GetUtilisateur(txbLogin.Text, txbPwd.Text);
            if (utilisateur == null)
            {
                MessageBox.Show("Login ou mot de passe incorrect.", "Erreur");
                return;
            }
            // utilisateur du service Culture : pas d'accès
            if (utilisateur.idService.Equals("00003"))
            {
                MessageBox.Show("Vos droits ne sont pas suffisants pour accéder à cette application.", "Accès refusé");
                Application.Exit();
                return;
            }
            // authentification réussie
            UtilisateurConnecte = utilisateur;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
