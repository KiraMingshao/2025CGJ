using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM {
    public class AttackState : FSMState {
        private Collider2D attackCollider;

        protected override void init() {
            this.StateID = FSMStateID.Attack;
        }

        private Collider2D getAttackCollider(CharacterBattleActionFSM fsm) {
            string lastKey = InputQueue.Instance.LastPressedKey;
            if (lastKey == "Sweep") {
                return fsm.sweepCollider;
            } else if (lastKey == "ChopUp") {
                return fsm.chopUpCollider;
            } else if (lastKey == "ChopDown") {
                return fsm.chopDownCollider;
            }
            return null;
        }

        public override void OnStateEnter(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM characterFSM) {
                this.attackCollider = this.getAttackCollider(characterFSM);
                this.attackCollider.enabled = true;

                characterFSM.character.status.imbalance += characterFSM.character.attackImbalanceIncrease;

                if (InputQueue.Instance.LastPressedKey == "ChopDown") {
                    var wave = Object.Instantiate(characterFSM.wave, characterFSM.transform.position, Quaternion.identity);
                    wave.tag = "PlayerWave";
                    int strength = Mathf.FloorToInt(characterFSM.character.GetDecoratedStatus().attack * characterFSM.waveStrengthFactor * characterFSM.transform.localScale.y);
                    wave.GetComponent<WaveController>().strength = strength;
                    wave.GetComponent<Rigidbody2D>().velocity = characterFSM.waveVelocityFactor / strength * Vector2.right;
                    var oldy = wave.transform.localScale.y;
                    wave.transform.localScale = new Vector2(Mathf.Abs(wave.transform.localScale.x) * -1, strength);
                    wave.transform.position = new Vector3(
                        wave.transform.position.x,
                        wave.transform.position.y + (oldy - wave.transform.localScale.y) / 2f,
                        wave.transform.position.z
                    );
                }
            }
        }

        public override void OnStateFixedStay(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM characterFSM) {
                List<Collider2D> result = new List<Collider2D>();
                this.attackCollider.OverlapCollider(new ContactFilter2D() { layerMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("EnemyBullet") }, result);
                foreach (var collider in result) {
                    if (collider.CompareTag("Enemy")) {
                        Enemy.Enemy enemy = collider.GetComponent<Enemy.Enemy>();
                        enemy.health -= characterFSM.character.GetDecoratedStatus().attack;
                    } else {
                        BulletController bulletController = collider.GetComponent<BulletController>();
                        bulletController.attack += characterFSM.character.GetDecoratedStatus().attack;
                        Rigidbody2D rigidbody = collider.GetComponent<Rigidbody2D>();
                        rigidbody.velocity = rigidbody.velocity * -3;
                        bulletController.gameObject.tag = "PlayerBullet";
                    }
                }
            }
        }

        public override void OnStateExit(FSMBase fsm) {
            if (fsm is CharacterBattleActionFSM characterFSM) {
                this.attackCollider.enabled = false;
            }
        }
    }
}