using BehaviorDesigner.Runtime.Tasks;

namespace Enemy.BehaviorTree {
    public class AttackAction : Action {
        public int additionalAttack;
        private Enemy enemy;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
            this.enemy.attackCollider.enabled = false;
        }

        public override void OnStart() {
            this.enemy.Attack(additionalAttack);
        }

        public override void OnEnd() {
            this.enemy.attackCollider.enabled = false;
        }
    }
}