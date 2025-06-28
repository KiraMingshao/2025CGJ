using Character;

namespace AI.FSM {
    public class EnergyExcitedState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.EnergyExcited;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.character.decorator = new ExcitedDecorator(battleActionFSM.character.decoratorParams);
            }
        }

        public override void OnStateExit(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                battleActionFSM.character.decorator = new DefaultDecorator(battleActionFSM.character.decoratorParams);
            }
        }
    }
}