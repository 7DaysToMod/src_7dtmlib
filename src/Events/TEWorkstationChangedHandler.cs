using System;
using System.Collections.Generic;

namespace SDTM
{
	public class TEWorkstationChangedHandler:ITileEntityChangedListener
	{
//		float burnTimeLeft;
//		float totalBurnTimeLeft;
//		bool isBurning;
//
		KeyValuePair<int, int>[] tools;
		KeyValuePair<int, int>[] fuel;
		KeyValuePair<int, int>[] input;
		//KeyValuePair<int, int>[] inputMaterials;
		KeyValuePair<int, int>[] output;
		KeyValuePair<int, int>[] recipes;

		public TEWorkstationChangedHandler (TileEntityWorkstation te)
		{
			//inputMaterials = new KeyValuePair<int, int>[te.MaterialNames.Length];

			SetHistory(te);
		}

		private void SetHistory(TileEntityWorkstation te){
//			burnTimeLeft = te.BurnTimeLeft;
//			totalBurnTimeLeft = te.BurnTotalTimeLeft;
//			isBurning = te.IsBurning;
			//Log.Out ("TOOLS");
			tools = new KeyValuePair<int, int>[te.Tools.Length];
			for (var i = 0; i < te.Tools.Length; i++) {
				KeyValuePair<int, int> kvp;
				if (te.Tools [i] != null) {
					kvp = new KeyValuePair<int, int> (te.Tools [i].itemValue.type, te.Tools [i].count);
				} else {
					kvp = new KeyValuePair<int, int> (0, 0);
				}

				tools[i] = kvp;
			}

			//Log.Out ("FUEL");
			fuel = new KeyValuePair<int, int>[te.Fuel.Length];
			for (var i = 0; i < te.Fuel.Length; i++) {
				KeyValuePair<int, int> kvp;
				if(te.Fuel[i] != null){
					kvp = new KeyValuePair<int, int> (te.Fuel [i].itemValue.type, te.Fuel [i].count);
				}else {
					kvp = new KeyValuePair<int, int> (0, 0);
				}
				fuel[i] = kvp;
			}

			//Log.Out ("INPUT");
			input = new KeyValuePair<int, int>[te.Input.Length];
			for (var i = 0; i < te.Input.Length-te.MaterialNames.Length; i++) {
				KeyValuePair<int, int> kvp;
				if (te.Input [i] != null) {
					kvp = new KeyValuePair<int, int> (te.Input [i].itemValue.type, te.Input [i].count);
				} else {
					kvp = new KeyValuePair<int, int> (0, 0);
				}
				input[i] = kvp;
			}

			//Log.Out ("OUTPUT");
			output = new KeyValuePair<int, int>[te.Output.Length];
			for (var i = 0; i < te.Output.Length; i++) {
				KeyValuePair<int, int> kvp;
				if(te.Output[i] != null){
					kvp = new KeyValuePair<int, int> (te.Output [i].itemValue.type, te.Output [i].count);
				}else{
					kvp = new KeyValuePair<int, int> (0, 0);
				}
				output[i] = kvp;
			}

			//Log.Out ("QUEUE");
			recipes = new KeyValuePair<int, int>[te.Queue.Length];
			for (var i = 0; i < te.Queue.Length; i++) {
				KeyValuePair<int,int> kvp;
				if (te.Queue [i] != null && te.Queue[i].Recipe!=null) {
					kvp = new KeyValuePair<int,int> (te.Queue [i].Recipe.itemValueType, te.Queue [i].Multiplier);
				} else {
					kvp = new KeyValuePair<int,int> (0,0);
				}

				recipes[i] = kvp;
			}
		}

