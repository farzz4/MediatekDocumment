using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande : contient les informations d'une commande
    /// </summary>
    public class Commande
    {
        public string Id { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }

        public Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}
