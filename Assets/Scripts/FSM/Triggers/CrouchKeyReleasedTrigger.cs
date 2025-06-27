using UnityEngine;

namespace AI.FSM {
    public class CrouchKeyReleasedTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM) {
                return !Input.GetButton("Crouch");
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.CrouchKeyReleased;
        }
    }
}