using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;

namespace Music_Test
{
	public enum ErrorTypes
	{
		None=0,
		NoDeviceFound=1,
		ErrorLoadingDevice=2
	}

	public class Player
	{
		public OutputDevice Device { get; private set; }
		public int DeviceID { get; private set; }
		public OutputDeviceDialog OutDialog { get; set; }

		public ErrorTypes LastError { get; private set; }

		public Player(int device_id = 0)
		{
			LastError = LoadDevice(device_id);
		}

		public ErrorTypes LoadDevice(int device_id = 0)
		{
			if (OutputDevice.DeviceCount == 0)
				return ErrorTypes.NoDeviceFound;

			try
			{
				Device = new OutputDevice(device_id);
				DeviceID = device_id;
			}
			catch (Exception)
			{
				return ErrorTypes.ErrorLoadingDevice;
			}

			return ErrorTypes.None;
		}

		public string GetLastErrorMessage()
		{
			switch (LastError)
			{
				case ErrorTypes.NoDeviceFound:
					return "No MIDI output device available.";

				case ErrorTypes.ErrorLoadingDevice:
					return "Error loading device.";

				default:
					return "Unknown error " + LastError.ToString() + ".";
			}
		}
	}
}
