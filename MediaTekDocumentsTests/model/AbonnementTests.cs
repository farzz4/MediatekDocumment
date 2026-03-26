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
    /// Classe de tests unitaires sur la classe métier Abonnement
    /// </summary>
    [TestClass()]
    public class AbonnementTests
    {
        private const string id = "00001";
        private static readonly DateTime dateCommande = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Local);
        private const double montant = 20;
        private static readonly DateTime dateFinAbonnement = new DateTime(2025, 3, 25, 0, 0, 0, DateTimeKind.Local);
        private const string idRevue = "00002";
        private const string titre = "Titre du document";

        private static readonly Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue, titre);

        /// <summary>
        /// Test sur le constructeur de la classe Abonnement
        /// </summary>
        [TestMethod()]
        public void AbonnementTest()
        {
            Assert.AreEqual(id, abonnement.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, abonnement.DateCommande, "devrait réussir : date de commande valorisée");
            Assert.AreEqual(montant, abonnement.Montant, "devrait réussir : montant valorisé");
            Assert.AreEqual(dateFinAbonnement, abonnement.DateFinAbonnement, "devrait réussir : date de fin d'abonnement valorisée");
            Assert.AreEqual(idRevue, abonnement.IdRevue, "devrait réussir : id de la revue valorisé");
            Assert.AreEqual(titre, abonnement.Titre, "devrait réussir : titre de la revue valorisé");
        }
    }
}