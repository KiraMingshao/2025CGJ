namespace Character {
    public abstract class StatusDecorator {
        protected readonly DecoratorParams decoratorParams;
        public StatusDecorator(DecoratorParams decoratorParams) {
            this.decoratorParams = decoratorParams;
        }
        public abstract Status Decorate(Status baseStatus);
    }
}

