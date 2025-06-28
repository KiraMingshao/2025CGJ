using UnityEngine;

namespace Character {
    public class Character : MonoBehaviour {
        public Status status;
        public DecoratorParams decoratorParams;
        public AnimationCurve imbalanceGainCurve;

        public StatusDecorator decorator;
        [Header("Imbalance")]
        public int attackImbalanceIncrease;

        private void Awake() {
            this.decorator = new DefaultDecorator(decoratorParams);
        }

        public Status GetDecoratedStatus() {
            return this.decorator.Decorate(status);
        }

        private void Update() {
            this.AddImbalance(this.GetImbalanceGain());
        }

        private int GetRandomDirection() {
            int val = Random.Range(0, 2);
            return val == 1 ? 1 : -1;
        }

        private int GetImbalanceGain() {
            return Mathf.RoundToInt(imbalanceGainCurve[Mathf.Abs(this.GetDecoratedStatus().imbalance)].value);
        }

        public void AddImbalance(int delta) {
            if (this.GetDecoratedStatus().imbalance == 0) {
                this.status.imbalance = this.GetRandomDirection() * delta;
            } else {
                this.status.imbalance += (this.status.imbalance > 0 ? 1 : -1) * delta;
            }
        }
    }
}
