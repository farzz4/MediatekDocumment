using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Net;
using System.IO;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest3/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            Console.WriteLine("Liste : " + (lesGenres != null ? "Success" : "Failed"));
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne tous les documents enregistrés dans la BDD
        /// </summary>
        /// <returns></returns>
        public List<Document> GetAllDocuments()
        {
            List<Document> lesDoc = TraitementRecup<Document>(GET, "document", null);
            return lesDoc;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne toutes les commandes à partir de la BDD 
        /// </summary>
        /// <returns>Liste d'objets Commande</returns>
        public List<Commande> GetAllCommandes()
        {
            List<Commande> lesCommandes = TraitementRecup<Commande>(GET, "commande", null);
            return lesCommandes;
        }

        /// <summary>
        /// Retourne tous les abonnements à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAllAbonnements()
        {
            List<Abonnement> lesAbonnements = TraitementRecup<Abonnement>(GET, "abonnement", null);
            return lesAbonnements;
        }

        /// <summary>
        /// Retourne tous les suivis à partir de la BDD
        /// </summary>
        /// <returns>List d'objets suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivis;
        }

        /// <summary>
        /// Retourne tous les états à partir de la BDD
        /// </summary>
        /// <returns>List d'objets etats</returns>
        public List<Etat> GetAllEtats()
        {
            List<Etat> lesEtats = TraitementRecup<Etat>(GET, "etat", null);
            return lesEtats;
        }

        /// <summary>
        /// Retourne les abonnements arrivant à échéance
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsEcheance()
        {
            List<Abonnement> lesAbonnementsEcheance = TraitementRecup<Abonnement>(GET, "abonnementecheance", null);
            return lesAbonnementsEcheance;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplairesDocument = TraitementRecup<Exemplaire>(GET, "exemplairedocument/" + jsonIdDocument, null);
            return lesExemplairesDocument;
        }

        /// <summary>
        /// Retourne les commandes d'un document
        /// </summary>
        /// <param name="idDocument">id du document</param>
        /// <returns>Liste d'objets CommandeDocument</returns>
        public List<CommandeDocument> GetCommandesDocument(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<CommandeDocument> lesCommandesDocument = TraitementRecup<CommandeDocument>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesCommandesDocument;
        }

        /// <summary>
        /// Retourne les abonnements d'une revue
        /// </summary>
        /// <param name="idDocument">id du document</param>
        /// <returns>List d'objets Abonnement</returns>
        public List<Abonnement> GetAbonnementsRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Abonnement> lesAbonnementsRevue = TraitementRecup<Abonnement>(GET, "abonnementrevue/" + jsonIdDocument, null);
            return lesAbonnementsRevue;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonDateAchat = JsonConvert.SerializeObject(exemplaire.DateAchat, new CustomDateTimeConverter());
            String jsonExemplaire = "{ \"id\" : \"" + exemplaire.Id + "\", " +
                        "              \"numero\" : \"" + exemplaire.Numero + "\", " +
                        "              \"dateAchat\" : " + jsonDateAchat + ", " +
                        "              \"photo\" : \"" + exemplaire.Photo + "\", " +
                        "              \"idEtat\" : \"" + exemplaire.IdEtat + "\"}";
            Console.WriteLine(jsonExemplaire);
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'un document en base de données
        /// </summary>
        /// <param name="id">id du documnent</param>
        /// <param name="titre">titre du document</param>
        /// <param name="image">image du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="idPublic">id du public du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerDocument(string id, string titre, string image, string idRayon, string idPublic, string idGenre)
        {
            String jsonDocument = "{ \"id\" : \"" + id + "\", " +
                "                    \"titre\" : \"" + titre + "\", " +
                "                    \"image\" : \"" + image + "\", " +
                "                    \"idRayon\" : \"" + idRayon + "\", " +
                "                    \"idPublic\" : \"" + idPublic + "\", " +
                "                    \"idGenre\" : \"" + idGenre + "\"}";
            Console.WriteLine("JSON Document: " + jsonDocument);
            try
            {
                List<Document> liste = TraitementRecup<Document>(POST, "document", "champs=" + jsonDocument);
                Console.WriteLine("Liste Documents: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Modification d'un document en base de données
        /// </summary>
        /// <param name="id">id du document</param>
        /// <param name="titre">titre du documnent</param>
        /// <param name="image">image du document</param>
        /// <param name="idRayon">id du rayon du document</param>
        /// <param name="idPublic">id du public du document</param>
        /// <param name="idGenre">id du genre du document</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierDocument(string id, string titre, string image, string idRayon, string idPublic, string idGenre)
        {
            String jsonDocument = "{ \"id\" : \"" + id + "\", " +
                "                    \"titre\" : \"" + titre + "\", " +
                "                    \"image\" : \"" + image + "\", " +
                "                    \"idRayon\" : \"" + idRayon + "\", " +
                "                    \"idPublic\" : \"" + idPublic + "\", " +
                "                    \"idGenre\" : \"" + idGenre + "\"}";
            Console.WriteLine("JSON Document: " + jsonDocument);
            try
            {
                List<Document> liste = TraitementRecup<Document>(PUT, "document", "id=" + id + "&champs=" + jsonDocument);
                Console.WriteLine("Liste Documents: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// ecriture d'un livre en base de données
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <param name="Isbn">isbn du livre</param>
        /// <param name="auteur">auteur du livre</param>
        /// <param name="collection">collection du livre</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerLivre(string id, string Isbn, string auteur, string collection)
        {
            String jsonLivre = "{ \"id\" : \"" + id + "\", " +
                "                 \"isbn\" : \"" + Isbn + "\", " +
                "                 \"auteur\" : \"" + auteur + "\", " +
                "                 \"collection\" : \"" + collection + "\"}";
            Console.WriteLine("JSON Livre: " + jsonLivre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre", "champs=" + jsonLivre);
                Console.WriteLine("Liste Livres: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Modification d'un livre en base de données
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <param name="Isbn">isbn du livre</param>
        /// <param name="auteur">auteur du livre</param>
        /// <param name="collection">collection du livre</param>
        /// <returns></returns>
        public bool ModifierLivre(string id, string Isbn, string auteur, string collection)
        {
            String jsonLivre = "{ \"id\" : \"" + id + "\", " +
                "                 \"isbn\" : \"" + Isbn + "\", " +
                "                 \"auteur\" : \"" + auteur + "\", " +
                "                 \"collection\" : \"" + collection + "\"}";
            Console.WriteLine("JSON Livre: " + jsonLivre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre", "id=" + id + "&champs=" + jsonLivre);
                Console.WriteLine("Liste Livres: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Suppression d'un livre en base de données
        /// </summary>
        /// <param name="id">id du livre</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerLivre(string id)
        {
            String jsonIdLivre = "{\"id\":\"" + id + "\"}";
            Console.WriteLine("JSON idLivre: " + jsonIdLivre);
            try
            { 
                List<Livre> liste = TraitementRecup<Livre>(DELETE, "livre/" + jsonIdLivre, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return false;
        }

        /// <summary>
        /// Ecriture d'un dvd en base de données
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <param name="synopsis">synopsis du dvd</param>
        /// <param name="realisateur">realisateur du dvd</param>
        /// <param name="duree">duree du dvd</param>
        /// <returns>true si la création a pu se faire</returns>
        public bool CreerDvd(string id, string synopsis, string realisateur, int duree)
        {
            String jsonDvd = "{ \"id\" : \"" + id +  "\", " +
                "              \"synopsis\" : \"" + synopsis + "\", " +
                "              \"realisateur\" : \"" + realisateur + "\", " +
                "              \"duree\" : \"" + duree + "\"} ";
            Console.WriteLine("Json Dvd: " + jsonDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd", "champs=" + jsonDvd);
                Console.WriteLine("Liste Dvd: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Modification d'un dvd en base de données
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <param name="synopsis">synopsis du dvd</param>
        /// <param name="realisateur">realisateur du dvd</param>
        /// <param name="duree">duree du dvd</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierDvd(string id, string synopsis, string realisateur, int duree)
        {
            String jsonDvd = "{ \"id\" : \"" + id + "\", " +
                "              \"synopsis\" : \"" + synopsis + "\", " +
                "              \"realisateur\" : \"" + realisateur + "\", " +
                "              \"duree\" : \"" + duree + "\"} ";
            Console.WriteLine("Json Dvd: " + jsonDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd", "id=" + id + "&champs=" + jsonDvd);
                Console.WriteLine("Liste Dvd: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un dvd en base de données
        /// </summary>
        /// <param name="id">id du dvd</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerDvd(string id)
        {
            String jsonIdDvd = "{\"id\":\"" + id + "\"}";
            Console.WriteLine("Json id Dvd : " + jsonIdDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(DELETE, "dvd/" + jsonIdDvd, null);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture d'une revue en base de données
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <param name="periodicite">periodicité de la revue</param>
        /// <param name="delaiMiseADispo">delai de mise à dispo de la revue</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerRevue(string id, string periodicite, int delaiMiseADispo)
        {
            String jsonRevue = "{ \"id\" : \"" + id + "\", " +
                "              \"periodicite\" : \"" + periodicite + "\", " +
                "              \"delaiMiseADispo\" : \"" + delaiMiseADispo + "\"} ";
            Console.WriteLine("Json Revue " + jsonRevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue", "champs=" + jsonRevue);
                Console.WriteLine("Liste Revue: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modification d'une revue en base de données
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <param name="periodicite">periodicite de la revue</param>
        /// <param name="delaiMiseADispo">delai de mise à dispo de la revue</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierRevue(string id, string periodicite, int delaiMiseADispo)
        {
            String jsonRevue = "{ \"id\" : \"" + id + "\", " +
                "              \"periodicite\" : \"" + periodicite + "\", " +
                "              \"delaiMiseADispo\" : \"" + delaiMiseADispo + "\"} ";
            Console.WriteLine("Json Revue " + jsonRevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue", "id=" + id + "&champs=" + jsonRevue);
                Console.WriteLine("Liste Revue: " + (liste != null ? "Success" : "Failed"));
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'une revue en base de données
        /// </summary>
        /// <param name="id">id de la revue</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerRevue(string id)
        {
            String jsonIdRevue = "{\"id\":\"" + id + "\"}";
            Console.WriteLine("Json id revue : " + jsonIdRevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue/" + jsonIdRevue, null);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture d'une commande en base de données
        /// </summary>
        /// <param name="commande">Commande à insérer</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommande(Commande commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<Commande> liste = TraitementRecup<Commande>(POST, "commande", "champs=" + jsonCommande);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture d'une commande d'un document en base de données
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <param name="nbExemplaire">nb d'exemplaires de la commande</param>
        /// <param name="idLivreDvd">id du livreDVD de la commande</param>
        /// <param name="idSuivi">id du suivi de la commande </param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerCommandeDocument(string id, int nbExemplaire, string idLivreDvd, string idSuivi)
        {
            String jsonCommandeDocument = "{ \"id\" : \"" + id + "\", " +
                        "                    \"nbExemplaire\" : \"" + nbExemplaire + "\", " +
                        "                    \"idLivreDvd\" : \"" + idLivreDvd + "\", " +
                        "                    \"idSuivi\" : \"" + idSuivi + "\"}";
            Console.WriteLine(jsonCommandeDocument);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(POST, "commandedocument", "champs=" + jsonCommandeDocument);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Modification du suivi d'une commande dans la bdd
        /// </summary>
        /// <param name="id">id du nouveau suivi</param>
        /// <returns>true si la modification a pu se faire</returns>
        public bool ModifierSuiviCommandeDocument(string id, string idSuivi)
        {
            String jsonCommandeDocument = "{ \"id\" : \"" + id + "\", " +
                        "                    \"idSuivi\" : \"" + idSuivi + "\"}";
            Console.WriteLine(jsonCommandeDocument);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(PUT, "commandedocument", "id=" + id + "&champs=" + jsonCommandeDocument);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Suppression d'une commande d'un document en base de données
        /// </summary>
        /// <param name="id">id de la commande</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerCommandeDocument(string id)
        {
            String jsonIdCommande = "{\"id\":\"" + id + "\"}";
            Console.WriteLine(jsonIdCommande);
            try
            {
                List<CommandeDocument> liste = TraitementRecup<CommandeDocument>(DELETE, "commandedocument/" + jsonIdCommande, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Ecriture d'un abonnement d'une revue en base de données
        /// </summary>
        /// <param name="abonnement">Abonnement à insérer</param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public bool CreerAbonnementRevue(string id, DateTime dateFinAbonnement, string idRevue)
        {
            String jsonDateFinAbonnement = JsonConvert.SerializeObject(dateFinAbonnement, new CustomDateTimeConverter());
            String jsonAbonnementRevue = "{ \"id\" : \"" + id + "\", " +
            "                    \"dateFinAbonnement\" : " + jsonDateFinAbonnement + ", " +
            "                    \"idRevue\" : \"" + idRevue + "\"}";
            Console.WriteLine(jsonAbonnementRevue);
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonAbonnementRevue);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un abonnement d'une revue en base de données
        /// </summary>
        /// <param name="id">id de l'abonnement</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerAbonnementRevue(string id)
        {
            String jsonIdAbonnement = "{\"id\":\"" + id + "\"}";
            Console.WriteLine(jsonIdAbonnement);
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "abonnement/" + jsonIdAbonnement, null);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
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
            return (DateTime.Compare(dateCommande, dateParution) < 0
                 && DateTime.Compare(dateParution, dateFinAbonnement) < 0);
        }

        /// <summary>
        /// Modification de l'état d'un exemplaire d'un document dans la bdd
        /// </summary>
        /// <param name="numero">numero du document</param>
        /// <param name="idEtat">id de l'état à modifier</param>
        /// <returns>true si la modification a puse faire</returns>
        public bool ModifierEtatExemplaireDocument(Exemplaire exemplaire)
        {
            String jsonEtatExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            Console.WriteLine(jsonEtatExemplaire);
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(PUT, "exemplairedocument", "id=" + exemplaire.Numero + "&champs=" + jsonEtatExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Suppression d'un exemplaire d'un document en base de données
        /// </summary>
        /// <param name="exemplaire">Objet exemplaire à supprimer</param>
        /// <returns>true si la suppression a pu se faire</returns>
        public bool SupprimerExemplaireDocument(Exemplaire exemplaire)
        {
            String jsonSupprimerExemplaire = "{ \"id\" : \"" + exemplaire.Id + "\", " +
                           "                    \"numero\" : \"" + exemplaire.Numero + "\"}";
            Console.WriteLine(jsonSupprimerExemplaire);
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(DELETE, "exemplaire/" + jsonSupprimerExemplaire, null);
                return (liste != null);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}
