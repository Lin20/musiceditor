using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace Music_Test
{
	public interface IDeviceController
	{
		OutputDevice Device { get; set; }
	}
}
