using UnityEngine;

namespace AI.FSM {
    public class OnGroundTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                RaycastHit2D[] hit = new RaycastHit2D[1];
                battleActionFSM.footCollider.Cast(Vector2.down, new ContactFilter2D() {
                    layerMask = LayerMask.GetMask("Ground")
                }, hit, 0.01f, true);
                return hit[0].collider != null;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.OnGround;
        }
    }
}