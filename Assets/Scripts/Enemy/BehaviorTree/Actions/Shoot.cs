using BehaviorDesigner.Runtime.Tasks;

namespace Enemy.BehaviorTree {
    public class ShootAction : Action {
        public float bulletSpeed;
        public int delayFrames;
        public string shootTrigger;
        private Enemy enemy;
        private int framesLeft;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override void OnStart() {
            this.framesLeft = this.delayFrames;
            this.enemy.animator.SetTrigger(shootTrigger);
        }

        public override TaskStatus OnUpdate() {
            if (this.framesLeft > 0) {
                --this.framesLeft;
                return TaskStatus.Running;
            }
            this.enemy.Shoot(this.bulletSpeed);
            return TaskStatus.Success;
        }
    }
}