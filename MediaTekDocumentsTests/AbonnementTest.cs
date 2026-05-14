using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class AbonnementTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            DateTime dateCommande = new DateTime(2024, 5, 10, 0, 0, 0, DateTimeKind.Local);
            DateTime dateFinAbonnement = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Local);
            Abonnement abo = new Abonnement("54321", dateCommande, 50.00, dateFinAbonnement, "10011");

            Assert.AreEqual("54321", abo.id);
            Assert.AreEqual(dateCommande, abo.dateCommande);
            Assert.AreEqual(50.00, abo.montant);
            Assert.AreEqual(dateFinAbonnement, abo.dateFinAbonnement);
            Assert.AreEqual("10011", abo.idRevue);
        }
    }
}
