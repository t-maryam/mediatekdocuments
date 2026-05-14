using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class HeritagesCategorieTest
    {
        [TestMethod]
        public void Genre_Constructeur_InitialiseToutesLesProprietes()
        {
            Genre genre = new Genre("10001", "Roman");
            Assert.AreEqual("10001", genre.Id);
            Assert.AreEqual("Roman", genre.Libelle);
        }

        [TestMethod]
        public void Public_Constructeur_InitialiseToutesLesProprietes()
        {
            Public lePublic = new Public("00002", "Adultes");
            Assert.AreEqual("00002", lePublic.Id);
            Assert.AreEqual("Adultes", lePublic.Libelle);
        }

        [TestMethod]
        public void Rayon_Constructeur_InitialiseToutesLesProprietes()
        {
            Rayon rayon = new Rayon("LV001", "Littérature étrangère");
            Assert.AreEqual("LV001", rayon.Id);
            Assert.AreEqual("Littérature étrangère", rayon.Libelle);
        }

        [TestMethod]
        public void Suivi_Constructeur_InitialiseToutesLesProprietes()
        {
            Suivi suivi = new Suivi("00001", "en cours");
            Assert.AreEqual("00001", suivi.Id);
            Assert.AreEqual("en cours", suivi.Libelle);
        }
    }
}