using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Music_Test;
using Music_Test.TrackEvents;

namespace Music_Test.UI
{
	public enum MouseActions
	{
		None = 0,
		Resize = 1,
		Move = 2
	}

	public partial class TrackEditor : UserControl
	{
		private const int HeaderSize = 20;

		private Track _track;
		public Track Track { get { return _track; } set { _track = value; Invalidate(); } }

		private double _zoom = 1;
		[Browsable(true), DefaultValue(1)]
		public double Zoom { get { return _zoom; } set { _zoom = value; Invalidate(); } }

		private int _verticalScroll;
		public new int VerticalScroll { get { return _verticalScroll; } set { _verticalScroll = value; Invalidate(); } }

		private int _step;
		public int Step { get { return _step; } set { _step = value; Invalidate(); } }

		private Color _noteBackgroundColor = Color.FromArgb(24, 48, 64);
		public Color NoteBackgroundColor { get { return _noteBackgroundColor; } set { _noteBackgroundColor = value; Invalidate(); } }
		private Color _noteBackgroundHighlightColor = Color.FromArgb(24, 48, 96);
		public Color NoteBackgroundHighlightColor { get { return _noteBackgroundHighlightColor; } set { _noteBackgroundHighlightColor = value; Invalidate(); } }

		private Font numberFont = new Font("Arial", 8, FontStyle.Regular);

		public int HoverIndexStep { get; private set; }
		public int HoverIndexNote { get; private set; }
		public bool Playing { get; set; }

		private List<PlayedNote> SelectedNotes { get; set; }
		private int _lastMouseX;
		private int _lastMouseY;
		private bool _ctrl;
		private bool _alt;

		public MouseActions MouseAction { get; set; }

		public TrackEditor()
		{
			InitializeComponent();
			this.MouseWheel += new MouseEventHandler(TrackEditor_MouseWheel);
			HoverIndexStep = -1;
			HoverIndexNote = -1;
			SelectedNotes = new List<PlayedNote>();
		}

		private void TrackEditor_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				int bpm = 4;
				int beatNote = Composition.QuarterNote;
				MusicalKey key;
				if (_track != null)
				{
					bpm = _track.Composition.BeatsPerMeasure;
					key = _track.Key;
				}
				else
				{
					key = new MusicalKey("C Major");
				}

