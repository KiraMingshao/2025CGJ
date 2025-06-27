using UnityEngine;

namespace AI.FSM {
    public class AnimateFinishedTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            return Mathf.Approximately(fsm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f); 
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.AnimateFinished;
        }
    }
}