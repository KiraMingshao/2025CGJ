using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class DashAction : Action {
        public float moveSpeed;
        public float dashSpeed;
        public Vector3 startDashPosition;
        public Vector3 endDashPosition;
        public float eps = 0.05f;
        private Enemy enemy;
        private bool isDashing = false;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override void OnStart() {
            this.isDashing = false;
        }

        public override TaskStatus OnUpdate() {
            if (!this.isDashing && Vector3.Distance(this.transform.position, this.startDashPosition) > eps * this.moveSpeed) {
                Vector3 direction = (this.startDashPosition - this.transform.position).normalized;
                this.enemy.rb.velocity = direction * this.moveSpeed * Time.deltaTime * 100;
                return TaskStatus.Running;
            } else {
                this.isDashing = true;
                this.enemy.rb.velocity = this.dashSpeed * 100 * Time.deltaTime * Vector2.left;
                if (Vector3.Distance(this.transform.position, this.endDashPosition) < eps * this.dashSpeed) {
                    this.enemy.Respawn();
                    return TaskStatus.Success;
                }
                return TaskStatus.Running;
            }
        }
    }
}