using UnityEngine;

namespace AI.FSM {
    public class JumpChargeState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.JumpCharge;
        }
        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.jumpCharge = 0;
            }
        }
        public override void OnStateStay(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.jumpCharge += battleActionFSM.character.status.jumpChargeSpeed * Time.deltaTime;
                if (battleActionFSM.jumpCharge > 1f) {
                    battleActionFSM.jumpCharge = 1f;
                }
            }
        }
    }
}