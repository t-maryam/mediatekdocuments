using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }
        /// <summary>
        /// Retourne les commandes d'un livre
        /// </summary>
        /// <param name="idLivreDvd">id du livre concerné</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesLivre(string idLivreDvd)
        {
            return access.GetCommandesLivre(idLivreDvd);
        }

        /// <summary>
        /// Retourne toutes les étapes de suivi
        /// </summary>
        /// <returns>Liste d'objets Categorie</returns>
        public List<Categorie> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Crée une commande de livre dans la BDD
        /// </summary>
        /// <param name="commande">L'objet CommandeDocument concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommandeLivre(CommandeDocument commande)
        {
            return access.CreerCommandeLivre(commande);
        }

        /// <summary>
        /// Modifie le suivi d'une commande de livre
        /// </summary>
        /// <param name="commande">L'objet CommandeDocument concerné</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierSuiviCommande(CommandeDocument commande)
        {
            return access.ModifierSuiviCommande(commande);
        }

        /// <summary>
        /// Supprime une commande de livre
        /// </summary>
        /// <param name="id">id de la commande à supprimer</param>
        /// <returns>True si la suppression a pu se faire</returns>
        public bool SupprimerCommandeLivre(string id)
        {
            return access.SupprimerCommandeLivre(id);
        }
        /// <summary>
        /// Retourne les commandes (abonnements) d'une revue
        /// </summary>
        public List<Abonnement> GetCommandesRevue(string idRevue)
        {
            return access.GetCommandesRevue(idRevue);
        }

        /// <summary>
        /// Crée une commande de revue
        /// </summary>
        public bool CreerCommandeRevue(Abonnement abonnement)
        {
            return access.CreerCommandeRevue(abonnement);
        }

        /// <summary>
        /// Renouvelle un abonnement
        /// </summary>
        public bool RenouvelerAbonnement(Abonnement abonnement)
        {
            return access.RenouvelerAbonnement(abonnement);
        }

        /// <summary>
        /// Supprime une commande de revue
        /// </summary>
        public bool SupprimerCommandeRevue(string id)
        {
            return access.SupprimerCommandeRevue(id);
        }

        /// <summary>
        /// Retourne les abonnements expirant bientôt
        /// </summary>
        public List<Abonnement> GetAbonnementsExpirantBientot()
        {
            return access.GetAbonnementsExpirantBientot();
        }

        /// <summary>
        /// Vérifie l'authentification d'un utilisateur
        /// </summary>
        public Utilisateur GetUtilisateur(string login, string pwd)
        {
            return access.GetUtilisateur(login, pwd);
        }
    }
}
