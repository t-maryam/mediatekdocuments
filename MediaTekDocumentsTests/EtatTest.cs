using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class EtatTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Etat etat = new Etat("00001", "neuf");

            Assert.AreEqual("00001", etat.Id);
            Assert.AreEqual("neuf", etat.Libelle);
        }
    }
}
