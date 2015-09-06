using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Asteroids.GameScreens;
using Microsoft.Xna.Framework.Content;

namespace Asteroids.Helpers
{
    public class HighScore
    {

        /// <summary>
        /// Highscore strcture
        /// </summary>
        [Serializable]
        public struct HighScoreData
        {
            public string[] playerName;
            public int[] score;

            public int Count;

            //Constructor
            public HighScoreData(int count)
            {
                playerName = new string[count];
                score = new int[count];

                Count = count;
            }

        }

        /// <summary>
        /// Method to save highscore to a file
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        public static void SaveHighScore(HighScoreData data, string filename)
        {
            //Get path of the scores file
            string fullPath = Path.Combine(GameConstants.GAME_ROOT_DIRECTORY, filename);

            //Open file. Create if necessary
            FileStream stream = File.Open(fullPath, FileMode.Create);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                //Close the file
                stream.Close();
            }
        }

        public static HighScoreData LoadHighScore(string filename)
        {
            HighScoreData data;

            //Get the path of saved score data
            string fullPath = Path.Combine(GameConstants.GAME_ROOT_DIRECTORY, filename);

            //Open the file
            FileStream stream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                //Close the file
                stream.Close();
            }

            return (data);
        }

    }
}
