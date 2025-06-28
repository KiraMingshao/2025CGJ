using UnityEngine;

namespace AI.FSM {
    [RequireComponent(typeof(Character.Character))]
    public class CharacterEnergyFSM : FSMBase {
        public Character.Character character { get; private set; }

        protected override void Init() {
            this.character = this.GetComponent<Character.Character>();
        }

        protected override void SetUpFSM() {
            base.SetUpFSM();

            EnergyNormalState energyNormalState = new EnergyNormalState();
            energyNormalState.AddMap(FSMTriggerID.EnergyFull, FSMStateID.EnergyExcited);
            this._states.Add(energyNormalState);

            EnergyExcitedState energyExcitedState = new EnergyExcitedState();
            energyExcitedState.AddMap(FSMTriggerID.EnergyLow, FSMStateID.EnergyNormal);
            this._states.Add(energyExcitedState);
        }
    }
}
