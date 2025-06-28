using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class MoveAction : Action {
        public float speed;
        private Rigidbody2D rb;

        public override void OnAwake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public override TaskStatus OnUpdate() {
            this.rb.velocity = speed * Time.deltaTime * Vector2.left;
            return TaskStatus.Running;
        }
    }
}