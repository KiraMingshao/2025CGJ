using UnityEngine;

namespace AI.FSM {
    [RequireComponent(typeof(Character.Character))]
    public class CharacterEnergyFSM : FSMBase {
        public Character.Character character { get; private set; }

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
        }
    }
}
