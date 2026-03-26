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
    /// Classe de tests unitaires sur la classe métier Service
    /// </summary>
    [TestClass()]
    public class ServiceTests
    {
        private const string id = "1";
        private const string libelle = "administratif";

        private static readonly Service service = new Service(id, libelle);

        /// <summary>
        /// Test sur le constructeur de la classe service
        /// </summary>
        [TestMethod()]
        public void ServiceTest()
        {
            Assert.AreEqual(id, service.Id, "devrait réussir : id valorisé");
            Assert.AreEqual(libelle, service.Libelle, "devrait réussir : libellé valorisé");
        }
    }
}