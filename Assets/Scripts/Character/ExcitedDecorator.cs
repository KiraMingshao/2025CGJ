namespace Character {
    public class ExcitedDecorator : StatusDecorator {
        public ExcitedDecorator(DecoratorParams decoratorParams) : base(decoratorParams) {
        }

        public override Status Decorate(Status baseStatus) {
            return new Status(baseStatus.maxHealth, baseStatus.maxImbalance, baseStatus.maxEnergy, baseStatus.lowEnergyBoundary) {
                health = baseStatus.health,
                resilience = baseStatus.resilience + decoratorParams.resilienceIncrease,
                imbalance = baseStatus.imbalance,
                attack = baseStatus.attack + decoratorParams.attackIncrease,
                attackSpeed = baseStatus.attackSpeed * decoratorParams.attackSpeedIncrease,
                energy = baseStatus.energy,
                speed = baseStatus.speed
            };
        }
    }
}