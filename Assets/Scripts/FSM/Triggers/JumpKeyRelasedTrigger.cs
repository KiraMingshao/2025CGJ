using UnityEngine;

namespace AI.FSM {
    public class JumpKeyReleasedTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM) {
                return !Input.GetButton("Jump");
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.JumpKeyReleased;
        }
    }
}