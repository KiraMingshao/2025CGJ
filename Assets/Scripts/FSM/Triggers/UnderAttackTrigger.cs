using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM {
    public class UnderAttackTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                List<Collider2D> result = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D() {
                    layerMask = LayerMask.GetMask("EnemyAttack", "EnemyBullet", "EnemyWave"),
                    useTriggers = true,
                };
                if (battleActionFSM.bodyCollider.enabled) {
                    battleActionFSM.bodyCollider.OverlapCollider(filter, result);
                } else {
                    battleActionFSM.crouchCollider.OverlapCollider(filter, result);
                }
                return result.Count > 0;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.UnderAttack;
        }
    }
}