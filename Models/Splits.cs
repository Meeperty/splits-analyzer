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

        public List<Segment> segmentList;

        internal Splits(string path)
        {
            //in case the file is invalid
            gameName = "Not Found";
            categoryName = "Not Found";
            attemptCount = -1;
            segmentList = new();
            try
            {
#pragma warning disable CS8600, CS8602
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
                    attemptCount = int.Parse(attemptCountNode.InnerText);
                }

                XmlNodeList segmentNodeList = document.SelectNodes("//Segment");
                if (segmentNodeList != null)
                {
                    for (int i = 0; i < segmentNodeList.Count; i++)
                    {
                        XmlNode currentSegmentNode = segmentNodeList.Item(i);
                        
                        //get segment name
                        Segment currentSegment = new("Error, Name wasn't found");
                        string name = currentSegmentNode["Name"].InnerText;
                        if (name != null)
                        {
                            currentSegment = new("Name");
                        }

                        //get segment history
                        currentSegment.segmentHistory = new();
                        XmlNode segmentHistoryNode = currentSegmentNode["SegmentHistory"];
                        XmlNodeList segmentTimesNodes = segmentHistoryNode.SelectNodes("Time");
                        if (segmentTimesNodes.Count > 0)
                        {
                            for (int j = 0; j < segmentTimesNodes.Count; j++)
                            {
                                XmlNode currentTimeNode = segmentTimesNodes[j];
                                
                                int attemptId = int.Parse(currentTimeNode.Attributes["id"].Value);

                                TimeSpan gameTime = new();
                                TimeSpan realTime = new();

                                XmlNode? gameTimeNode = currentTimeNode["GameTime"];
                                XmlNode? realTimeNode = currentTimeNode["RealTime"];

                                if (gameTimeNode != null)
                                {
                                    gameTime = TimeSpan.Parse(gameTimeNode.InnerText);
                                    currentSegment.segmentHistory.Add(attemptId, gameTime);
                                }
                                else
                                {
                                    if (realTimeNode != null)
                                    {
                                        realTime = TimeSpan.Parse(realTimeNode.InnerText);
                                        currentSegment.segmentHistory.Add(attemptId, realTime);
                                    }
                                    else
                                    {
                                        
                                    }
                                }
                            }
                        }

                        segmentList.Add(currentSegment);
                    }
                }
#pragma warning restore CS8600, CS8602
            }
            catch (NullReferenceException)
            {
                
            }
        }
    }
    
    internal class Segment
    {
        public string name;
        public Dictionary<int, TimeSpan> segmentHistory;
        
        internal Segment(string name)
        {
            this.name = name;
            segmentHistory = new();
        }
    }
    
    internal class Attempt
    {
        public int id;
        public DateTime startTime, endTime;
        public TimeSpan totalTime;
    }
}
