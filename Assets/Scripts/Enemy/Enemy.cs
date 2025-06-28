using System.Threading.Tasks;
using UnityEngine;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        public Collider2D attackCollider;
        public Collider2D bodyCollider;
        public Rigidbody2D rb { get; private set; }
        public Animator animator;
        public GameObject bullet;
        public Transform spawnPosition;

        [Header("Properties")]
        public int health;
        public int attack;
        public int resilience;
        public int imbalance;
        public int maxImbalance;
        [HideInInspector]
        public int additionalAttack;

        [Header("Animator Triggers")]
        public string attackTrigger;
        public string chargeTrigger;
        public string respawnTrigger;

        public void Awake() {
            this.rb = this.GetComponent<Rigidbody2D>();
        }

        public void Attack(int additionalAttack) {
            this.animator.SetTrigger(attackTrigger);
            this.attackCollider.enabled = true;
            this.additionalAttack = additionalAttack;
        }

        public async void Attack(int additionalAttack, float chargeTime) {
            this.animator.SetFloat(chargeTrigger, chargeTime);
            await Task.Delay((int)(chargeTime * 1000));
            this.Attack(additionalAttack);
        }

        public void Shoot() {
            var newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
            newBullet.tag = "EnemyBullet";
            newBullet.GetComponent<BulletController>().attack = this.attack;
        }

        public void Respawn() {
            this.animator.SetTrigger(respawnTrigger);
            this.transform.position = spawnPosition.position;
        }

        private void Update() {
            if (this.health <= 0 || this.imbalance >= this.maxImbalance) {
                Destroy(this.gameObject);
            }
        }
    }
}