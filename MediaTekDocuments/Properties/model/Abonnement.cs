using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement hérite de Commande :
    /// contient les informations d'un abonnement à une revue
    /// </summary>
    public class Abonnement : Commande
    {
        public DateTime DateFinAbonnement { get; }
        public string IdRevue { get; }
        public string Titre { get; }

        public Abonnement(string id, DateTime dateCommande, double montant,
            DateTime dateFinAbonnement, string idRevue, string titre = "")
            : base(id, dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
            Titre = titre;
        }
    }
}