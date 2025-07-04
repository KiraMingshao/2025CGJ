using BehaviorDesigner.Runtime;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        public Collider2D attackCollider;
        public Collider2D bodyCollider;
        public Rigidbody2D rb { get; private set; }
        public Animator animator;
        public GameObject bullet;
        public GameObject wave;
        public Vector3 spawnPosition;
        public Slider hpSlider;
        public float secondsBeforeDestroy = 5f;

        [Header("Properties")]
        public int health;
        public int attack;
        public int resilience;
        public int imbalance;
        public int maxImbalance;
        public bool isAir;
        [HideInInspector]
        public int additionalAttack;
        [HideInInspector]
        public bool hasBeenAttacked = false;

        [Header("Animator Triggers")]
        public string attackTrigger;
        public string chargeTrigger;
        public string respawnTrigger;

        [Header("Wave Strength Params")]
        public float waveStrengthFactor = 0.8f;
        public float waveVelocityFactor = 5f;

        public void Awake() {
            this.rb = this.GetComponent<Rigidbody2D>();
            hpSlider.maxValue = health;
        }

        public void Attack(int additionalAttack) {
            this.animator.SetTrigger(attackTrigger);
            this.attackCollider.enabled = true;
            this.additionalAttack = additionalAttack;
        }

        public void Shoot(int additionalAttack) {
            this.additionalAttack = additionalAttack;
            var newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
            newBullet.tag = "EnemyBullet";
            newBullet.layer = LayerMask.NameToLayer("EnemyBullet");
            newBullet.GetComponent<BulletController>().attack = this.attack + additionalAttack;
        }

        public void CreateWave(int additionalAttack) {
            var newWave = Instantiate(wave, this.transform.position, Quaternion.identity);
            newWave.tag = "EnemyWave";
            newWave.layer = LayerMask.NameToLayer("EnemyWave");
            int strength = Mathf.FloorToInt((attack + additionalAttack) * waveStrengthFactor * this.transform.localScale.y);
            newWave.GetComponent<WaveController>().strength = strength;
            newWave.GetComponent<Rigidbody2D>().velocity = waveVelocityFactor / strength * Vector2.left;
            newWave.GetComponent<WaveController>().Rescale(strength);
        }

        public void Respawn() {
            //this.animator?.SetTrigger(respawnTrigger);
            this.transform.position = spawnPosition;
            this.rb.velocity = Vector2.zero;           
            hpSlider.value = health;
        }

        private IEnumerator Dead() {
            this.bodyCollider.enabled = false;
            this.GetComponent<Rigidbody2D>().gravityScale = 1;
            this.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>().enabled = false;
            yield return new WaitForSeconds(this.secondsBeforeDestroy);
            Destroy(this.gameObject);
        }

        private void Update() {
            if (this.health <= 0 || this.imbalance >= this.maxImbalance) {
                LevelManager.Instance.OnEnemyDeath(this.gameObject);
                StartCoroutine(this.Dead());
            }

            DOTween.To(() => hpSlider.value, x => hpSlider.value = x, health, 0.3f);
        }
    }
}
