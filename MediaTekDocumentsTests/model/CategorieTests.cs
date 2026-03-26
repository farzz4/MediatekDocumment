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
    /// Classe de tests unitaires sur la classe métier Catégorie
    /// </summary>
    [TestClass()]
    public class CategorieTests
    {
        private const string id = "00001";
        private const string libelle = "Horreur";

        private static readonly Categorie categorie = new Categorie(id, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Categorie
        /// </summary>
        [TestMethod()]
        public void CategorieTest()
        {
            Assert.AreEqual(id, categorie.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, categorie.Libelle, "devrait réussir : libellé valorisé");
        }

        /// <summary>
        /// Test sur la méthode ToString de la classe Categorie
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("Horreur", libelle.ToString(), "devrait réussir : libellé au format string");
        }
    }
}