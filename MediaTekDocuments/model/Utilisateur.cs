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
        public string id { get; set; }
        public string login { get; set; }
        public string idService { get; set; }
        public string service { get; set; }

        public Utilisateur() { }

        public Utilisateur(string id, string login, string idService, string service)
        {
            this.id = id;
            this.login = login;
            this.idService = idService;
            this.service = service;
        }
    }
}