		public void OnTileEntityChanged (TileEntity _te, int _dataObject){
			if (_te is TileEntityWorkstation) {
				
//				TileEntityWorkstation ws = _te as TileEntityWorkstation;
//				bool isChanged = false;
//				if (ws.IsBurning != isBurning) {
//					SDTM.API.Events.NotifyWorkstationIsActiveChangedHandlers (ws, isBurning, ws.IsBurning);
//					isChanged = true;
//				}
//
//				if (ws.BurnTotalTimeLeft != totalBurnTimeLeft) {
//					SDTM.API.Events.NotifyWorkstationTotalBurnTimeChangedHandlers (ws, totalBurnTimeLeft, ws.BurnTotalTimeLeft);
//					isChanged = true;
//				}
//
//				if (ws.BurnTimeLeft != burnTimeLeft) {
//					SDTM.API.Events.NotifyWorkstationTotalBurnTimeChangedHandlers (ws, burnTimeLeft, ws.BurnTimeLeft);
//					isChanged = true;
//				}
//
//				for (int i = 0; i < ws.Tools.Length; i++) {
//					ItemStack wsTool = ws.Tools [i];
//					KeyValuePair<int, int> tool = tools [i];
//					if (tool.Key != wsTool.itemValue.type) {
//						ItemStack oldItem = new ItemStack (new ItemValue (tool.Key), tool.Value);
//						SDTM.API.Events.NotifyWorkstationToolChangedHandlers (ws, i, oldItem, wsTool);
//						isChanged = true;
//					} else {
//						if (tool.Value != wsTool.count) {
//							ItemStack oldItem = new ItemStack (new ItemValue (tool.Key), tool.Value);
//							SDTM.API.Events.NotifyWorkstationToolChangedHandlers (ws, i, oldItem, wsTool);
//							isChanged = true;
//						}
//					}
//				}
//
//				for (int i = 0; i < ws.Fuel.Length; i++) {
//					ItemStack wsFuel = ws.Fuel [i];
//					KeyValuePair<int, int> fuelItem = fuel [i];
//					if (fuelItem.Key != wsFuel.itemValue.type) {
//						ItemStack oldItem = new ItemStack (new ItemValue (fuelItem.Key), fuelItem.Value);
//						SDTM.API.Events.NotifyWorkstationFuelChangedHandlers (ws, i, oldItem, wsFuel);
//						isChanged = true;
//					} else {
//						if (fuelItem.Value != wsFuel.count) {
//							ItemStack oldItem = new ItemStack (new ItemValue (fuelItem.Key), fuelItem.Value);
//							SDTM.API.Events.NotifyWorkstationFuelChangedHandlers (ws, i, oldItem, wsFuel);
//							isChanged = true;
//						}
//					}
//				}
//
//				for (int i = 0; i < ws.Input.Length-ws.MaterialNames.Length; i++) {
//					ItemStack wsInput = ws.Input [i];
//					KeyValuePair<int, int> inputItem = input [i];
//					if (inputItem.Key != wsInput.itemValue.type) {
//						ItemStack oldItem = new ItemStack (new ItemValue (inputItem.Key), inputItem.Value);
//						SDTM.API.Events.NotifyWorkstationInputChangedHandlers (ws, i, oldItem, wsInput);
//						isChanged = true;
//					} else {
//						if (inputItem.Value != wsInput.count) {
//							ItemStack oldItem = new ItemStack (new ItemValue (inputItem.Key), inputItem.Value);
//							SDTM.API.Events.NotifyWorkstationInputChangedHandlers (ws, i, oldItem, wsInput);
//							isChanged = true;
//						}
//					}
//				}
//
//				for (int i = 0; i < ws.Output.Length; i++) {
//					ItemStack wsOutput = ws.Output [i];
//					KeyValuePair<int, int> outputItem = output [i];
//					if (outputItem.Key != wsOutput.itemValue.type) {
//						ItemStack oldItem = new ItemStack (new ItemValue (outputItem.Key), outputItem.Value);
//						SDTM.API.Events.NotifyWorkstationOutputChangedHandlers (ws, i, oldItem, wsOutput);
//						isChanged = true;
//					} else {
//						if (outputItem.Value != wsOutput.count) {
//							ItemStack oldItem = new ItemStack (new ItemValue (outputItem.Key), outputItem.Value);
//							SDTM.API.Events.NotifyWorkstationOutputChangedHandlers (ws, i, oldItem, wsOutput);
//							isChanged = true;
//						}
//					}
//				}
//
//				if (isChanged) {
//					SetHistory (ws);
//				}
			}
		}
	}
}

