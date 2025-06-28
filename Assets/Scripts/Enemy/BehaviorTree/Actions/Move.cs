using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class MoveAction : Action {
        public float speed;
        private Rigidbody2D rb;

        public override void OnAwake() {
            this.rb = GetComponent<Rigidbody2D>();
        }

        public override TaskStatus OnUpdate() {
            this.rb.velocity = new Vector2(speed * Time.deltaTime * 100, this.rb.velocity.y);
            return TaskStatus.Running;
        }

        public override void OnEnd() {
            Debug.Log("reset velocity");
            this.rb.velocity = Vector2.zero;
        }
    }
}