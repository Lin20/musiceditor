using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music_Test
{
	public class Generator
	{
		public void Generate(Track t)
		{
			Random rnd = new Random();
			GenerateSection(0, ref rnd, t);
		}

		public void GenerateSection(int step, ref Random rnd, Track t)
		{
			int start = rnd.Next(7) + 6;

			t.CreateNote(step, 12 * 4, start, 2);
			step += 12 * 4;
			int next = rnd.Next(1);
			switch (next)
			{
				case 0:
					t.CreateNote(step, 12 * 4, start - 2, 2);
					step += 12 * 4;
					next = rnd.Next(2);
					if (t.Key.GetNote(start - 3).Status == NoteStatus.Sharp)
						t.CreateNote(step, 12 * 4, start - 3, 2);
					else
						t.CreateNote(step, 12 * 4, start - 1, 2);
					break;
				case 1:
					t.CreateNote(step, 12 * 4, start - 2, 2);
					break;
				case 2:
					t.CreateNote(step, 12 * 4, start + 2, 2);
					break;
				case 3:
					t.CreateNote(step, 12 * 4, start, 2);
					break;
			}
		}
	}
}
