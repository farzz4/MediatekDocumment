using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe utilitaire pour les calculs de dates
    /// </summary>
    public class GestionAbonnement
    {
        /// <summary>
        /// Vérifie si une date de parution est comprise entre deux dates
        /// </summary>
        public static bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateParution >= dateCommande && dateParution <= dateFinAbonnement;
        }
    }
}
