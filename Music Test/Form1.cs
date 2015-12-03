using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace Music_Test
{
	public partial class Form1 : Form
	{
		private Player _player;
		private OutputDeviceDialog _dialog = new OutputDeviceDialog();

		private Composition _composition;
		private SoundFont _soundFont;
		private KeyLoader _keyLoader;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_player = new Player();


			if (_player.LastError != ErrorTypes.None)
			{
				MessageBox.Show(_player.GetLastErrorMessage(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else
			{
				//MessageBox.Show("Loaded device " + OutputDevice.GetDeviceCapabilities(0).name);
			}

			_keyLoader = new KeyLoader();
			_soundFont = new SoundFont("General MIDI");

			Random rnd = new Random();
			_composition = new Composition(_player.Device, 120);
			try
			{
				int size = 1000;
				_composition.Tracks.Add(new Track(_composition, "Track1", _soundFont, 0, 0, _keyLoader.Keys[rnd.Next(2)][rnd.Next(12)], size));
				keyDisplay1.Key = _composition.Tracks[0].Key;
				int lastLength = 0;
				int octave = 4;
				new Generator().Generate(_composition.Tracks[0]);
			}
			catch (Exception)
			{

			}

			trackEditor1.Track = _composition.Tracks[0];
			//_composition.BeginPlayback(0, true);
			//tmrLine.Interval = Math.Max(60000 / _composition.Tempo / 96, 1);
			//tmrLine.Start();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_player.Device != null)
			{
				try
				{
					_player.Device.Close();
				}
				catch (Exception) { }
			}
			if (_composition != null)
			{
				try
				{
					_composition.Timer.Stop();
				}
				catch (Exception) { }
			}
		}

		int k = 0;
		private void keyDisplay1_Click(object sender, EventArgs e)
		{
			k++;
			k %= 24;
			_composition.Tracks[0].Key = _keyLoader.Keys[k / 12][k % 12];
			keyDisplay1.Key = _composition.Tracks[0].Key;
			trackEditor1.Invalidate();
		}

		private void tmrLine_Tick(object sender, EventArgs e)
		{
			trackEditor1.Step = _composition.Step - _composition.Tempo / 12;
			trackEditor1.Invalidate();
		}

		private void trackEditor1_Click(object sender, EventArgs e)
		{
			/*_composition.BeginPlayback(0, true);
			trackEditor1.Playing = true;
			tmrLine.Interval = Math.Max(60000 / _composition.Tempo / 96, 1);
			tmrLine.Start();*/
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				SwitchPlayingState();
			}
			if (e.KeyCode == Keys.Home)
			{
				_composition.Step = 0;
				trackEditor1.Step = 0;
				trackEditor1.Invalidate();
			}
		}

		private void SwitchPlayingState()
		{
			if (!_composition.Playing)
			{
				_composition.BeginPlayback(0, true);
				trackEditor1.Playing = true;
				tmrLine.Interval = Math.Max(60000 / _composition.Tempo / 96, 1);
				tmrLine.Start();
			}
			else
			{
				_composition.StopPlayback();
				trackEditor1.Playing = false;
				tmrLine.Stop();
			}
		}
	}
}
