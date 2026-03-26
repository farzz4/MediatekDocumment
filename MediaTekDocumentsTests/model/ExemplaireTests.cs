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
    /// Classes de tests unitaires sur la classe métier Exemplaire
    /// </summary>
    [TestClass()]
    public class ExemplaireTests
    {
        private const int numero = 00001;
        private const string photo = "photoExemplaire";
        private static readonly DateTime dateAchat = new DateTime(2025, 3, 18, 0, 0, 0, DateTimeKind.Local);
        private const string idEtat = "00001";
        private const string idDocument = "00001";
        private const string libelle = "neuf";

        private static readonly Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Exemplaire
        /// </summary>
        [TestMethod()]
        public void ExemplaireTest()
        {
            Assert.AreEqual(numero, exemplaire.Numero, "devrait réussir : numéro valorisé");
            Assert.AreEqual(photo, exemplaire.Photo, "devrait réussir : photo valorisée");
            Assert.AreEqual(dateAchat, exemplaire.DateAchat, "devrait réussir : date d'achat valorisée");
            Assert.AreEqual(idEtat, exemplaire.IdEtat, "devrait réussir : id de l'état valorisé");
            Assert.AreEqual(idDocument, exemplaire.Id, "devrait réussir : id du document valorisé");
            Assert.AreEqual(libelle, exemplaire.Libelle, "devrait réussir : libelle de l'état valorisé");
        }
    }
}