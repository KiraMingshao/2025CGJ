using DG.Tweening;
namespace AI.FSM {
    public class UnderAttackState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.UnderAttack;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                PlayUnderAttackAnim(battleActionFSM);
            }
        }

        private void PlayUnderAttackAnim(CharacterBattleActionFSM battleActionFSM)
        {
            battleActionFSM.transform.DOPunchPosition(new UnityEngine.Vector3(-10, 0, 0), 0.5f, 5);
        }
    }
}