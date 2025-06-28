using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class MoveAction : Action {
        public float speed;
        public Rigidbody2D rb;

        public override TaskStatus OnUpdate() {
            this.rb.velocity = speed * Time.deltaTime * Vector2.left;
            return TaskStatus.Running;
        }
    }
}