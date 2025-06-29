namespace AI.FSM {
    public class DeadState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Dead;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterLifeFSM lifeFSM) {
                CharacterBattleActionFSM battleActionFSM = lifeFSM.GetComponent<CharacterBattleActionFSM>();
                battleActionFSM.bodyCollider.enabled = false;
                battleActionFSM.crouchCollider.enabled = false;
            }
        }
    }
}