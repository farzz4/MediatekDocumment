using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model.Tests
{
    /// <summary>
    /// Classe de tests unitaires sur la classe métier CommandeDocument
    /// </summary>
    [TestClass()]
    public class CommandeDocumentTests
    {
        private const string id = "00001";
        private static readonly DateTime dateCommande = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Local);
        private const double montant = 20;
        private const int nbExemplaire = 5;
        private const string idLivreDvd = "00020";
        private const string idSuivi = "1";
        private const string libelle = "en cours";

        private static readonly CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe CommandeDocument
        /// </summary>
        [TestMethod()]
        public void CommandeDocumentTest()
        {
            Assert.AreEqual(id, commandeDocument.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commandeDocument.DateCommande, "devrait réussir : date de la commande valorisée");
            Assert.AreEqual(montant, commandeDocument.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(nbExemplaire, commandeDocument.NbExemplaire, "devrait réussir : le nombre d'exemplaire est valorisé");
            Assert.AreEqual(idLivreDvd, commandeDocument.IdLivreDvd, "devrait réussir : l'id du livreDvd est valorisé");
            Assert.AreEqual(idSuivi, commandeDocument.IdSuivi, "devrait réussir : l'id du suivi est valorisé");
            Assert.AreEqual(libelle, commandeDocument.Libelle, "devrait réussir : le libellé du suivi est valorisé");
        }
    }
}