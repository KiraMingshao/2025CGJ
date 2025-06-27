namespace AI.FSM {
    public class EnergyFullTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterEnergyFSM energyFSM) {
                return energyFSM.character.status.energy == energyFSM.character.status.maxEnergy;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.EnergyFull;
        }
    }
}