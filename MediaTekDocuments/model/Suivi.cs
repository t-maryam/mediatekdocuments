using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi : étape de suivi d'une commande
    /// </summary>
    public class Suivi : Categorie
    {
        public Suivi(string id, string libelle) : base(id, libelle)
        {
        }
    }
}
