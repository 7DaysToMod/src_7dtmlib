using System;

namespace SDTM
{
	public class TEPoweredTriggerChangedHandler: TEPoweredChangedHandler
	{
		bool isTriggered = false;
		private byte property1 = 0;
		private byte property2 = 0;
		private bool targetSelf = false;
		private bool targetAllies = false;
		private bool targetStrangers = false;
		private bool targetZombies = false;

		public TEPoweredTriggerChangedHandler(TileEntity _te):base(_te){
			TileEntityPoweredTrigger trigger = _te as TileEntityPoweredTrigger;

			this.isTriggered = trigger.IsTriggered;
			this.property1 = trigger.Property1;
			this.property2 = trigger.Property2;
			this.targetSelf = trigger.TargetSelf;
			this.targetAllies = trigger.TargetAllies;
			this.targetStrangers = trigger.TargetStrangers;
			this.targetZombies = trigger.TargetZombies;
		}

		public new void OnTileEntityChanged (TileEntity _te, int _dataObject){
			base.OnTileEntityChanged(_te, _dataObject);

			Log.Out (_te.GetTileEntityType ().ToString ()+" - "+_dataObject);
			TileEntityPoweredTrigger trigger = _te as TileEntityPoweredTrigger;

			if (trigger.IsTriggered != this.isTriggered) {
				bool oldIsTriggered = this.isTriggered;
				this.isTriggered = trigger.IsTriggered;
				API.Events.NotifyPoweredTriggerIsTriggeredHandlers (trigger, oldIsTriggered, this.isTriggered);
			}

			if (trigger.Property1 != this.property1) {
				byte oldProperty1 = this.property1;
				this.property1 = trigger.Property1;
				API.Events.NotifyPoweredTriggerProperty1ChangeHandlers (trigger, oldProperty1, this.property1);
			}

			if (trigger.Property2 != this.property2) {
				byte oldProperty2 = this.property2;
				this.property2 = trigger.Property2;
				API.Events.NotifyPoweredTriggerProperty2ChangeHandlers (trigger, oldProperty2, this.property2);
			}

			if (trigger.TargetSelf != this.targetSelf) {
				bool oldTargetSelf = this.targetSelf;
				this.targetSelf = trigger.TargetSelf;
				API.Events.NotifyPoweredTriggerTargetSelfChangeHandlers (trigger, oldTargetSelf, this.targetSelf);
			}

			if (trigger.TargetAllies != this.targetAllies) {
				bool oldTargetAllies = this.targetAllies;
				this.targetAllies = trigger.TargetAllies;
				API.Events.NotifyPoweredTriggerTargetAlliesChangeHandlers (trigger, oldTargetAllies, this.targetAllies);
			}

			if (trigger.TargetStrangers != this.targetStrangers) {
				bool oldTargetStrangers = this.targetStrangers;
				this.targetStrangers = trigger.TargetStrangers;
				API.Events.NotifyPoweredTriggerTargetStrangersChangeHandlers (trigger, oldTargetStrangers, this.targetStrangers);
			}

			if (trigger.TargetZombies != this.targetZombies) {
				bool oldTargetZombies = this.targetZombies;
				this.targetZombies = trigger.TargetZombies;
				API.Events.NotifyPoweredTriggerTargetZombieChangeHandlers (trigger, oldTargetZombies, this.targetZombies);
			}
		}
	}
}

