using ChatLearning.DataAccess;
using ChatLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ChatLearning.Controllers
{
    /// <summary>
    /// Page d'accueil
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Chargement de la vue Index
        /// </summary>
        /// <returns>La vue Index</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Permet de récupérer la réponse associé à la phrase de l'utilisateur
        /// </summary>
        /// <param name="sentence">La phrase de l'utilisateur</param>
        /// <returns>La réponse associé à la phrase de l'utilisateur</returns>
        [HttpPost]
        public string GetAnswer(string sentence)
        {
            Sentence set = new Sentence(sentence);

            if (DBContext.CheckIfSentenceIsAlreadyLearned(set))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(DBContext.GetAnswer(set).ANSCONTAINS);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Permrt d'associer une réponse à une phrase de l'utilisateur
        /// </summary>
        /// <param name="sentence">La phrase de l'utilisateur</param>
        /// <param name="answer">La réponse associé à la phrase de l'utilisateur</param>
        /// <returns>Confirmation de l'association entre la phrase et la réponse</returns>
        [HttpPost]
        public string AddAnswer(string sentence, string answer)
        {
            DBContext.CreateSentenceAnswerLink(sentence, answer);

            return Newtonsoft.Json.JsonConvert.SerializeObject("J'ai appris que à la phrase : << " + sentence + " >>, je devais répondre : << " + answer + " >>");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetAnswersTags(string text)
        {
             List<string> listOfAnswersTags = DBContext.GetListOfAnswers(text);

            return Newtonsoft.Json.JsonConvert.SerializeObject(listOfAnswersTags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetListOfPlugins()
        {
            List<string> listOfPlugins = DBContext.GetListOfPlugins();

            return Newtonsoft.Json.JsonConvert.SerializeObject(listOfPlugins);
        }
    }
}