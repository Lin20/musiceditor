using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Music_Test
{
	public struct TrackMessage
	{
		public int Time;
		public ChannelMessage Message;

		public TrackMessage(int time, ChannelMessage message)
		{
			Message = message;
			Time = time;
		}
	}
}
