namespace AI.FSM {
    public class UnderAttackState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.UnderAttack;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {

            }
        }
    }
}