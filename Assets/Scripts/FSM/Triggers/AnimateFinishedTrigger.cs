using UnityEngine;

namespace AI.FSM {
    public class AnimateFinishedTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            // Debug.Log(fsm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            float time = fsm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            return Mathf.Approximately(time, 1f) || time > 1f; 
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.AnimateFinished;
        }
    }
}