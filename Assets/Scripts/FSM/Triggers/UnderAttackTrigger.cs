using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI.FSM {
    public class UnderAttackTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                List<Collider2D> result = new List<Collider2D>();
                ContactFilter2D filter = new ContactFilter2D() {
                    layerMask = LayerMask.GetMask("Enemy", "EnemyBullet", "EnemyWave"),
                    useTriggers = true,
                };
                if (battleActionFSM.bodyCollider.enabled) {
                    battleActionFSM.bodyCollider.OverlapCollider(filter, result);
                } else {
                    battleActionFSM.crouchCollider.OverlapCollider(filter, result);
                }
                int cnt = 0;
                foreach (var collider in result) {
                    if (collider.CompareTag("Enemy") || collider.CompareTag("EnemyBullet") || collider.CompareTag("EnemyWave"))
                        ++cnt;
                }
                return cnt > 0;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.UnderAttack;
        }
    }
}