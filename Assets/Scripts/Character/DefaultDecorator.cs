namespace Character {
    public class DefaultDecorator : StatusDecorator {
        public DefaultDecorator(DecoratorParams decoratorParams) : base(decoratorParams) {
        }

        public override Status Decorate(Status baseStatus) {
            return baseStatus;
        }
    }
}