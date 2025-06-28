using UnityEngine;

namespace AI.FSM {
    public class ImbalanceFullTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterLifeFSM lifeFSM) {
                return Mathf.Abs(lifeFSM.character.status.imbalance) >= lifeFSM.character.status.maxImbalance;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.ImbalanceFull;
        }
    }
}