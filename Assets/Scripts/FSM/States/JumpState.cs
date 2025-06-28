using UnityEngine;

namespace AI.FSM {
    public class JumpState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Jump;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.rb.velocity = new Vector2(battleActionFSM.rb.velocity.x, battleActionFSM.character.status.maxJumpSpeed * battleActionFSM.jumpCharge);
            }
        }
    }
}