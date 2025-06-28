using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class Timer : Decorator {
        public float time;

        private float beginTime = -1;

        public override bool CanExecute() {
            if (Mathf.Approximately(beginTime, -1))
                return true;
            return beginTime + time > Time.time;
        }

        public override int CurrentChildIndex() {
            if (Mathf.Approximately(beginTime, -1))
                return -1;
            return 0;
        }

        public override void OnChildExecuted(TaskStatus childStatus) {
            if (beginTime + time < Time.time) {
                BehaviorManager.instance.Interrupt(Owner, this, TaskStatus.Success);
            }
        }

        public override TaskStatus OverrideStatus(TaskStatus status) {
            if (status != TaskStatus.Inactive && status != TaskStatus.Running)
                return TaskStatus.Success;
            return status;
        }


        public override void OnStart() {
            Debug.Log("Start timer");
            beginTime = Time.time;
            StartCoroutine(interruptChild());
        }

        private IEnumerator interruptChild() {
            yield return new WaitForSeconds(time);
            BehaviorManager.instance.Interrupt(Owner, this, TaskStatus.Success);
        }

        public override void OnEnd() {
            Debug.Log("Reset timer " + beginTime + ' ' + time + ' ' + Time.time);
            beginTime = -1;
        }
    }
}
