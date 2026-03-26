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
    /// Classe de tests unitaires sur la classe métier Rayon
    /// </summary>
    [TestClass()]
    public class RayonTests
    {
        private const string id = "BD001";
        private const string libelle = "BD Adultes";

        private static readonly Rayon rayon = new Rayon(id, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Rayon
        /// </summary>
        [TestMethod()]
        public void RayonTest()
        {
            Assert.AreEqual(id, rayon.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, rayon.Libelle, "devrait réussir : libellé valorisé");
        }
    }
}