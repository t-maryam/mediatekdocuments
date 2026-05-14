using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class LivreTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Livre livre = new Livre("00001", "Titre", "image.jpg", "1234567890", "Auteur",
                "Collection", "10001", "Roman", "00002", "Adultes", "LV001", "Littérature");

            Assert.AreEqual("00001", livre.Id);
            Assert.AreEqual("Titre", livre.Titre);
            Assert.AreEqual("image.jpg", livre.Image);
            Assert.AreEqual("1234567890", livre.Isbn);
            Assert.AreEqual("Auteur", livre.Auteur);
            Assert.AreEqual("Collection", livre.Collection);
            Assert.AreEqual("10001", livre.IdGenre);
            Assert.AreEqual("Roman", livre.Genre);
            Assert.AreEqual("00002", livre.IdPublic);
            Assert.AreEqual("Adultes", livre.Public);
            Assert.AreEqual("LV001", livre.IdRayon);
            Assert.AreEqual("Littérature", livre.Rayon);
        }
    }
}