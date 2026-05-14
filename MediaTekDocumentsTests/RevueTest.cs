using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class RevueTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Revue revue = new Revue("10001", "Titre Revue", "image.jpg", "10016", "Presse Culturelle",
                "00002", "Adultes", "PR002", "Magazines", "MS", 52);

            Assert.AreEqual("10001", revue.Id);
            Assert.AreEqual("Titre Revue", revue.Titre);
            Assert.AreEqual("image.jpg", revue.Image);
            Assert.AreEqual("10016", revue.IdGenre);
            Assert.AreEqual("Presse Culturelle", revue.Genre);
            Assert.AreEqual("00002", revue.IdPublic);
            Assert.AreEqual("Adultes", revue.Public);
            Assert.AreEqual("PR002", revue.IdRayon);
            Assert.AreEqual("Magazines", revue.Rayon);
            Assert.AreEqual("MS", revue.Periodicite);
            Assert.AreEqual(52, revue.DelaiMiseADispo);
        }
    }
}