using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music_Test
{
	public enum NoteStatus
	{
		Natural = 0,
		Flat = 1,
		Sharp = 2
	}

	public enum NoteLetters
	{
		C = 0,
		D = 2,
		E = 4,
		F = 5,
		G = 7,
		A = 9,
		B = 11
	}

	public struct MusicalNote
	{
		public NoteLetters Letter;
		public int Octave;
		public NoteStatus Status;
		public const int NotesPerOctave = 12;

		public MusicalNote(NoteLetters letter, NoteStatus status)
		{
			this.Letter = letter;
			this.Octave = 0;
			this.Status = status;
		}

		public MusicalNote(NoteLetters letter, int octave, NoteStatus status)
		{
			this.Letter = letter;
			this.Octave = octave;
			this.Status = status;
		}

		public MusicalNote(int letterID, int octave = 0, bool useFlat = false)
		{
			this.Octave = octave + letterID / 12;
			letterID %= 12;
			this.Status = NoteStatus.Natural;
			switch (letterID)
			{
				case 0:
					Letter = NoteLetters.C;
					break;
				case 1:
					if (!useFlat)
					{
						Letter = NoteLetters.C;
						Status = NoteStatus.Sharp;
					}
					else
					{
						Letter = NoteLetters.D;
						Status = NoteStatus.Flat;
					}
					break;
				case 2:
					Letter = NoteLetters.D;
					break;
				case 3:
					if (!useFlat)
					{
						Letter = NoteLetters.D;
						Status = NoteStatus.Sharp;
					}
					else
					{
						Letter = NoteLetters.E;
						Status = NoteStatus.Flat;
					}
					break;
				case 4:
					Letter = NoteLetters.E;
					break;
				case 5:
					Letter = NoteLetters.F;
					break;
				case 6:
					if (!useFlat)
					{
						Letter = NoteLetters.F;
						Status = NoteStatus.Sharp;
					}
					else
					{
						Letter = NoteLetters.G;
						Status = NoteStatus.Flat;
					}
					break;
				case 7:
					Letter = NoteLetters.G;
					break;
				case 8:
					if (!useFlat)
					{
						Letter = NoteLetters.G;
						Status = NoteStatus.Sharp;
					}
					else
					{
						Letter = NoteLetters.A;
						Status = NoteStatus.Flat;
					}
					break;
				case 9:
					Letter = NoteLetters.A;
					break;
				case 10:
					if (!useFlat)
					{
						Letter = NoteLetters.A;
						Status = NoteStatus.Sharp;
					}
					else
					{
						Letter = NoteLetters.B;
						Status = NoteStatus.Flat;
					}
					break;
				case 11:
					Letter = NoteLetters.B;
					break;
				default:
					Letter = NoteLetters.C;
					break;
			}
		}

		public int GetMidiNote()
		{
			int id = GetLetterID();
			return Octave * NotesPerOctave + GetLetterID();
		}

		public int GetLetterID()
		{
			return ((int)Letter + (Status == NoteStatus.Sharp ? 1 : Status == NoteStatus.Flat ? NotesPerOctave - 1 : 0)) % NotesPerOctave;
		}

		public string GetLetterName()
		{
			if (Status == NoteStatus.Natural)
				return Letter.ToString();
			if (Status == NoteStatus.Sharp)
			{
				if (Letter == NoteLetters.E)
					return NoteLetters.F.ToString();
				if (Letter == NoteLetters.B)
					return NoteLetters.C.ToString();
				return Letter.ToString() + "#";
			}
			if (Letter == NoteLetters.F)
				return NoteLetters.E.ToString();
			if (Letter == NoteLetters.C)
				return NoteLetters.B.ToString();
			return Letter.ToString() + "b";
		}

		public MusicalNote SetOctave(int octave)
		{
			this.Octave = octave;
			return this;
		}
	}
}
