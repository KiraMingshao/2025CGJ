using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree.Composites {
    public class FirstSuccess : Composite {
        private int currentChildIndex = 0;
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override int CurrentChildIndex() {
            return currentChildIndex;
        }

        public override bool CanExecute() {
            return currentChildIndex < children.Count && executionStatus != TaskStatus.Success;
        }

        public override void OnChildExecuted(TaskStatus childStatus) {
            currentChildIndex++;
            executionStatus = childStatus;
        }

        public override void OnConditionalAbort(int childIndex) {
            currentChildIndex = childIndex;
            executionStatus = TaskStatus.Inactive;
        }

        public override void OnEnd() {
            executionStatus = TaskStatus.Inactive;
            currentChildIndex = 0;
        }
    }
}
