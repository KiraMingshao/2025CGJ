using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class AttackAction : Action {
        public Animator animator;
        public string attackTrigger;
        public Collider2D attackCollider;
        public int additionalAttack;
        public bool isCharge;
        public string chargeTrigger;
        public float chargeSeconds;

        public override void OnAwake() {
            attackCollider.enabled = false;
        }

        public override void OnStart() {
            animator.SetTrigger(attackTrigger);
            attackCollider.enabled = true;
            if (isCharge) {
                animator.SetFloat(chargeTrigger, chargeSeconds);
            }
        }

        public override void OnEnd() {
            attackCollider.enabled = false;
        }
    }
}