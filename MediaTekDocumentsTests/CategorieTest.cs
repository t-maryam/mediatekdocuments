using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class CategorieTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Categorie categorie = new Categorie("10001", "Roman");

            Assert.AreEqual("10001", categorie.Id);
            Assert.AreEqual("Roman", categorie.Libelle);
        }

        [TestMethod]
        public void ToString_RetourneLibelle()
        {
            Categorie categorie = new Categorie("10001", "Roman");

            Assert.AreEqual("Roman", categorie.ToString());
        }
    }
}