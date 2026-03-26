using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.dal.Tests
{ 
    /// <summary>
    /// Tests sur la classe d'accès aux données Access
    /// </summary>
    [TestClass()]
    public class AccessTests
    {
        private static readonly Access access = Access.GetInstance();

        /// <summary>
        /// Test sur la méthode ParutionDansAbonnement
        /// </summary>
        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            DateTime dateCommande = new DateTime(2025, 03, 05, 0, 0, 0, DateTimeKind.Local);
            DateTime dateParution = new DateTime(2025, 03, 06, 0, 0, 0, DateTimeKind.Local);
            DateTime dateFinAbonnement = new DateTime(2025, 03, 07, 0, 0, 0, DateTimeKind.Local);

            bool expected = true;
            bool actual = access.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.AreEqual(expected, actual);
        }
    }
}