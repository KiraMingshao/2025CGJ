using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class Timer : Decorator {
        public float time;

        private float beginTime = -1;
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override bool CanExecute() {
            if (Mathf.Approximately(beginTime, -1))
                return true;
            return beginTime + time > Time.time;
        }

        public override int CurrentChildIndex() {
            if (Mathf.Approximately(beginTime, -1))
                return 0;
            return -1;
        }

        public override TaskStatus Decorate(TaskStatus status) {
            return executionStatus;
        }

        public override TaskStatus OnUpdate() {
            if (beginTime + time > Time.time)
                executionStatus = TaskStatus.Success;
            return executionStatus;
        }

        public override void OnStart() {
            executionStatus = TaskStatus.Running;
            beginTime = Time.time;
        }

        public override void OnEnd() {
            executionStatus = TaskStatus.Inactive;
            beginTime = -1;
        }
    }
}
