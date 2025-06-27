namespace AI.FSM {
    public class EnergyLowTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterEnergyFSM energyFSM) {
                return energyFSM.character.status.energy < energyFSM.character.status.lowEnergyBoundary;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.EnergyLow;
        }
    }
}