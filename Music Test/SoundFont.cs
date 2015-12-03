using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Music_Test
{
	public class SoundFont
	{
		public string Name { get; set; }
		public string[] InstrumentNames { get; set; }

		public SoundFont(string name)
		{
			this.Name = name;
			InstrumentNames = new string[128];

			try
			{
				StreamReader sr = new StreamReader(File.OpenRead("./Data/" + name + ".txt"));
				for (int i = 0; i < 128 && !sr.EndOfStream; i++)
					InstrumentNames[i] = sr.ReadLine();
				sr.Close();
			}
			catch (Exception)
			{

			}
		}
	}
}
