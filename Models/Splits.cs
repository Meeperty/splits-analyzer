using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SplitsAnalyzer.Models
{
    internal class Splits
    {
        public string gameName;
        public string categoryName;

        public int attemptCount;

        internal Splits(string path)
        {
            //in case the file is invalid
            gameName = "Not Found";
            categoryName = "Not Found";
            attemptCount = -1;
            try
            {
                //load and analyze document if possible
                XmlDocument document = new();
                document.Load(path);

                XmlNode gameNameNode = document.SelectSingleNode("//GameName");
                if (gameNameNode != null)
                {
                    gameName = gameNameNode.InnerText;
                }

                XmlNode categoryNameNode = document.SelectSingleNode("//CategoryName");
                if (categoryNameNode != null)
                {
                    categoryName = categoryNameNode.InnerText;
                }

                XmlNode attemptCountNode = document.SelectSingleNode("//AttemptCount");
                if (attemptCountNode != null)
                {
                    int.TryParse(attemptCountNode.InnerText, out attemptCount);
                }
            }
            catch
            {
                
            }
        }
    }

    internal class Split
    {
        string name;
        
    }
    
    internal class Attempt
    {
        public int id;
        public DateTime startTime, endTime;
        public TimeSpan totalTime;
    }
}
