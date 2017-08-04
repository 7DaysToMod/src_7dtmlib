using System;
using UnityEngine;

namespace SDTM
{
	public static class CommandUtils
	{
		public static class MSG{
			public static string DENIED = "Permission Denied";
		}

		public enum PositionPart{
			ALL=0, X=1, Y=2, Z=3, XZ=4
		}

		public static string parseRelativePosition(EntityPlayer player, PositionPart positionPart, string _position){
			if (_position.IndexOf("@p")>-1) {
				string offsetString = _position.Replace ("@p", "");

				int offset;
				string parsedPosition="";

				if (_position != "") {
					if (int.TryParse (offsetString, out offset)) {
						Vector3 playerPos = player.position;
						if (positionPart == PositionPart.ALL) {
							parsedPosition = playerPos.x.ToString() + " " + playerPos.y.ToString () + " " + playerPos.z.ToString ();
						}else if (positionPart == PositionPart.X) {
							parsedPosition = (playerPos.x + offset).ToString();
						}else if (positionPart == PositionPart.Y) {
							parsedPosition = (playerPos.y + offset).ToString();
						}else if (positionPart == PositionPart.Z) {
							parsedPosition = (playerPos.z + offset).ToString();
						}

						return parsedPosition;

					} else {
						return _position;
					}
				}
			}

			return _position;
		}
	}
}

