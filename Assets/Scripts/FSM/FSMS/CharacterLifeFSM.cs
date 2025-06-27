using BehaviorDesigner.Runtime.Tasks;

namespace AI.FSM {
    [RequiredComponent(typeof(Character.Character))]
    public class CharacterLifeFSM : FSMBase {
        public Character.Character character { get; private set; }

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
        }
    }
}