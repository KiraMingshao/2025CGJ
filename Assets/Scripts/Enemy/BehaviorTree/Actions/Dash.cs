using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class DashAction : Action {
        public float moveSpeed;
        public float dashSpeed;
        public Vector3 startDashPosition;
        public float eps = 0.05f;
        private Enemy enemy;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate() {
            if (Mathf.Abs(Vector3.Distance(this.transform.position, this.startDashPosition)) > eps) {
                Vector3 direction = (this.startDashPosition - this.transform.position).normalized;
                this.enemy.rb.velocity = direction * this.moveSpeed * Time.deltaTime * 100;
                return TaskStatus.Running;
            } else {
                this.enemy.rb.velocity = this.dashSpeed * 100 * Time.deltaTime * Vector2.left;
                return TaskStatus.Success;
            }
        }
    }
}