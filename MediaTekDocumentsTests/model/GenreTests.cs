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
    /// Classe de test sunitaires sur la classe métier Genre
    /// </summary>
    [TestClass()]
    public class GenreTests
    {
        private const string id = "10000";
        private const string libelle = "Humour";

        private static readonly Genre genre = new Genre(id, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Genre
        /// </summary>
        [TestMethod()]
        public void GenreTest()
        {
            Assert.AreEqual(id, genre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, genre.Libelle, "devrait réussir : libellé valorisé");
        }
    }
}