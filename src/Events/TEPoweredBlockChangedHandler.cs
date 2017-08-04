using System;

namespace SDTM
{
	public class TEPoweredBlockChangedHandler: ITileEntityChangedListener
	{
		private bool isToggled = false;
		private bool isPowered = false;
		private int powerUsed = 0;
		private int requiredPower = 0;

		public TEPoweredBlockChangedHandler(TileEntity _te){
			TileEntityPoweredBlock poweredBlock = _te as TileEntityPoweredBlock;
			this.isToggled = poweredBlock.IsToggled;
			this.isPowered = poweredBlock.IsPowered;
			this.powerUsed = poweredBlock.PowerUsed;
			this.requiredPower = poweredBlock.RequiredPower;
		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+_dataObject);
			TileEntityPoweredBlock poweredBlock = _te as TileEntityPoweredBlock;

			if (poweredBlock.PowerUsed != this.powerUsed) {
				this.powerUsed = poweredBlock.PowerUsed;
				Log.Out ("Power Block Used:" + this.powerUsed.ToString());
			}

			if (poweredBlock.RequiredPower != this.requiredPower) {
				this.requiredPower = poweredBlock.RequiredPower;
				Log.Out ("Power Block Required:" + this.requiredPower.ToString());
			}

			if (poweredBlock.IsToggled != this.isToggled) {
				this.isToggled = poweredBlock.IsToggled;
				Log.Out ("Power Block Toggled:" + this.isToggled.ToString());
			}

			if (poweredBlock.IsPowered != this.isPowered) {
				this.isPowered = poweredBlock.IsPowered;
				Log.Out ("Power Block Powered:" + this.isPowered.ToString());
			}
		}
	}
}

