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
    /// Classe de test sunitaires sur la classe métier Commande
    /// </summary>
    [TestClass()]
    public class CommandeTests
    {
        private const string id = "00001";
        private static readonly DateTime dateCommande = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Local);
        private const double montant = 20;

        private static readonly Commande commande = new Commande(id, dateCommande, montant);

        /// <summary>
        /// Test sur le constructeur de la classe Commande
        /// </summary>
        [TestMethod()]
        public void CommandeTest()
        {
            Assert.AreEqual(id, commande.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(dateCommande, commande.DateCommande, "devrait réussir : date de la commande valorisée");
            Assert.AreEqual(montant, commande.Montant, "devrait réussir : montant valorisé");
        }
    }
}