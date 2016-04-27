using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatLearning.Models
{
    /// <summary>
    /// Contient les fonctions nécessaires pour une réponse
    /// </summary>
    public class Answer
    {
        #region Getter and Setter

        /// <summary>
        /// Id de la réponse
        /// </summary>
        public int ANSSYSID { get; set; }

        /// <summary>
        /// Chaine d'identification de la réponse
        /// </summary>
        public string ANSSYSEXTID { get; set; }

        /// <summary>
        /// Contenu de la réponse
        /// </summary>
        public string ANSCONTAINS { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Permet d'initialiser une réponse vide
        /// </summary>
        public Answer()
        {
            ANSSYSID = 0;
            ANSSYSEXTID = null;
            ANSCONTAINS = null;
        }

        /// <summary>
        /// Permet d'intialiser une réponse avec un contenu
        /// </summary>
        /// <param name="answer">Contenu de la réponse</param>
        public Answer(string answertextId, string answer)
        {
            ANSSYSID = 0;
            ANSSYSEXTID = answertextId;
            ANSCONTAINS = answer;
        }

        #endregion
    }
}