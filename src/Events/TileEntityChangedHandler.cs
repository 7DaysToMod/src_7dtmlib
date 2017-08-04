using System;

namespace SDTM
{
	public class TileEntityChangedHandler: ITileEntityChangedListener
	{
		
		public TileEntityChangedHandler(TileEntity _te){

		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+_dataObject);
		}
	}
}

