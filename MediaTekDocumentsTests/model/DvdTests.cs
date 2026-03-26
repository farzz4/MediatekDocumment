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
    /// Classe de tests unitaires sur la classe métier Dvd
    /// </summary>
    [TestClass()]
    public class DvdTests
    {
        private const string id = "00001";
        private const string titre = "titre du livre";
        private const int duree = 120;
        private const string realisateur = "realisateurDvd";
        private const string synopsis = "synopsisDvd";
        private const string image = "image";
        private const string idGenre = "10018";
        private const string genre = "Actualités";
        private const string idPublic = "00004";
        private const string lepublic = "Ados";
        private const string idRayon = "BD001";
        private const string rayon = "BD adultes";

        private static readonly Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idGenre, genre, idPublic, lepublic, idRayon, rayon);

        /// <summary>
        /// Test sur le constructeur de la class Dvd
        /// </summary>
        [TestMethod()]
        public void DvdTest()
        {
            Assert.AreEqual(id, dvd.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(titre, dvd.Titre, "devrait réussir : titre valorisé");
            Assert.AreEqual(duree, dvd.Duree, "devrait réussir : duree valorisée");
            Assert.AreEqual(realisateur, dvd.Realisateur, "devrait réussir : realisateur valorisé");
            Assert.AreEqual(synopsis, dvd.Synopsis, "devrait réussir : synopsis valorisé");
            Assert.AreEqual(image, dvd.Image, "devrait réussir : image valorisée");
            Assert.AreEqual(idGenre, dvd.IdGenre, "devrait réussir : id du genre valorisé");
            Assert.AreEqual(genre, dvd.Genre, "devrait réussir : genre valorisé");
            Assert.AreEqual(idPublic, dvd.IdPublic, "devrait réussir : id du public valorisé");
            Assert.AreEqual(lepublic, dvd.Public, "devrait réussir : public valorisé");
            Assert.AreEqual(idRayon, dvd.IdRayon, "devrait réussir : id du rayon valorisé");
            Assert.AreEqual(rayon, dvd.Rayon, "devrait réussir : rayon valorisé");
        }
    }
}