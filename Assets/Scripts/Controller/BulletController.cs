using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BulletController : MonoBehaviour {
    [HideInInspector]
    public int attack;
    public float initSpeed;
    public float maxAliveTime = 100f;

    public new Rigidbody2D rigidbody { get; private set; }

    private void Awake() {
        this.rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void Start() {
        this.rigidbody.velocity = initSpeed * 100 * Time.deltaTime * Vector2.right;
    }

    private void Update() {
        maxAliveTime -= Time.deltaTime;
        if (maxAliveTime <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy") && this.CompareTag("PlayerBullet")) {
            Enemy.Enemy enemy = other.GetComponent<Enemy.Enemy>();
            enemy.health -= attack;
        } else if (other.CompareTag("Player") && this.CompareTag("EnemyBullet")) {
            Character.Character character = other.GetComponent<Character.Character>();
            character.status.health -= attack;
        }
    }
}
