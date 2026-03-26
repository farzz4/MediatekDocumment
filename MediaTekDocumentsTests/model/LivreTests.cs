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
    /// Classe de tests unitaires sur la classe métier Livre
    /// </summary>
    [TestClass()]
    public class LivreTests
    {
        private const string id = "00001";
        private const string titre = "titre du livre";
        private const string image = "image";
        private const string isbn = "123456789";
        private const string auteur = "auteur du livre";
        private const string collection = "collection du livre";
        private const string idGenre = "10018";
        private const string genre = "Actualités";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "BD001";
        private const string rayon = "BD adultes";

        private static readonly Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idGenre, genre, idPublic, lepublic, idRayon, rayon);
        /// <summary>
        /// Test sur le constructeur de la classe Livre
        /// </summary>
        [TestMethod()]
        public void LivreTest()
        {
            Assert.AreEqual(id, livre.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, livre.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, livre.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(isbn, livre.Isbn, "devrait réussir : isbn valorisé");
            Assert.AreEqual(auteur, livre.Auteur, "devrait réussir : auteur valorisé");
            Assert.AreEqual(collection, livre.Collection, "devrait réussir : collection valorisée");
            Assert.AreEqual(idGenre, livre.IdGenre, "devrait réussir : id du genre valorisé");
            Assert.AreEqual(genre, livre.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, livre.IdPublic, "devrait réussir : id du public valorisé");
            Assert.AreEqual(lepublic, livre.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, livre.IdRayon, "devrait réussir : id du rayon valorisé");
            Assert.AreEqual(rayon, livre.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}