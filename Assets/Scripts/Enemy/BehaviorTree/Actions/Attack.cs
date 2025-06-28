using BehaviorDesigner.Runtime.Tasks;

namespace Enemy.BehaviorTree {
    public class AttackAction : Action {
        public int additionalAttack;
        public bool isCharge;
        public float chargeSeconds;
        private Enemy enemy;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
            this.enemy.attackCollider.enabled = false;
        }

        public override void OnStart() {
            if (isCharge) {
                this.enemy.Attack(additionalAttack, chargeSeconds);
            } else {
                this.enemy.Attack(additionalAttack);
            }
        }

        public override void OnEnd() {
            this.enemy.attackCollider.enabled = false;
        }
    }
}