				e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, HeaderSize, this.Height));

				int scaleHeight = (int)(_zoom * 32f);
				float scaleWidth = (float)(_zoom * 2f);
				int vScroll = (this.Height - HeaderSize * 2) / (int)(scaleHeight);
				int deltaY = (int)(_verticalScroll % scaleHeight);

				e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, this.Height - HeaderSize, this.Width, HeaderSize));
				e.Graphics.DrawLine(Pens.Gray, 0, this.Height - HeaderSize, this.Width, this.Height - HeaderSize);

				int hScroll = (int)((float)(this.Width - HeaderSize) / scaleWidth);
				Pen thick = new Pen(Color.Gray, 3);
				Pen thin = new Pen(Color.Gray);
				int realStep = Math.Max(0, -hScroll / 2 + _step);
				int spm = Composition.QuarterNote * Composition.QuarterNote / beatNote * bpm;
				for (int i = realStep; i <= hScroll + realStep; i++)
				{
					int x = (i - realStep) * (int)scaleWidth + HeaderSize + 2;
					int measure = (i / (Composition.QuarterNote * Composition.QuarterNote / beatNote * bpm));
					if ((i % spm) == 0) //One
					{
						e.Graphics.DrawLine(thick, x, 0, x, this.Height - HeaderSize);
						e.Graphics.DrawString(measure.ToString(), numberFont, Brushes.Maroon, x - 2, this.Height - HeaderSize + 4);
					}
					else if ((i % (spm / 2)) == 0) //Half
					{
						thin.DashPattern = new float[] { 100000, 1 };
						e.Graphics.DrawLine(thin, x, HeaderSize, x, this.Height - HeaderSize);
						e.Graphics.DrawString((measure + .5f).ToString(), numberFont, Brushes.Black, x - 2, this.Height - HeaderSize + 4);
					}
					else if ((i % (spm / 4)) == 0) //Quarter
					{
						thin.DashPattern = new float[] { 1, 1 };
						e.Graphics.DrawLine(thin, x, HeaderSize, x, this.Height - HeaderSize);
					}
					else if ((i % (spm / 8)) == 0) //Eighth
					{
						thin.DashPattern = new float[] { 10, 22 };
						e.Graphics.DrawLine(thin, x, HeaderSize - 28 + deltaY, x, this.Height - HeaderSize);
					}
					else if ((i % (spm / 16)) == 0) //Sixteenth
					{
						thin.DashPattern = new float[] { 3, 29 };
						e.Graphics.DrawLine(thin, x, HeaderSize - 25 + deltaY, x, this.Height - HeaderSize);
					}
				}

				e.Graphics.SetClip(new Rectangle(0, HeaderSize, this.Width, this.Height - HeaderSize * 2));
				if (_track != null)
				{
					PlayedNote p;
					for (int i = 0; i < 8 * 12; i++)
					{
						p = _track.GetNote(realStep, i);
						if (p != null)
							DrawNote(p, e.Graphics, realStep, scaleWidth, scaleHeight, deltaY, vScroll, key);
					}
					//Draw the notes
					for (int noteIndex = realStep + 1; noteIndex <= realStep + hScroll && noteIndex < _track.NotesAt.Length; noteIndex++)
					{
						if (_track.NotesAt[noteIndex] == null || _track.NotesAt[noteIndex].Count == 0)
							continue;
						foreach (byte b in _track.NotesAt[noteIndex])
						{
							p = _track.Notes[noteIndex, b];
							if (p == null)
								continue;

							DrawNote(p, e.Graphics, realStep, scaleWidth, scaleHeight, deltaY, vScroll, key);
						}
					}
				}

				for (int i = 0; i <= vScroll + 1; i++)
				{
					int y = this.Height - i * scaleHeight + deltaY - HeaderSize;
					e.Graphics.DrawLine(Pens.Gray, new Point(0, y), new Point(this.Width - 1, y));
					int index = i + _verticalScroll / scaleHeight;

					MusicalNote m = key.GetNote(index);
					Color c = KeyDisplay.HsvToRgb(360f / 12f * (float)m.GetLetterID(), .8f, .4f);
					e.Graphics.DrawRectangle(new Pen(c), 1, y - scaleHeight + 1, HeaderSize - 1, scaleHeight - 2);
					c = KeyDisplay.HsvToRgb(360f / 12f * (float)m.GetLetterID(), .8f, .8f);
					e.Graphics.FillRectangle(new SolidBrush(c), 2, y - scaleHeight + 2, HeaderSize - 2, scaleHeight - 3);
					string s = m.GetLetterName();
					e.Graphics.DrawString(s, Font, Brushes.Black, HeaderSize / 2 - e.Graphics.MeasureString(s, Font).Width / 2, y - scaleHeight / 2 - Font.Size - 2);
					e.Graphics.DrawString(m.Octave.ToString(), numberFont, Brushes.Black, HeaderSize / 2 - 5, y - scaleHeight / 2 - Font.Size + 14);
				}

				e.Graphics.ResetClip();

				e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, this.Width, HeaderSize));
				e.Graphics.DrawLine(Pens.Gray, 0, HeaderSize - 1, this.Width, HeaderSize - 1);
				e.Graphics.DrawRectangle(Pens.Gray, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
				e.Graphics.DrawLine(Pens.Gray, HeaderSize, 0, HeaderSize, this.Height);

				int lineX = HeaderSize + Math.Max(0, Math.Min((int)(_step * scaleWidth), (this.Width - HeaderSize) / 2)) + 2;
				e.Graphics.DrawLine(Pens.White, lineX, 0, lineX, this.Height);
			}
			catch (Exception)
			{

			}
		}

		private void DrawNote(PlayedNote p, Graphics g, int realStep, float scaleWidth, int scaleHeight, int deltaY, int vScroll, MusicalKey key)
		{
			float s = (float)(p.Step - realStep);
			int x = (int)(s * (float)scaleWidth) + HeaderSize + 3;

			float i = GetKeyIndex(p.Note.GetMidiNote(), key);

			int start = _verticalScroll / scaleHeight;
			int end = start + vScroll;
			if (i < start - 1 || i > end)
				return;
			int y = this.Height - (int)((i - start + 1) * (float)scaleHeight) + deltaY - HeaderSize;
			int width = (int)((float)p.Length * scaleWidth);

			bool highlighted = (Playing && Math.Max(0, _step) >= p.Step && Math.Max(0, _step) < p.Step + p.Length) || (HoverIndexNote == p.Note.GetMidiNote() && HoverIndexStep == p.Step) || SelectedNotes.Contains(p);
			Color c = (highlighted ? Color.White : KeyDisplay.HsvToRgb(360f / 12f * (float)p.Note.GetLetterID(), .8f, .4f));
			g.DrawRectangle(new Pen(c), x, y + 1, width - 2, scaleHeight - 2);
			if (highlighted)
				c = KeyDisplay.HsvToRgb(360f / 12f * (float)p.Note.GetLetterID(), 1f, .8);
			else
				c = KeyDisplay.HsvToRgb(360f / 12f * (float)p.Note.GetLetterID(), .8f, .8f);
			g.FillRectangle(new SolidBrush(c), x + 1, y + 2, width - 3, scaleHeight - 3);
			g.DrawString(p.Note.Octave.ToString(), Font, Brushes.Black, x + width / 2 - g.MeasureString(p.Note.Octave.ToString(), Font).Width / 2 - 1, y + scaleHeight / 2 - 10);
		}

		private int GetNoteY(PlayedNote p, int scaleHeight, int deltaY, int vScroll, MusicalKey key)
		{
			float i = GetKeyIndex(p.Note.GetMidiNote(), key);

			int start = _verticalScroll / scaleHeight;
			int end = start + vScroll;
			return this.Height - (int)((i - start + 1) * (float)scaleHeight) + deltaY - HeaderSize;
		}

		private void TrackEditor_MouseWheel(object sender, MouseEventArgs e)
		{
			VerticalScroll += (int)(32f * _zoom) * (e.Delta < 0 ? -1 : 1);
			if (VerticalScroll < 0)
				VerticalScroll = 0;
		}

		private void TrackEditor_MouseEnter(object sender, EventArgs e)
		{
			this.Focus();
		}

		private float GetKeyIndex(int midi, MusicalKey key)
		{
			midi -= (int)key.BaseNote.GetLetterID();
			int octave = midi / 12;
			int note = midi % 12;
			int total = 0;
			for (int i = 0; i < key.Scale.Length; i++)
			{
				if (total == note)
					return i + octave * key.Scale.Length;
				total += (int)key.Scale[i];
				if (total > note)
				{
					return i + octave * key.Scale.Length + (float)(1f / (float)key.Scale[i]);
				}
			}
			return midi;
		}

		private void TrackEditor_MouseMove(object sender, MouseEventArgs e)
		{
			if (_track == null || Playing)
				return;

			if (e.Button == System.Windows.Forms.MouseButtons.None)
			{
				HandleHover(e);
				return;
			}

			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				int snap = _alt ? 1 : 6;
				int deltaX = (int)((e.X - _lastMouseX) / (_zoom * 2f));
				int deltaY = (int)((e.Y - _lastMouseY) / (_zoom * 2f));
				switch (MouseAction)
				{
					case MouseActions.Resize:
						if (deltaX != 0)
						{
							foreach (PlayedNote p in SelectedNotes)
							{
								p.Length = Math.Max(4, Math.Min(Track.MaxNoteLength, p.OriginalLength + deltaX) / snap * snap);
							}
							this.Invalidate();
						}
						break;
					case MouseActions.Move:
						
						this.Invalidate();
						break;
				}
			}
		}

		private void HandleHover(MouseEventArgs e)
		{
			float scaleWidth = (float)(_zoom * 2f);
			int scaleHeight = (int)(_zoom * 32f);
			int step = (int)(Math.Max(0, e.X + Math.Max(0, _step - (this.Width - HeaderSize) / 2) - HeaderSize - 1) / scaleWidth);
			float keyIndex = (float)((this.Height - e.Y - HeaderSize + 1) + _verticalScroll) / (float)scaleHeight;
			int midi = _track.Key.GetNote((int)keyIndex).GetMidiNote();
			PlayedNote at = _track.GetNote(step, midi);
			if (at == null)
			{
				int vScroll = (this.Height - HeaderSize * 2) / (int)(scaleHeight);
				int deltaY = (int)(_verticalScroll % scaleHeight);
				for (int i = midi - 4; i <= midi + 4; i++)
				{
					at = _track.GetNote(step, i);
					if (at == null)
						continue;
					int y = GetNoteY(at, scaleHeight, deltaY, vScroll, _track.Key);
					if (e.Y >= y && e.Y <= y + scaleHeight)
						break;
					at = null;
				}
			}
			if (at == null)
			{
				HoverIndexStep = HoverIndexNote = -1;
				this.Cursor = Cursors.Arrow;
				MouseAction = MouseActions.None;
			}
			else
			{
				HoverIndexStep = at.Step;
				HoverIndexNote = at.Note.GetMidiNote();
				if (step - at.Step < 4)
				{
					this.Cursor = Cursors.VSplit;
					MouseAction = MouseActions.Resize;
				}
				else if (at.Step + at.Length - step < 4)
				{
					this.Cursor = Cursors.VSplit;
					MouseAction = MouseActions.Resize;
				}
				else
				{
					this.Cursor = Cursors.Arrow;
					MouseAction = MouseActions.Move;
				}
			}
			this.Invalidate();
		}

		private void TrackEditor_MouseDown(object sender, MouseEventArgs e)
		{
			if (Playing)
				return;
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (HoverIndexNote != -1)
				{
					PlayedNote p = _track.Notes[HoverIndexStep, HoverIndexNote];
					if (p != null && !SelectedNotes.Contains(p))
					{
						p.OriginalLength = p.Length;
						SelectedNotes.Add(p);
						this.Invalidate();
					}
					_lastMouseX = e.X;
					_lastMouseY = e.Y;
				}
			}
		}

		private void TrackEditor_MouseUp(object sender, MouseEventArgs e)
		{
			if (Playing)
				return;
			foreach (PlayedNote p in SelectedNotes)
			{
				p.OriginalLength = p.Length;
			}
			if (_ctrl)
				return;
			SelectedNotes.Clear();
			this.Invalidate();
		}

		private void TrackEditor_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control || e.KeyCode == Keys.Control)
				_ctrl = true;
			if (e.Alt || e.KeyCode == Keys.Alt)
				_alt = true;
		}

		private void TrackEditor_KeyUp(object sender, KeyEventArgs e)
		{
			if (!e.Control && e.KeyCode != Keys.Control)
				_ctrl = false;
			if (!e.Alt && e.KeyCode != Keys.Alt)
				_alt = false;
		}
	}
}
