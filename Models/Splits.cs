using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ReactiveUI;
using System.ComponentModel;
using SplitsAnalyzer.ViewModels;

namespace SplitsAnalyzer.Models
{
    internal class Splits : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string gameName;
        public string categoryName;

        public int attemptCount;

        public List<Segment> segmentList;

        public string lastError;

        private MainWindowViewModel errorNotificationObj;

        internal Splits(string path, MainWindowViewModel changeEventObj)
        {
            //in case the file is invalid
            gameName = "Not Found";
            categoryName = "Not Found";
            attemptCount = -1;
            segmentList = new();
            
            errorNotificationObj = changeEventObj;
            
            //parse XML
            try
            {
                XmlDocument document = new();
                
#pragma warning disable CS8600, CS8602
                //load document if possible
                try
                {
                    document.Load(path);
                    //if this is executed, document.Load(path) didn't cause an error
                    ResetError();
                }
                catch (System.Exception e)
                {
                    if (e.Message != null)
                    {
                        Error(e.Message);
                    }
                }
                

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

                XmlNodeList attemptNodeList = document.SelectNodes("//Attempt");
                if (attemptNodeList != null)
                {
                    
                }
#pragma warning restore CS8600, CS8602
            }
            catch (NullReferenceException)
            {
                Error($"File {path} was missing a critical element, or there was a code error");
            }

            //analyze data
            
        }

        public void Error(string error)
        {
            lastError = error;
            errorNotificationObj.UpdateSplitsError();
        }

        public void ResetError()
        {
            lastError = "";
            errorNotificationObj.UpdateSplitsError();
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
