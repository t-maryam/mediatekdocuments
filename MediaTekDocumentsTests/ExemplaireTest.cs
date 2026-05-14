using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class ExemplaireTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            DateTime dateAchat = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Local);
            Exemplaire exemplaire = new Exemplaire(1, dateAchat, "photo.jpg", "00001", "10011");

            Assert.AreEqual(1, exemplaire.Numero);
            Assert.AreEqual(dateAchat, exemplaire.DateAchat);
            Assert.AreEqual("photo.jpg", exemplaire.Photo);
            Assert.AreEqual("00001", exemplaire.IdEtat);
            Assert.AreEqual("10011", exemplaire.Id);
        }
    }
}