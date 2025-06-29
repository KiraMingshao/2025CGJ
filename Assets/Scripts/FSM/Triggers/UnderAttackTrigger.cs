using UnityEngine;

namespace AI.FSM {
    public class UnderAttackTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                return battleActionFSM.bodyCollider.IsTouchingLayers(LayerMask.GetMask("EnemyAttack", "EnemyBullet", "EnemyWave"));
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.UnderAttack;
        }
    }
}