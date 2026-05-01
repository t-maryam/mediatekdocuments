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
        public string id { get; set; }
        public DateTime dateCommande { get; set; }
        public double montant { get; set; }
        public DateTime dateFinAbonnement { get; set; }
        public string idRevue { get; set; }

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
