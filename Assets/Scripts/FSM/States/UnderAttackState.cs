using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM {
    public class UnderAttackState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.UnderAttack;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM battleActionFSM) {
                PlayUnderAttackAnim(battleActionFSM);

                List<Collider2D> colliders = new List<Collider2D>();
                battleActionFSM.bodyCollider.OverlapCollider(new ContactFilter2D() {
                    layerMask = LayerMask.GetMask("Enemy"),
                    useTriggers = true,
                }, colliders);
                foreach (var collider in colliders) {
                    if (collider.CompareTag("Enemy")) {
                        Enemy.Enemy enemy = collider.GetComponent<Enemy.Enemy>();
                        var attack = enemy.attack + enemy.additionalAttack;
                        battleActionFSM.character.status.health -= attack;
                        battleActionFSM.character.AddImbalance(Mathf.Max(attack - battleActionFSM.character.GetDecoratedStatus().resilience));
                    }
                }
            }
        }

        private void PlayUnderAttackAnim(CharacterBattleActionFSM battleActionFSM)
        {
            battleActionFSM.transform.DOPunchPosition(new UnityEngine.Vector3(-0.5f, 0, 0), 0.5f, 5);
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