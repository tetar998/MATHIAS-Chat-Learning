using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatLearning.Models
{
    public class Plugin
    {
        #region Getter and Setter

        /// <summary>
        /// Id du Plugin
        /// </summary>
        public int PLUSYSID { get; set; }

        /// <summary>
        /// Nom du Plugin
        /// </summary>
        public string PLUNAME { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Permet d'initialiser un plugin vide
        /// </summary>
        public Plugin()
        {
            PLUSYSID = 0;
            PLUNAME = null;
        }

        /// <summary>
        /// Permet d'intialiser un plugin avec un contenu
        /// </summary>
        /// <param name="plugin">Nom du plugin</param>
        public Plugin(string plugin)
        {
            PLUSYSID = 0;
            PLUNAME = plugin;
        }

        #endregion
    }
}