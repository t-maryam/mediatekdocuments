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
        public string id { get; set; }
        public DateTime dateCommande { get; set; }
        public double montant { get; set; }
        public int nbExemplaire { get; set; }
        public string idLivreDvd { get; set; }
        public string idSuivi { get; set; }
        public string suivi { get; set; }

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
