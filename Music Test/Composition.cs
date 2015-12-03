using System;
using System.Collections.Generic;
//using System.Timers;
using System.Text;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Timers;

namespace Music_Test
{
	public class Composition
	{
		public const int QuarterNote = 24;

		public MidiInternalClock Timer { get; set; }
		//public Timer Timer { get; set; }
		public int Step { get; set; }
		//public MessageQueue Messages { get; set; }
		public int MessageQueueStep { get; set; }

		public bool Playing { get; private set; }
		public bool Loop { get; set; }
		public OutputDevice Device { get; set; }

		public List<Track> Tracks { get; set; }
		public int BeatNote { get; set; }
		public int BeatsPerMeasure { get; set; }
		public int Tempo { get; set; }

		public Composition(OutputDevice device, int tempo)
		{
			Device = device;
			this.Tempo = tempo;
			this.BeatNote = 1;
			this.BeatsPerMeasure = 4;
			Timer = new MidiInternalClock();
			Timer.Tempo = (int)(120f / (float)tempo * 120f * 4000f);
			Timer.Tick += new EventHandler(Step_Changed);

			//Messages = new MessageQueue();
			Tracks = new List<Track>();
		}

		public void BeginPlayback(int fromStep = 0, bool loop = false)
		{
			MessageQueueStep = fromStep;
			/*for (int i = 0; i < Messages.Messages.Count; i++)
			{
				if (Messages.Messages[i].Time >= fromStep)
				{
					MessageQueueStep = i;
					break;
				}
			}
			if (MessageQueueStep == -1)
			{
				StopPlayback();
				return;
			}*/

			Playing = true;
			Loop = loop;
			Step = fromStep - 1;
			Step_Changed(this, null);
			Timer.Start();
		}

		public void StopPlayback()
		{
			Playing = false;
			Timer.Stop();
			foreach (Track t in Tracks)
			{
				for (int i = 0; i < 128; i++)
					Device.Send(new ChannelMessage(ChannelCommand.NoteOff, t.MidiChannel, i));
			}
		}

		public void Step_Changed(object sender, EventArgs e)
		{
			Step++;
			foreach (Track t in Tracks)
			{
				t.PlayStep(Device, Step);
			}
			/*if (MessageQueueStep >= Messages.Messages.Count)
			{
				if (Loop)
				{
					BeginPlayback(0, true);
					return;
				}
				StopPlayback();
				return;
			}

			while(Messages.Messages[MessageQueueStep].Time == Step)
			{
				try
				{
					Device.Send(Messages.Messages[MessageQueueStep++].Message);
					if (MessageQueueStep >= Messages.Messages.Count)
					{
						if (Loop)
						{
							BeginPlayback(0, true);
							return;
						}
						StopPlayback();
						return;
					}
				}
				catch (Exception)
				{
					StopPlayback();
				}
			}*/
		}
	}
}
