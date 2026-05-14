using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeDocument : commande d'un livre ou d'un DVD
    /// </summary>
    public class CommandeDocument
    {
        /// <summary>Identifiant de la commande</summary>
        public string id { get; set; }
        /// <summary>Date de la commande</summary>
        public DateTime dateCommande { get; set; }
        /// <summary>Montant de la commande</summary>
        public double montant { get; set; }
        /// <summary>Nombre d'exemplaires commandés</summary>
        public int nbExemplaire { get; set; }
        /// <summary>Identifiant du livre ou DVD commandé</summary>
        public string idLivreDvd { get; set; }
        /// <summary>Identifiant de l'étape de suivi</summary>
        public string idSuivi { get; set; }
        /// <summary>Libellé de l'étape de suivi (en cours, livrée, etc.)</summary>
        public string suivi { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public CommandeDocument(string id, DateTime dateCommande, double montant,
            int nbExemplaire, string idLivreDvd, string idSuivi, string suivi)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.montant = montant;
            this.nbExemplaire = nbExemplaire;
            this.idLivreDvd = idLivreDvd;
            this.idSuivi = idSuivi;
            this.suivi = suivi;
        }
    }
}
