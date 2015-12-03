using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Music_Test
{
	public class Options
	{
		public static bool AllowNotePlaceCuttoff { get; set; }

		static Options()
		{
			AllowNotePlaceCuttoff = true;
		}
	}
}
