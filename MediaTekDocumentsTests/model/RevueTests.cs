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
    /// Classe de tests unitaires sur la classe métier Revue
    /// </summary>
    [TestClass()]
    public class RevueTests
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
        private const string periodicite = "MS";
        private const int delaiMiseADispo = 5;

        private static readonly Revue revue = new Revue(id, titre, image, idGenre, genre, idPublic, lepublic, idRayon, rayon, periodicite, delaiMiseADispo);

        /// <summary>
        /// Test sur le constructeur de la classe Revue
        /// </summary>
        [TestMethod()]
        public void RevueTest()
        {
            Assert.AreEqual(id, revue.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, revue.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(image, revue.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, revue.IdGenre, "devrait réussir : id du genre valorisé");
            Assert.AreEqual(genre, revue.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, revue.IdPublic, "devrait réussir : id du public valorisé");
            Assert.AreEqual(lepublic, revue.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, revue.IdRayon, "devrait réussir : id du rayon valorisé");
            Assert.AreEqual(rayon, revue.Rayon, "devrait réussir : rayon valorisé");
            Assert.AreEqual(periodicite, revue.Periodicite, "devrait réussir : periodicité valorisée");
            Assert.AreEqual(delaiMiseADispo, revue.DelaiMiseADispo, "devrait réussir : délai de mise à disposition valorisé");
        }
    }
}