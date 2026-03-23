using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmAuthentification
    /// </summary>
    class FrmAuthentificationController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmAuthentificationController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Retourne l'utilisateur associé au login et password
        /// </summary>
        /// <param name="login">login de l'utilisateur cherché</param>
        /// <param name="password">password de l'utilisateur cherché</param>
        /// <returns>Objet utilisateur</returns>
        public Utilisateur GetUtilisateur(string login, string password)
        {
            Utilisateur utilisateur = access.GetUtilisateur(login, password);
            if (utilisateur != null && utilisateur.Password.Equals(password))
            {
                return utilisateur;
            }
            return null;
        }
    }
}
