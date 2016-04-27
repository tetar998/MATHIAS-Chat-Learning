using ChatLearning.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Collections.Generic;

namespace ChatLearning.DataAccess
{
    /// <summary>
    /// Contient toutes les fonctions necessitants un accès à la base de donnée
    /// </summary>
    public static class DBContext
    {
        /// <summary>
        /// Chaine de connexion à la base de donnée
        /// </summary>
        private static SqlConnection dbConnection;

        /// <summary>
        /// permet d'otebnie la chaine d'identification d'un texte
        /// </summary>
        /// <param name="text">Le texte</param>
        /// <returns>La chaine d'identificatuibn du texte</returns>
        private static string GetIdentifyText(string text)
        {
            char[] listOfUselesscharacters = new char[] { ' ', '²', '&', '"', '(', '-', '_', '~', '#', '^', '$', '*', 'µ', '%', '!', ':', ';', ',', '+', '?', '.' };
            string identifyText = "";

            foreach (string word in text.Split(' '))
            {
                identifyText += word.Trim(listOfUselesscharacters).ToLower();
            }

            return identifyText;
        }

        #region SENTENCE

        /// <summary>
        /// Permet de vérifier si la phrase est connue
        /// </summary>
        /// <param name="sentence">La phrase à vérfier</param>
        /// <returns>Oui ou Non</returns>
        public static bool CheckIfSentenceIsAlreadyLearned(Sentence sentence)
        {
            int nbrSentence = 0;

            sentence.SETCONTAINS = GetIdentifyText(sentence.SETCONTAINS);

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                nbrSentence = dbConnection.Query<int>("select count(SETSYSID) from SENTENCE where SETCONTAINS = @SETCONTAINS", sentence).Single();
            }

            if (nbrSentence == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Permet d'obtenir une phrase
        /// </summary>
        /// <param name="id">Id de la phrase</param>
        /// <returns>Une phrase</returns>
        public static Sentence GetSentence(int id)
        {
            Sentence sentence = new Sentence();

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                sentence = dbConnection.Query<Sentence>("select SETSYSID, SETCONTAINS from SENTENCE where SETSYSID = @ID", new { ID = id }).Single();
            }

            return sentence;
        }

        /// <summary>
        /// Permet de créer une phrase
        /// </summary>
        /// <param name="sentence">Phrase devant être créé</param>
        /// <returns>La phrase créé</returns>
        public static Sentence CreateSentence(Sentence sentence)
        {
            int id = 0;

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                id = dbConnection.Query<int>("insert into SENTENCE (SETCONTAINS) values(@SETCONTAINS); select CAST(SCOPE_IDENTITY() as int)", sentence).Single();
            }

            return GetSentence(id);
        }

        #endregion

        #region ANSWER

        /// <summary>
        /// Permet d'obtenir une réponse en fonction de la phrase d'un utilisateur
        /// </summary>
        /// <param name="sentence">La phrase d'un utilisateur</param>
        /// <returns>LA réponse à la phrase d'un utilisateur</returns>
        public static Answer GetAnswer(Sentence sentence)
        {
            Answer answer = new Answer();

            sentence.SETCONTAINS = GetIdentifyText(sentence.SETCONTAINS);

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                answer = dbConnection.Query<Answer>("select ANSSYSID, ANSSYSEXTID, ANSCONTAINS from ANSWER where ANSSYSID = (select AERANSSYSID from ANSWER_ENTITY_ROLE where AERSETSYSID = (select SETSYSID from SENTENCE where SETCONTAINS = @SETCONTAINS))", sentence).Single();
            }

            return answer;
        }

        /// <summary>
        /// Permet d'obtenir une réponse
        /// </summary>
        /// <param name="id">Id de la réponse</param>
        /// <returns>Une réponse</returns>
        public static Answer GetAnswer(int id)
        {
            Answer answer = new Answer();

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                answer = dbConnection.Query<Answer>("select ANSSYSID, ANSSYSEXTID, ANSCONTAINS from ANSWER where ANSSYSID = @ID", new { ID = id }).Single();
            }

            return answer;
        }

        /// <summary>
        /// Permet d'otenir une réponse
        /// </summary>
        /// <param name="answer">La réponse</param>
        /// <returns>La réponse</returns>
        public static Answer GetAnswer(Answer answer)
        {
            Answer ans = new Answer();

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                ans = dbConnection.Query<Answer>("select ANSSYSID, ANSSYSEXTID ,ANSCONTAINS from ANSWER where ANSSYSEXTID = @ANSSYSEXTID", answer).Single();
            }

            return ans;
        }

        /// <summary>
        /// Permet de créer une réponse
        /// </summary>
        /// <param name="answer">Réponse devant être créé</param>
        /// <returns>La réponse créé</returns>
        public static Answer CreateAnswer(Answer answer)
        {
            int id = 0;

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                id = dbConnection.Query<int>("insert into ANSWER (ANSSYSEXTID, ANSCONTAINS) values(@ANSSYSEXTID, @ANSCONTAINS); select CAST(SCOPE_IDENTITY() as int)", answer).Single();
            }

            return GetAnswer(id);
        }

        /// <summary>
        /// Permet de vérifier si la réponse éxiste déja
        /// </summary>
        /// <param name="answer">La réponse à vérifier</param>
        /// <returns>Oui ou Non</returns>
        public static bool CheckIfAnswerIsAlreadyLearned(string answer)
        {
            int nbrAnswer = 0;

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                nbrAnswer = dbConnection.Query<int>("select count(ANSSYSID) from ANSWER where ANSSYSEXTID = @ANSWER", new { ANSWER = GetIdentifyText(answer) }).Single();
            }

            if (nbrAnswer == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Permet d'obtenir la liste des réponses ressemblant au texte passé en paramètre
        /// </summary>
        /// <param name="text">Le texte servant à retrouver la liste des réponses</param>
        /// <returns>La liste des réponses</returns>
        public static List<string> GetListOfAnswers(string text)
        {
            List<string> listOfAnswers = new List<string>();

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                listOfAnswers = dbConnection.Query<string>("select ANSCONTAINS from ANSWER where ANSCONTAINS like @TEXT", new { TEXT = '%' + text + '%' }).ToList();
            }

            return listOfAnswers;
        }

        #endregion

        #region ANSWER_ENTITY_ROLE

        /// <summary>
        /// Permet de lier une phrase à une réponse
        /// </summary>
        /// <param name="sentence">Une phrase</param>
        /// <param name="answer">Une réponse</param>
        public static void CreateSentenceAnswerLink(string sentence, string answer)
        {
            Sentence set = CreateSentence(new Sentence(GetIdentifyText(sentence)));
            Answer ans = new Answer(GetIdentifyText(answer), answer);

            if (CheckIfAnswerIsAlreadyLearned(answer))
            {
                ans = GetAnswer(ans);
            }
            else
            {
                ans = CreateAnswer(new Answer(GetIdentifyText(answer), answer));
            }

            using (dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                dbConnection.Execute("insert into ANSWER_ENTITY_ROLE (AERANSSYSID, AERSETSYSID) values (@ANSSYSID, @SETSYSID)", new { ANSSYSID = ans.ANSSYSID, SETSYSID = set.SETSYSID });
            }
        }

        #endregion
    }
}