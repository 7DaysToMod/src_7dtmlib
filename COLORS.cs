using System;
using UnityEngine;

namespace SDTM
{
	public static class COLORS
	{
		public static string DEFAULT = "[ffffff]";
		public static string ERROR = "[f94d4d]";
		public static string MODNAME = "[ffff00]";

		public const string WHITE = "[ffffff]";
		public const string SILVER = "[C0C0C0]";
		public const string GRAY = "[808080]";
		public const string BLACK = "[000000]";
		public const string RED = "[FF0000]";
		public const string MAROON = "[800000]";
		public const string YELLOW = "[FFFF00]";
		public const string OLIVE = "[808000]";
		public const string LIME = "[00FF00]";
		public const string GREEN = "[008000]";
		public const string AQUA = "[00FFFF]";
		public const string TEAL = "[008080]";
		public const string BLUE = "[0000FF]";
		public const string NAVY = "[000080]";
		public const string MAGENTA = "[FF00FF]";
		public const string PURPLE = "[800080]";


		public static string ColorModName(string modName){
			return MODNAME + modName + DEFAULT;
		}

		public static string ColorError(string modName){
			return ERROR + modName + DEFAULT;
		}

		public static class HEX 
		{
			public const string BLACK = "000000";
			public const string BLUE = "0000FF";
			public const string GREEN = "008000";
			public const string CYAN = "00FFFF";
			public const string WHITE = "ffffff";
			public const string SILVER = "C0C0C0";
			public const string GRAY = "808080";
			public const string RED = "FF0000";
			public const string MAROON = "800000";
			public const string YELLOW = "FFFF00";
			public const string OLIVE = "808000";
			public const string LIME = "00FF00";
			public const string TEAL = "008080";
			public const string NAVY = "000080";
			public const string MAGENTA = "FF00FF";
			public const string PURPLE = "800080";
		}

		public static class COLOR {
			public static Color BLACK = COLORS.HexToColor("000000");
			public static Color BLUE = COLORS.HexToColor("0000FF");
			public static Color WHITE = COLORS.HexToColor("ffffff");
			public static Color SILVER = COLORS.HexToColor("C0C0C0");
			public static Color GRAY = COLORS.HexToColor("808080");
			public static Color RED = COLORS.HexToColor("FF0000");
			public static Color MAROON = COLORS.HexToColor("800000");
			public static Color YELLOW = COLORS.HexToColor("FFFF00");
			public static Color OLIVE = COLORS.HexToColor("808000");
			public static Color LIME = COLORS.HexToColor("00FF00");
			public static Color GREEN = COLORS.HexToColor("008000");
			public static Color AQUA = COLORS.HexToColor("00FFFF");
			public static Color TEAL = COLORS.HexToColor("008080");
			public static Color NAVY = COLORS.HexToColor("000080");
			public static Color MAGENTA = COLORS.HexToColor("FF00FF");
			public static Color PURPLE = COLORS.HexToColor("800080");
		}

		public static string ColorToHex(Color color)
		{
			return ColorUtility.ToHtmlStringRGB (color);
		}

		public static Color HexToColor(string hex)
		{
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r,g,b, 255);
		}
	}
}

