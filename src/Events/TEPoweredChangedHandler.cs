using System;

namespace SDTM
{
	public class TEPoweredChangedHandler: ITileEntityChangedListener
	{
		private bool isPowered = false;
		private int powerUsed = 0;
		private int requiredPower = 0;

		public TEPoweredChangedHandler(TileEntity _te){
			TileEntityPowered powered = _te as TileEntityPowered;
			isPowered = powered.IsPowered;
			powerUsed = powered.PowerUsed;
			requiredPower = powered.RequiredPower;
		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+_dataObject);
			TileEntityPowered powered = _te as TileEntityPowered;
			if (powered.IsPowered!= this.isPowered) {
				bool oldIsPowered = this.isPowered;
				this.isPowered = powered.IsPowered;
				//notify handlers
				API.Events.NotifyPoweredIsPoweredChangeHandlers(powered, oldIsPowered, this.isPowered);
			}

			if (powered.PowerUsed != this.powerUsed) {
				int oldPowerUsed = this.powerUsed;
				this.powerUsed = powered.PowerUsed;
				//notify handlers
				API.Events.NotifyPoweredPowerUsedChangeHandlers(powered, oldPowerUsed, this.powerUsed);
			}

			if (powered.RequiredPower != this.requiredPower) {
				int oldRequiredPower = this.requiredPower;
				this.requiredPower = powered.RequiredPower;
				//notify handlers
				API.Events.NotifyPoweredRequiredPowerChangeHandlers(powered, oldRequiredPower, this.requiredPower);
			}
		}
	}
}

