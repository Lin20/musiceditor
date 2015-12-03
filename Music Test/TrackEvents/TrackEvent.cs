using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Music_Test.TrackEvents
{
	public interface TrackEvent
	{
		void BuildMessages(MessageQueue queue);
	}
}
