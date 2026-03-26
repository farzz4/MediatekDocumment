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
    /// Classe de tests unitaires sur la classe métier Utilisateur
    /// </summary>
    [TestClass()]
    public class UtilisateurTests
    {
        private const string login = "nolanRooney";
        private const string password = "nR1234";
        private const string idService = "1";
        private const string libelle = "administratif";

        private static readonly Utilisateur utilisateur = new Utilisateur(login, password, idService, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe Utilisateur
        /// </summary>
        [TestMethod()]
        public void UtilisateurTest()
        {
            Assert.AreEqual(login, utilisateur.Login, "devrait réussir : login valorisé");
            Assert.AreEqual(password, utilisateur.Password, "devrait réussir : password valorisé");
            Assert.AreEqual(idService, utilisateur.IdService, "devrait réussir : id du service valorisé");
            Assert.AreEqual(libelle, utilisateur.Libelle, "devrait réussir : libellé du service valorisé");
        }
    }
}