using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class TeleportAction : Action {
        private Enemy enemy;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override TaskStatus OnUpdate() {
            this.enemy.Respawn();
            return TaskStatus.Success;
        }
    }
}