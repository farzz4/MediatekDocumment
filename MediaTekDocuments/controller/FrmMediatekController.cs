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
    }
}