using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Utilisateur (avec son service d'affectation)
    /// </summary>
    public class Utilisateur
    {
        /// <summary>Identifiant de l'utilisateur</summary>
        public string id { get; set; }
        /// <summary>Login de connexion</summary>
        public string login { get; set; }
        /// <summary>Identifiant du service d'affectation</summary>
        public string idService { get; set; }
        /// <summary>Libellé du service d'affectation</summary>
        public string service { get; set; }

        /// <summary>
        /// Constructeur par défaut (nécessaire à la désérialisation JSON)
        /// </summary>
        public Utilisateur() { }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Utilisateur(string id, string login, string idService, string service)
        {
            this.id = id;
            this.login = login;
            this.idService = idService;
            this.service = service;
        }
    }
}
