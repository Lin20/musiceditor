using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Sanford.Multimedia.Midi;
using Music_Test.TrackEvents;

namespace Music_Test
{
	public class Track
	{
		public Composition Composition { get; set; }

		public string Name { get; set; }
		public SoundFont SoundFont { get; set; }
		public int InstrumentIndex { get; set; }

		public PlayedNote[,] Notes { get; set; }
		public List<byte>[] NotesAt { get; set; }
		public int MidiChannel { get; set; }
		public MusicalKey Key { get; set; }

		public byte Volume { get; set; }
		public Color Color { get; set; }

		public MessageQueue Queue { get; set; }

		public const int DefaultLength = 96 * 100;
		public const int MaxNoteLength = 96 * 4;
		public int EndPoint { get; set; }

		public Track(Composition c, string name, SoundFont font, int channel, int instrument, MusicalKey key, int steps = DefaultLength)
		{
			this.Composition = c;
			this.Name = name;
			this.InstrumentIndex = instrument;
			this.SoundFont = font;
			this.MidiChannel = channel;
			this.Key = key;
			this.Volume = 127;
			this.Queue = new MessageQueue();
			this.EndPoint = steps + 1;

			new InstrumentChange(0, channel, instrument).BuildMessages(Queue);
			Notes = new PlayedNote[steps + 1, 12 * 8];
			this.NotesAt = new List<byte>[steps + 1];
		}

		public void CreateNote(int step, int length, int keyIndex, int octave)
		{
			CreateNote(step, length, Key.GetNote(keyIndex, octave));
		}

		public void CreateNote(int step, int length, MusicalNote note)
		{
			PlayedNote at = GetNote(step, note.GetMidiNote());
			if (at == null && !Options.AllowNotePlaceCuttoff)
				return;
			PlayedNote p = new PlayedNote(step, length, note, MidiChannel, Volume);
			if (at != null)
				at.Length = step - at.Step;
			Notes[step, note.GetMidiNote()] = p;
			if (NotesAt[step] == null)
				NotesAt[step] = new List<byte>();
			NotesAt[step].Add((byte)note.GetMidiNote());
		}

		public PlayedNote GetNote(int step, int note)
		{
			if (step >= EndPoint || note >= 8 * 12)
				return null;
			if (Notes[step, note] != null)
				return Notes[step, note];
			for (int i = step - 1; i > -1 && i > step - MaxNoteLength; i--)
			{
				if (Notes[i, note] != null)
				{
					if (Notes[i, note].Length + Notes[i, note].Step < step)
						return null;
					return Notes[i, note];
				}
			}
			return null;
		}

		public void PlayStep(OutputDevice device, int step)
		{
			if (step < EndPoint && NotesAt[step] != null)
			{
				foreach (byte b in NotesAt[step])
					Notes[step, b].BuildMessages(Queue);
			}

			if (Queue.Messages.Count == 0)
				return;
			for (int i = 0; i < Queue.Messages.Count; i++)
			{
				if (Queue.Messages[i].Time == step)
				{
					device.Send(Queue.Messages[i].Message);
					Queue.Messages.RemoveAt(i--);
				}
			}
		}
	}
}
