using System;
using System.Collections.Generic;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Music_Test.TrackEvents
{
	public class PlayedNote : TrackEvent
	{
		public int Step { get; private set; }
		public int Length { get; set; }
		public MusicalNote Note { get; set; }
		public int Channel { get; private set; }
		public byte Volume { get; private set; }

		public int OriginalLength { get; set; }

		public PlayedNote(int step, int length, MusicalNote note, int channel, byte volume)
		{
			this.Step = step;
			this.Length = length;
			this.Note = note;
			this.Channel = channel;
			this.Volume = volume;
		}

		public void BuildMessages(MessageQueue queue)
		{
			queue.AddMessage(new TrackMessage(Step, new ChannelMessage(ChannelCommand.NoteOn, Channel, Note.GetMidiNote(), Volume)));
			queue.AddMessage(new TrackMessage(Step + Length, new ChannelMessage(ChannelCommand.NoteOff, Channel, Note.GetMidiNote(), Volume)));
		}
	}
}
