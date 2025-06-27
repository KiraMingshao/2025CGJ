namespace AI.FSM {
    public class AttackKeyPressedTrigger : FSMTrigger {
        private string[] attackKeys = {
            "Sweep", "ChopUp", "ChopDown"
        };
        public override bool HandleTrigger(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM) {
                return InputQueue.Instance.GetKey(attackKeys).HasValue;
            }
            throw new NotSupportedFSMTypeException(this.TriggerID, fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.AttackKeyPressed;
        }
    }
}