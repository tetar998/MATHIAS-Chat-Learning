using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatLearning.Models
{
    /// <summary>
    /// Contient les fonctions nécessaires pour une phrase
    /// </summary>
    public class Sentence
    {
        #region Getter and Setter

        /// <summary>
        /// Id de la phrase
        /// </summary>
        public int SETSYSID { get; set; }

        /// <summary>
        /// Contenu de la phrase
        /// </summary>
        public string SETCONTAINS { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Permet d'intialiser une phrase vide
        /// </summary>
        public Sentence()
        {
            SETSYSID = 0;
            SETCONTAINS = null;
        }

        /// <summary>
        /// Permet d'intialiser une phrase avec un contenu
        /// </summary>
        /// <param name="sentence">Contenu de la phrase</param>
        public Sentence(string sentence)
        {
            SETSYSID = 0;
            SETCONTAINS = sentence;
        }

        #endregion
    }
}