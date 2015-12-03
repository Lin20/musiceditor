using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Music_Test
{
	public class KeyLoader
	{
		public MusicalKey[][] Keys { get; set; }

		public KeyLoader()
		{
			List<MusicalKey[]> list = new List<MusicalKey[]>();
			try
			{
				StreamReader sr = new StreamReader(File.OpenRead("./Data/Keys.txt"));
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					MusicalKey[] keys = new MusicalKey[12];
					for (int i = 0; i < 12; i++)
					{
						keys[i] = CreateKey(line, i);
					}
					list.Add(keys);
				}
				sr.Close();
			}
			catch (Exception)
			{

			}

			Keys = list.ToArray();
		}

		public MusicalKey CreateKey(string line, int n)
		{
			try
			{
				string[] parts = line.Split(':');
				bool flat = parts[1][0] == 'F';
				MusicalNote note = new MusicalNote(n, 0, flat);
				string name = parts[0].Replace("N", note.GetLetterName());
				StepLengths[] steps = new StepLengths[parts[2].Length];
				for (int i = 0; i < parts[2].Length; i++)
				{
					char c = parts[2][i].ToString().ToUpper()[0];
					steps[i] = (c == 'H' ? StepLengths.Half : c == 'W' ? StepLengths.Whole : c == 'F' ? StepLengths.Half | StepLengths.Whole : StepLengths.None);
				}
				return new MusicalKey(name, steps, note, parts[1][0] == 'F');
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
