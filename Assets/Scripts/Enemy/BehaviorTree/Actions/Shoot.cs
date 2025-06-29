using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class ShootAction : Action {
        public int delayFrames;
        public string animTrigger;
        private Enemy enemy;
        private int framesLeft;
        public bool isCharge;
        public float chargeSeconds;
        private float chargeSecondsLeft;
        public int additionalAttack;

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
            this.chargeSecondsLeft = this.chargeSeconds;
            this.enemy.animator.SetTrigger(animTrigger);
        }

        public override TaskStatus OnUpdate() {
            if (this.isCharge && this.chargeSecondsLeft > 0) {
                this.chargeSecondsLeft -= Time.deltaTime;
                return TaskStatus.Running;
            }
            if (this.framesLeft > 0) {
                --this.framesLeft;
                return TaskStatus.Running;
            }
            if (this.type == Type.Bullet)
                this.enemy.Shoot(additionalAttack);
            else
                this.enemy.CreateWave(additionalAttack);
            return TaskStatus.Success;
        }
    }
}