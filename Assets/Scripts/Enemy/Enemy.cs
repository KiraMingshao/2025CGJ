using System.Threading.Tasks;
using UnityEngine;

namespace Enemy {
    public class Enemy : MonoBehaviour {
        public Collider2D attackCollider;
        public Rigidbody2D rb { get; private set; }
        public Animator animator;
        public GameObject bullet;
        public Transform spawnPosition;

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
        }

        public async void Attack(int additionalAttack, float chargeTime) {
            this.animator.SetFloat(chargeTrigger, chargeTime);
            await Task.Delay((int)(chargeTime * 1000));
            this.Attack(additionalAttack);
        }

        public void Shoot(float bulletSpeed) {
            var newBullet = Instantiate(bullet, this.transform);
            newBullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * 100 * Time.deltaTime * Vector2.left;
        }

        public void Respawn() {
            this.animator.SetTrigger(respawnTrigger);
            this.transform.position = spawnPosition.position;
        }
    }
}