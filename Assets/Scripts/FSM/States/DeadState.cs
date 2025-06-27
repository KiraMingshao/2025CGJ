namespace AI.FSM {
    public class DeadState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Dead;
        }
    }
}