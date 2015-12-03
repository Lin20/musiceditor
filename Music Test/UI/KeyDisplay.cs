using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Music_Test.UI
{
	public partial class KeyDisplay : UserControl
	{
		private MusicalKey _key;
		[Browsable(true)]
		public MusicalKey Key { get { return _key; } set { _key = value; this.Invalidate(); } }

		public KeyDisplay()
		{
			InitializeComponent();
		}

		private void KeyDisplay_Paint(object sender, PaintEventArgs e)
		{
			if (Key == null)
				return;
			float splitX = (float)(this.Width - 20) / (float)Key.Scale.Length;
			float splitY = (float)(this.Height - 20f) / (float)MusicalNote.NotesPerOctave;
			float splitH = 360f / 12f;
			e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, (int)(12f * splitY + 1f), this.Width, 21));
			e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, 20, this.Height));
			e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, this.Width - 1, this.Height - 1));

			for (int i = 0; i < Key.Scale.Length; i++)
			{
				e.Graphics.DrawLine(Pens.Gray, new Point((int)Math.Ceiling((float)i * splitX) + 20, 0), new Point((int)Math.Ceiling((float)i * splitX) + 20, this.Height));
			}
			for (int i = 0; i <= MusicalNote.NotesPerOctave; i++)
			{
				e.Graphics.DrawLine(Pens.Gray, new Point(20, (int)((float)i * splitY)), new Point(this.Width, (int)((float)i * splitY)));
			}
			
			int step = 0;
			for (int k = 0; k < Key.Scale.Length; k++)
			{
				MusicalNote m = Key.GetNote(k);
				string note = m.GetLetterName();
				step += (int)Key.Scale[k];
				int x = (int)Math.Ceiling((float)k * splitX) + 21;
				int width = (int)Math.Ceiling((float)(k + 1) * splitX) + 20 - x;
				int y = (int)((float)(12 - step) * splitY) + 1;
				int height = (int)((float)(12 - (step - (int)Key.Scale[k])) * splitY) - y;
				e.Graphics.DrawString(note, Font, Brushes.Black, new Point(x + width / 2 - (int)e.Graphics.MeasureString(note, Font).Width / 2, this.Height - 19));

				e.Graphics.FillRectangle(new SolidBrush(HsvToRgb(splitH * (float)m.GetLetterID(), .8f, .8f)), new Rectangle(x, y, width, height));
			}

			e.Graphics.RotateTransform(-90f);
			e.Graphics.DrawString(Key.Name, this.Font, Brushes.Black, this.Height / -2 -(e.Graphics.MeasureString(Key.Name, Font).Width / 2f), -1);
			e.Graphics.ResetTransform();


		}

		public static Color HsvToRgb(double h, double S, double V)
		{
			// ######################################################################
			// T. Nathan Mundhenk
			// mundhenk@usc.edu
			// C/C++ Macro HSV to RGB

			int r, g, b;
			double H = h;
			while (H < 0) { H += 360; };
			while (H >= 360) { H -= 360; };
			double R, G, B;
			if (V <= 0)
			{ R = G = B = 0; }
			else if (S <= 0)
			{
				R = G = B = V;
			}
			else
			{
				double hf = H / 60.0;
				int i = (int)Math.Floor(hf);
				double f = hf - i;
				double pv = V * (1 - S);
				double qv = V * (1 - S * f);
				double tv = V * (1 - S * (1 - f));
				switch (i)
				{

					// Red is the dominant color

					case 0:
						R = V;
						G = tv;
						B = pv;
						break;

					// Green is the dominant color

					case 1:
						R = qv;
						G = V;
						B = pv;
						break;
					case 2:
						R = pv;
						G = V;
						B = tv;
						break;

					// Blue is the dominant color

					case 3:
						R = pv;
						G = qv;
						B = V;
						break;
					case 4:
						R = tv;
						G = pv;
						B = V;
						break;

					// Red is the dominant color

					case 5:
						R = V;
						G = pv;
						B = qv;
						break;

					// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

					case 6:
						R = V;
						G = tv;
						B = pv;
						break;
					case -1:
						R = V;
						G = pv;
						B = qv;
						break;

					// The color is not defined, we should throw an error.

					default:
						//LFATAL("i Value error in Pixel conversion, Value is %d", i);
						R = G = B = V; // Just pretend its black/white
						break;
				}
			}
			r = Clamp((int)(R * 255.0));
			g = Clamp((int)(G * 255.0));
			b = Clamp((int)(B * 255.0));
			return Color.FromArgb(r, g, b);
		}

		/// <summary>
		/// Clamp a value to 0-255
		/// </summary>
		public static int Clamp(int i)
		{
			if (i < 0) return 0;
			if (i > 255) return 255;
			return i;
		}
	}
}
