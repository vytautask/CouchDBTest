using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectModel
{
	public static class TypeResolver
	{
		public static OMBase ResolveType(string typeName)
		{
			OMBase result = null;

			if (!string.IsNullOrEmpty(typeName))
			{
				switch(typeName)
				{
					case "DV_TEXT":
						result = new DvText();
						break;

					case "DV_COUNT":
						result = new DvCount();
						break;
				}

			}

			return result;
		}

	}
}
