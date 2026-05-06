using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediaTekDocuments.view;

namespace MediaTekDocumentsTests
{
    [TestClass]
    public class ParutionDansAbonnementTest
    {
        [TestMethod]
        public void ParutionEntreLesDeuxDates_RetourneVrai()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2025, 6, 15);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ParutionAvantDateCommande_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2024, 12, 31);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ParutionApresDateFinAbonnement_RetourneFaux()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2026, 1, 1);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsFalse(resultat);
        }

        [TestMethod]
        public void ParutionEgaleDateCommande_RetourneVrai()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2025, 1, 1);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsTrue(resultat);
        }

        [TestMethod]
        public void ParutionEgaleDateFinAbonnement_RetourneVrai()
        {
            DateTime dateCommande = new DateTime(2025, 1, 1);
            DateTime dateFinAbonnement = new DateTime(2025, 12, 31);
            DateTime dateParution = new DateTime(2025, 12, 31);

            bool resultat = FrmMediatek.ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);

            Assert.IsTrue(resultat);
        }
    }
}