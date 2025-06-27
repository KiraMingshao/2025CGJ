namespace AI.FSM {
    public class AttackState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Attack;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM characterFSM) {
                characterFSM.attackCollider.enabled = true;
            }
        }

        public override void OnStateExit(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM characterFSM) {
                characterFSM.attackCollider.enabled = false;
            }
        }
    }
}