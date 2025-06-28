using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class MoveAction : Action {
        public Vector2 speed;
        private Rigidbody2D rb;

        public override void OnAwake() {
            this.rb = GetComponent<Rigidbody2D>();
        }

        public override TaskStatus OnUpdate() {
            this.rb.velocity = 100 * Time.deltaTime * speed;
            return TaskStatus.Running;
        }

        public override void OnEnd() {
            Debug.Log("reset velocity");
            this.rb.velocity = Vector2.zero;
        }
    }
}