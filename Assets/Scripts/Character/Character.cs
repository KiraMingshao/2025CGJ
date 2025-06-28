using UnityEngine;

namespace Character {
    public class Character : MonoBehaviour {
        public Status status;
        public DecoratorParams decoratorParams;
        public AnimationCurve imbalanceGainCurve;

        public StatusDecorator decorator;
        [Header("Imbalance")]
        public int attackImbalanceIncrease;
        public int combactForce;

        private void Awake() {
            this.decorator = new DefaultDecorator(decoratorParams);
        }

        public Status GetDecoratedStatus() {
            return this.decorator.Decorate(status);
        }

        private void Update() {
            this.AddImbalance(this.GetImbalanceGain());
            this.ImbalanceCombact();
        }

        private void ImbalanceCombact() {
            var value = Input.GetAxis("ImbalanceCombact");
            var delta = Mathf.RoundToInt(value * combactForce * Time.deltaTime * 100);
            this.status.imbalance += delta;
        }

        private int GetRandomDirection() {
            int val = Random.Range(0, 2);
            return val == 1 ? 1 : -1;
        }

        private int GetImbalanceGain() {
            // return Mathf.RoundToInt(imbalanceGainCurve[(int)(Mathf.Abs(this.GetDecoratedStatus().imbalance) * 1f / this.status.maxImbalance)].value);
            return 0;
        }

        public void AddImbalance(int delta) {
            if (this.GetDecoratedStatus().imbalance == 0) {
                this.status.imbalance = this.GetRandomDirection() * delta;
            } else {
                this.status.imbalance += (this.status.imbalance > 0 ? 1 : -1) * delta;
            }
        }

        //public void ReduceImbalance(int delta) {
        //    if (this.GetDecoratedStatus().imbalance == 0) {
        //        return;
        //    }
        //    int maxDelta = Mathf.Max(delta, Mathf.Abs(this.GetDecoratedStatus().imbalance));
        //    this.status.imbalance -= (this.status.imbalance > 0 ? 1 : -1) * maxDelta;
        //}
    }
}
