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
    /// Classe de tests unitaires sur la classe métier Document
    /// </summary>
    [TestClass()]
    public class DocumentTests
    {
        private const string id = "00001";
        private const string titre = "titre du livre";
        private const string image = "image";
        private const string idGenre = "10018";
        private const string genre = "Actualités";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "BD001";
        private const string rayon = "BD adultes";

        private static readonly Document document = new Document(id, titre, image, idGenre, genre, idPublic, lepublic, idRayon, rayon);

        /// <summary>
        /// Test sur le constructeur de la classe Document
        /// </summary>
        [TestMethod()]
        public void DocumentTest()
        {
            Assert.AreEqual(id, document.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, document.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, document.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, document.IdGenre, "devrait réussir : id du genre valorisé");
            Assert.AreEqual(genre, document.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, document.IdPublic, "devrait réussir : id du public valorisé");
            Assert.AreEqual(lepublic, document.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, document.IdRayon, "devrait réussir : id du rayon valorisé");
            Assert.AreEqual(rayon, document.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}