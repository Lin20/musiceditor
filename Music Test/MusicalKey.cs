using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music_Test
{
	public enum StepLengths
	{
		None = 0,
		Half = 1,
		Whole = 2
	}

	public class MusicalKey
	{
		public string Name { get; set; }
		public StepLengths[] Scale { get; set; }
		public MusicalNote BaseNote { get; set; }
		public bool UseFlats { get; set; }

		public MusicalKey(string name)
			: this(name, null, new MusicalNote(NoteLetters.C, NoteStatus.Natural))
		{
		}

		public MusicalKey(string name, StepLengths[] scale, MusicalNote baseNote, bool useFlats = false)
		{
			this.Name = name;
			this.BaseNote = baseNote;
			if (scale == null)
				this.Scale = MajorScale(); //C Major
			else
				this.Scale = scale;
			this.UseFlats = useFlats;
		}

		public static StepLengths[] MajorScale()
		{
			StepLengths[] steps = new StepLengths[7];
			for (int i = 0; i < 7; i++)
				steps[i] = StepLengths.Whole;
			steps[2] = StepLengths.Half;
			steps[6] = StepLengths.Half;
			return steps;
		}

		public MusicalNote GetNote(int step, int octave = 0)
		{
			int note = BaseNote.GetLetterID();
			for (int i = 0; i < step; i++)
			{
				note += (int)Scale[i % Scale.Length];
			}
			return new MusicalNote(note % 12, octave + note / 12, UseFlats);
		}
	}
}
