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
    /// Classe de tests unitaires sur la classe métier Suivi
    /// </summary>
    [TestClass()]
    public class SuiviTests
    {
        private const string id = "1";
        private const string libelle = "en cours";

        private static readonly Suivi suivi = new Suivi(id, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Suivi
        /// </summary>
        [TestMethod()]
        public void SuiviTest()
        {
            Assert.AreEqual(id, suivi.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, suivi.Libelle, "devrait réussir : libellé valorisé");
        }
    }
}