using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ReactiveUI;
using System.ComponentModel;
using SplitsAnalyzer.ViewModels;
using System.Xml.Serialization;
using System.IO;

namespace SplitsAnalyzer.Models
{
    internal class Splits : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public string gameName;
        public string categoryName;

        public int attemptCount;

        public List<Segment> segmentList;

        public string lastError = "";

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

                XmlSerializer serializer = new XmlSerializer(typeof(Run));

                TimeSpan test = TimeSpan.Parse("00:00:00");

                var run = (Run)serializer.Deserialize(new FileStream(path, FileMode.Open));
                if (run is null)
                    Error("Run was null somehow idk");

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
