namespace AI.FSM {
    public class HealthZeroTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterLifeFSM lifeFSM) {
                return lifeFSM.character.status.health <= 0;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.HealthZero;
        }
    }
}