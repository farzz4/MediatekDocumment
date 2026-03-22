using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System;

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

        public List<Document> GetAllDocuments()
        {
            return access.GetAllDocuments();
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
        /// getter sur les commandes
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public List<Commande> GetAllCommandes()
        {
            return access.GetAllCommandes();
        }

        /// <summary>
        /// getter sur les abonnements
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAllAbonnements()
        {
            return access.GetAllAbonnements();
        }

        /// <summary>
        /// getter sur les suivis
        /// </summary>
        /// <returns>Liste d'objets Suivis</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }

        /// <summary>
        /// Récupère les abonnements arrivant à échéance
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsEcheance()
        {
            return access.GetAbonnementsEcheance();
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            return access.GetExemplairesRevue(idDocument);
        }

        /// <summary>
        /// Recupère les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns></returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            return access.GetExemplairesDocument(idDocument);
        }

        /// <summary>
        /// Récupère les commandes d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<CommandeDocument> GetCommandeDocuments(string idDocument)
        {
            return access.GetCommandesDocument(idDocument);
        }

        /// <summary>
        /// Récupère les abonnements d'une revue
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsRevue(string idDocument)
        {
            return access.GetAbonnementsRevue(idDocument);
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
        /// Créé un document dans la bdd
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">image du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="idPublic">id du public du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerDocument(string id, string titre, string image, string idRayon, string idPublic, string idGenre)
        {
            return access.CreerDocument(id, titre, image, idRayon, idPublic, idGenre);
        }

        /// <summary>
        /// Modifie un document dans la bdd
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">image du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="idPublic">id du public du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <returns>True si la modification a pu se faire</returns>
        public bool ModifierDocument(string id, string titre, string image, string idRayon, string idPublic, string idGenre)
        {
            return access.ModifierDocument(id, titre, image, idRayon, idPublic, idGenre);
        }


        /// <summary>
        /// Créé un livre dans la bdd
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <param name="isbn">isbn du livre</param>
        /// <param name="auteur">auteur du livre</param>
        /// <param name="collection">collection du livre</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerLivre(string id, string Isbn, string auteur, string collection)
        {
            return access.CreerLivre(id, Isbn, auteur, collection);
        }

        /// <summary>
        /// Modifie un livre dans la bdd
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <param name="Isbn">isbn du livre</param>
        /// <param name="auteur">auteur du livre</param>
        /// <param name="collection">collection du livre</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool ModifierLivre(string id, string Isbn, string auteur, string collection)
        {
            return access.ModifierLivre(id, Isbn, auteur, collection);
        }

        /// <summary>
        /// Supprime un livre dans la bdd
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerLivre(string id)
        {
            return access.SupprimerLivre(id);
        }

        /// <summary>
        /// Cree un dvd dans la bdd
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <param name="synopsis">synopsis du dvd</param>
        /// <param name="realisateur">realisateur du dvd</param>
        /// <param name="duree">duree du dvd</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerDvd(string id, string synopsis, string realisateur, int duree)
        {
            return access.CreerDvd(id, synopsis, realisateur, duree);
        }

        /// <summary>
        /// Modifie un dvd dans la bdd
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <param name="synopsis">synopsis du dvd</param>
        /// <param name="realisateur">realisateur du dvd</param>
        /// <param name="duree">duree du dvd</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierDvd(string id, string synopsis, string realisateur, int duree)
        {
            return access.ModifierDvd(id, synopsis, realisateur, duree);
        }

        /// <summary>
        /// Supprime un dvd dans la bdd
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerDvd(string id)
        {
            return access.SupprimerDvd(id);
        }

        /// <summary>
        /// Cree une revue dans la bdd
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <param name="periodicite">periodicite de la revue</param>
        /// <param name="delaiMiseADispo">delai de mise à dispo de la revue</param>
        /// <returns></returns>
        public bool CreerRevue(string id, string periodicite, int delaiMiseADispo)
        {
            return access.CreerRevue(id, periodicite, delaiMiseADispo);
        }

        /// <summary>
        /// Modifie une revue dans la bdd
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <param name="periodicite">periodicite de la revue</param>
        /// <param name="delaiMiseADispo">delai de mise à dispo de la revue</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierRevue(string id, string periodicite, int delaiMiseADispo)
        {
            return access.ModifierRevue(id, periodicite, delaiMiseADispo);
        }

        /// <summary>
        /// Supprime une revue dans la bdd
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerRevue(string id)
        {
            return access.SupprimerRevue(id);
        }

        /// <summary>
        /// Créé une commande dans la bdd
        /// </summary>
        /// <param name="commande">objet commande</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Créé une commande d'un document dans la bdd
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="nbExemplaire">nb d'exemplaire dans la commande</param>
        /// <param name="idLivreDvd">id du livreDvd de la commande</param>
        /// <param name="idSuivi">id du suivi de la commande</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            return access.CreerCommandeDocument(id, nbExemplaire, idLivreDvd, idSuivi);
        }

        /// <summary>
        /// Modifie le suivi d'une commande d'un document dans la bdd 
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="nbExemplaire">nb d'exemplaires dans la commande</param>
        /// <param name="idLivreDvd">id du livreDvd de la commande</param>
        /// <param name="idSuivi">id du suivi de la commande</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierSuiviCommandeDocument(string id, string idSuivi)
        {
            return access.ModifierSuiviCommandeDocument(id, idSuivi);
        }

        /// <summary>
        /// Supprime une commande d'un document dans la bdd
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(string id)
        {
            return access.SupprimerCommandeDocument(id);
        }

        /// <summary>
        /// Créé un abonnement d'une revue dans la bdd
        /// </summary>
        /// <param name="abonnement">objet abonnement</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerAbonnementRevue(string id, DateTime dateFinAbonnement, string idRevue)
        {
            return access.CreerAbonnementRevue(id, dateFinAbonnement, idRevue);
        }

        /// <summary>
        /// Supprime un abonnement d'une revue dans la bdd 
        /// </summary>
        /// <param name="id">id de l'abonnement</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerAbonnementRevue(string id)
        {
            return access.SupprimerAbonnementRevue(id);
        }

        /// <summary>
        /// Vérifier si une parution est associée à une commande donnée
        /// </summary>
        /// <param name="dateCommande">date de la commande</param>
        /// <param name="dateFinAbonnement">date de fin de l'abonnement</param>
        /// <param name="dateParution">date de parution de la revue</param>
        /// <returns>true si la date de parution est entre les 2 autres dates</returns>
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return access.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);
        }

    }

}