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
        /// Crée un livre dans la BDD
        /// </summary>
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        /// <summary>
        /// Modifie un livre dans la BDD
        /// </summary>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }

        /// <summary>
        /// Supprime un livre dans la BDD
        /// </summary>
        public bool SupprimerLivre(Livre livre)
        {
            return access.SupprimerLivre(livre);
        }

        /// <summary>
        /// Crée un dvd dans la BDD
        /// </summary>
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        /// <summary>
        /// Modifie un dvd dans la BDD
        /// </summary>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDvd(dvd);
        }


        /// <summary>
        /// Supprime un dvd dans la BDD
        /// </summary>
        public bool SupprimerDvd(Dvd dvd)
        {
            return access.SupprimerDvd(dvd);
        }

        /// <summary>
        /// Crée une revue dans la BDD
        /// </summary>
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        /// <summary>
        /// Modifie une revue dans la BDD
        /// </summary>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierRevue(revue);
        }

        /// <summary>
        /// Supprime une revue dans la BDD
        /// </summary>
        public bool SupprimerRevue(Revue revue)
        {
            return access.SupprimerRevue(revue);
        }

        /// <summary>
        /// Retourne les commandes d'un document
        /// </summary>
        public List<CommandeDocument> GetCommandesDocument(string idLivreDvd)
        {
            return access.GetCommandesDocument(idLivreDvd);
        }

        /// <summary>
        /// Retourne tous les suivis
        /// </summary>
        public List<Categorie> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Crée une commande de document
        /// </summary>
        public bool CreerCommandeDocument(CommandeDocument commande)
        {
            return access.CreerCommandeDocument(commande);
        }

        /// <summary>
        /// Modifie le suivi d'une commande
        /// </summary>
        public bool ModifierSuiviCommande(CommandeDocument commande)
        {
            return access.ModifierSuiviCommande(commande);
        }

        /// <summary>
        /// Supprime une commande de document
        /// </summary>
        public bool SupprimerCommandeDocument(CommandeDocument commande)
        {
            return access.SupprimerCommandeDocument(commande);
        }

        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        public List<Abonnement> GetAbonnementsRevue(string idRevue)
        {
            return access.GetAbonnementsRevue(idRevue);
        }

        /// <summary>
        /// Crée un abonnement
        /// </summary>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Supprime un abonnement
        /// </summary>
        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            return access.SupprimerAbonnement(abonnement);
        }

        /// <summary>
        /// Retourne les abonnements qui expirent dans moins de 30 jours
        /// </summary>
        public List<Abonnement> GetAbonnementsExpirantBientot()
        {
            return access.GetAbonnementsExpirantBientot();
        }

        /// <summary>
        /// Retourne tous les états
        /// </summary>
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }

        /// <summary>
        /// Modifie l'état d'un exemplaire
        /// </summary>
        public bool ModifierEtatExemplaire(Exemplaire exemplaire)
        {
            return access.ModifierEtatExemplaire(exemplaire);
        }

        /// <summary>
        /// Supprime un exemplaire
        /// </summary>
        public bool SupprimerExemplaire(Exemplaire exemplaire)
        {
            return access.SupprimerExemplaire(exemplaire);
        }

        /// <summary>
        /// Vérifie les identifiants et retourne l'utilisateur si trouvé
        /// </summary>
        public Utilisateur GetUtilisateur(string login, string pwd)
        {
            return access.GetUtilisateur(login, pwd);
        }
    }
}
