using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.model;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class UtilisateurTest
    {
        [TestMethod]
        public void Constructeur_InitialiseToutesLesProprietes()
        {
            Utilisateur utilisateur = new Utilisateur("00001", "admin", "00004", "Administrateur");

            Assert.AreEqual("00001", utilisateur.id);
            Assert.AreEqual("admin", utilisateur.login);
            Assert.AreEqual("00004", utilisateur.idService);
            Assert.AreEqual("Administrateur", utilisateur.service);
        }
    }
}