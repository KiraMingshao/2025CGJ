using BehaviorDesigner.Runtime.Tasks;

namespace Enemy.BehaviorTree {
    public class ShootAction : Action {
        public float bulletSpeed;
        public int delayFrames;
        private Enemy enemy;
        private int framesLeft;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override void OnStart() {
            this.framesLeft = this.delayFrames;
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