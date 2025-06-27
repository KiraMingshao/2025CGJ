using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace AI.FSM {
    [RequiredComponent(typeof(Character.Character))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterBattleActionFSM : FSMBase {
        public Character.Character character { get; private set; }
        public Collider2D footCollider;
        public Collider2D bodyCollider;
        public Collider2D attackCollider;

        [HideInInspector]
        public float jumpCharge;
        public Rigidbody2D rb;

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
            this.attackCollider.enabled = false;
            this.rb = this.GetComponent<Rigidbody2D>();
        }
    }
}