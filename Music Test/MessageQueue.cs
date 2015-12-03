using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Music_Test
{
	public class MessageQueue
	{
		public List<TrackMessage> Messages { get; set; }

		public MessageQueue()
		{
			Messages = new List<TrackMessage>();
		}

		public void AddMessage(TrackMessage m)
		{
			for (int i = 0; i <= Messages.Count; i++)
			{
				if (i == Messages.Count || Messages[i].Time > m.Time)
				{
					Messages.Insert(i, m);
					break;
				}
			}
		}
	}
}
