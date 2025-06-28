namespace AI.FSM {
    public class CrouchState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Crouch;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.character.status.resilience += battleActionFSM.character.status.increaseResilience;
            }
        }

        public override void OnStateExit(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.character.status.resilience -= battleActionFSM.character.status.increaseResilience;
            }
        }
    }
}