using BehaviorDesigner.Runtime.Tasks;

namespace AI.FSM {
    [RequiredComponent(typeof(Character.Character))]
    public class CharacterLifeFSM : FSMBase {
        public Character.Character character { get; private set; }

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
        }

        protected override void SetUpFSM() {
            base.SetUpFSM();

            AliveState aliveState = new AliveState();
            aliveState.AddMap(FSMTriggerID.HealthZero, FSMStateID.Dead);
            aliveState.AddMap(FSMTriggerID.ImbalanceFull, FSMStateID.Dead);
            this._states.Add(aliveState);

            DeadState deadState = new DeadState();
            this._states.Add(deadState);
        }
    }
}