using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement : commande d'une revue (abonnement ou renouvellement)
    /// </summary>
    public class Abonnement
    {
        /// <summary>Identifiant de la commande</summary>
        public string id { get; set; }
        /// <summary>Date de la commande</summary>
        public DateTime dateCommande { get; set; }
        /// <summary>Montant de l'abonnement</summary>
        public double montant { get; set; }
        /// <summary>Date de fin de l'abonnement</summary>
        public DateTime dateFinAbonnement { get; set; }
        /// <summary>Identifiant de la revue concernée</summary>
        public string idRevue { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Abonnement(string id, DateTime dateCommande, double montant,
            DateTime dateFinAbonnement, string idRevue)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.montant = montant;
            this.dateFinAbonnement = dateFinAbonnement;
            this.idRevue = idRevue;
        }
    }

}
