using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class DashAction : Action {
        public float moveSpeed;
        public float dashSpeed;
        public SharedTransform startDashPosition;
        private Enemy enemy;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate() {
            if (!Mathf.Approximately(Vector3.Distance(this.transform.position, this.startDashPosition.Value.position), 0)) {
                Vector3 direction = (this.startDashPosition.Value.position - this.transform.position).normalized;
                this.enemy.rb.velocity = direction * this.moveSpeed * Time.deltaTime * 100;
                return TaskStatus.Running;
            } else {
                this.enemy.rb.velocity = this.dashSpeed * 100 * Time.deltaTime * Vector2.left;
                return TaskStatus.Success;
            }
        }
    }
}