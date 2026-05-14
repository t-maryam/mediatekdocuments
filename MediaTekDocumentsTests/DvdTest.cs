using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class DvdTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Dvd dvd = new Dvd("20001", "Titre DVD", "image.jpg", 120, "Réalisateur", "Synopsis",
                "10002", "Science Fiction", "00003", "Tous publics", "DF001", "DVD films");

            Assert.AreEqual("20001", dvd.Id);
            Assert.AreEqual("Titre DVD", dvd.Titre);
            Assert.AreEqual("image.jpg", dvd.Image);
            Assert.AreEqual(120, dvd.Duree);
            Assert.AreEqual("Réalisateur", dvd.Realisateur);
            Assert.AreEqual("Synopsis", dvd.Synopsis);
            Assert.AreEqual("10002", dvd.IdGenre);
            Assert.AreEqual("Science Fiction", dvd.Genre);
            Assert.AreEqual("00003", dvd.IdPublic);
            Assert.AreEqual("Tous publics", dvd.Public);
            Assert.AreEqual("DF001", dvd.IdRayon);
            Assert.AreEqual("DVD films", dvd.Rayon);
        }
    }
}