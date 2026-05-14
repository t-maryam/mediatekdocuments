using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class CommandeDocumentTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            DateTime dateCommande = new DateTime(2024, 5, 10, 0, 0, 0, DateTimeKind.Local);
            CommandeDocument commande = new CommandeDocument("12345", dateCommande, 25.50, 3,
                "00001", "00001", "en cours");

            Assert.AreEqual("12345", commande.id);
            Assert.AreEqual(dateCommande, commande.dateCommande);
            Assert.AreEqual(25.50, commande.montant);
            Assert.AreEqual(3, commande.nbExemplaire);
            Assert.AreEqual("00001", commande.idLivreDvd);
            Assert.AreEqual("00001", commande.idSuivi);
            Assert.AreEqual("en cours", commande.suivi);
        }
    }
}