using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AI.FSM {
    [RequiredComponent(typeof(Character.Character))]
    public class CharacterBattleActionFSM : FSMBase {
        public Character.Character character { get; private set; }
        public Collider2D footCollider;
        public Collider2D bodyCollider;

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
        }
    }
}