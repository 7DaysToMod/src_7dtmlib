using System;

namespace SDTM
{
	public class TEPowerSourceChangedHandler: TEPoweredChangedHandler
	{
		//private bool isOn = false;
		//private ushort currentFuel = 0;
		//private int powerUsed = 0;
		//private ushort maxOutput = 0;

		public TEPowerSourceChangedHandler(TileEntity _te):base(_te){
//			TileEntityPowerSource powerSource = _te as TileEntityPowerSource;
//			isOn = powerSource.IsOn;
//			currentFuel = powerSource.CurrentFuel;
//			powerUsed = powerSource.PowerUsed;
//			maxOutput = powerSource.MaxOutput;

		}

		public new void OnTileEntityChanged (TileEntity _te, int _dataObject){
//			base.OnTileEntityChanged(_te, _dataObject);
//			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+_dataObject);
//			TileEntityPowerSource powerSource = _te as TileEntityPowerSource;
//			if (powerSource.IsOn != this.isOn) {
//				bool oldIsOn = this.isOn;
//				this.isOn = powerSource.IsOn;
//
//				if (powerSource.IsOn) {
//					API.Events.NotifyPowerSourceTurnedOnHandlers (powerSource);
//				} else {
//					API.Events.NotifyPowerSourceTurnedOffHandlers (powerSource);
//				}
//			}
//
//			if (powerSource.CurrentFuel != this.currentFuel) {
//				int oldAmount = this.currentFuel;
//				this.currentFuel = powerSource.CurrentFuel;
//				API.Events.NotifyPowerSourceCurrentFuelChangedHandlers (powerSource, oldAmount, this.currentFuel);
//			}
//
//			if (powerSource.MaxOutput != this.maxOutput) {
//				int oldMaxOutput = this.maxOutput;
//				this.maxOutput = powerSource.MaxOutput;
//				API.Events.NotifyPowerSourceMaxPowerChangedHandlers (powerSource, oldMaxOutput, this.maxOutput);
//			}
//
//			if (powerSource.PowerUsed != this.powerUsed) {
//				int oldPowerUsed = this.powerUsed;
//				this.powerUsed = powerSource.PowerUsed;
//				Log.Out ("POWER USED AMOUNT CHANGED: " + oldPowerUsed.ToString()+" => "+this.powerUsed.ToString());
//			}
		}
	}
}

