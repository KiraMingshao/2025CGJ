using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Enemy.BehaviorTree {
    public class PlayerDistance : Conditional {
        public float distance;
        public float eps;
        public enum Type {
            Equal,
            Less,
            Greater,
        }
        public Type type;

        public override TaskStatus OnUpdate() {
            float distance = (GameLauncher.Instance.player.transform.position - this.transform.position).magnitude;
            if (type == Type.Equal && Mathf.Abs(distance - this.distance) < eps) {
                return TaskStatus.Success;
            }
            if (type == Type.Less && distance < this.distance) {
                return TaskStatus.Success;
            }
            if (type == Type.Greater && distance > this.distance) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}