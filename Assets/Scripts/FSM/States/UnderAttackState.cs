using DG.Tweening;
using UnityEngine;

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
            var sprite = battleActionFSM.GetComponent<SpriteRenderer>();
            Sequence seq = DOTween.Sequence();
            //添加动画到序列中
            seq.Append(DOTween.To(() => sprite.color, x => sprite.color = x, Color.red, 0.2f));
            seq.Append(DOTween.To(() => sprite.color, x => sprite.color = x, Color.white, 0.3f));
            Debug.Log("受击");
            seq.SetAutoKill();
        }
    }
}