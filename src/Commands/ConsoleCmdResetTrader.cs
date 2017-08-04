using System;
using System.Collections.Generic;

namespace SDTM
{
	public class ConsoleCmdResetTrader:ConsoleCmdAbstract
	{
		public override string GetDescription ()
		{
			return "Resets a Traders Trade s List";
		}

		public override string GetHelp ()
		{
			return "Usage:\n" +
				"/resettrader <entityId>";
		}

		public override string[] GetCommands ()
		{
			return new string[] { "resettrader", "rst"};
		}

		public override void Execute (System.Collections.Generic.List<string> _params, CommandSenderInfo _senderInfo)
		{
			if (_params.Count != 1) {
				SdtdConsole.Instance.Output ("[ResetTrader] Invalid Parameter Length");
				return;
			}

			int entityId = -1;
			int.TryParse (_params [0], out entityId);
			if (entityId > -1) {
				Entity ent = GameManager.Instance.World.GetEntity (entityId);
				if (ent is EntityNPC) {
					EntityNPC trader = ent as EntityNPC;
					TileEntityTrader te = trader.TileEntityTrader;

					if (te != null) {
						Log.Out (te.TraderData.AvailableMoney+" dukes");
						te.TraderData.PrimaryInventory = new List<ItemStack> ();
						te.TraderData.TierItemGroups = new List<ItemStack[]> ();

						te.SetModified ();
					}
				}
			}
		}
	}
}

