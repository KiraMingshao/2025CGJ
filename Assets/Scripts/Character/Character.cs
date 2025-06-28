using UnityEngine;

namespace Character {
    public class Character : MonoBehaviour {
        public Status status;
        public DecoratorParams decoratorParams;

        public StatusDecorator decorator;

        private void Awake() {
            this.decorator = new DefaultDecorator(decoratorParams);
        }

        public Status GetDecoratedStatus() {
            return this.decorator.Decorate(status);
        }
    }
}
