using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SplitsAnalyzer
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public class Run
	{
		/*
		 * GameIcon *dont care
		 * GameName 
		 * CategoryName
		 * LayoutPath *dont care
		 * Metadata
		 * Offset
		 * AttemptCount
		 * AttemptHistory
		 * Segments
		 * AutoSplitterSettings *dont care
		 */
		[XmlElement]
		public string GameName { get; set; }

		[XmlElement]
		public string CategoryName { get; set; }

		public Metadata Metadata { get; set; }

		[XmlIgnore]
		public TimeSpan Offset { get { return TimeSpan.Parse(OffsetString); } }

		[XmlElement("Offset")]
		private string OffsetString { get; set; }

		[XmlElement]
		public int AttemptCount { get; set; }

		[XmlElement]
		public List<Attempt> AttemptHistory { get; set; }

		[XmlElement]
		public List<Segment> Segments { get; set; }
	}

	public class Metadata
	{
		Platform Platform;
	}

	public class Platform
	{
		[XmlAttribute]
		bool usesEmulator;
		[XmlText]
		string Value;
	}

	public class Attempt
	{

	}

	public class Segment
	{

	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
