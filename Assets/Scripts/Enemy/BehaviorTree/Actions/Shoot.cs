using BehaviorDesigner.Runtime.Tasks;

namespace Enemy.BehaviorTree {
    public class ShootAction : Action {
        public int delayFrames;
        public string animTrigger;
        private Enemy enemy;
        private int framesLeft;

        public enum Type { 
            Bullet,
            Wave,
        }
        public Type type;

        public override void OnAwake() {
            this.enemy = this.GetComponent<Enemy>();
        }

        public override void OnStart() {
            this.framesLeft = this.delayFrames;
            this.enemy.animator.SetTrigger(animTrigger);
        }

        public override TaskStatus OnUpdate() {
            if (this.framesLeft > 0) {
                --this.framesLeft;
                return TaskStatus.Running;
            }
            if (this.type == Type.Bullet)
                this.enemy.Shoot();
            else
                this.enemy.CreateWave();
            return TaskStatus.Success;
        }
    }
}