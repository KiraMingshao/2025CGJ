namespace AI.FSM {
    public class AliveState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Alive;
        }
    }
}