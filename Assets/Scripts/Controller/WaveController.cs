using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WaveController : MonoBehaviour {
    public int strength;
    public float maxAliveTime = 100f;
    private new Rigidbody2D rigidbody;

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (this.strength <= 0) {
            Destroy(this.gameObject);
        }
        maxAliveTime -= Time.deltaTime;
        if (maxAliveTime <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerWave") || collision.CompareTag("EnemyWave")) {
            WaveController anotherWave = collision.GetComponent<WaveController>();
            if (this.rigidbody.velocity.x * collision.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0) {
                this.strength += anotherWave.strength;
                anotherWave.strength = 0;
            } else {
                int temp = this.strength;
                this.strength -= anotherWave.strength;
                anotherWave.strength -= temp;
            }
        } else if (collision.CompareTag("Enemy") && this.CompareTag("PlayerWave")) {
            Enemy.Enemy enemy = collision.GetComponent<Enemy.Enemy>();
            enemy.imbalance += Mathf.Max(0, this.strength - enemy.resilience);
            Destroy(this.gameObject);
        } else if (collision.CompareTag("Player") && this.CompareTag("EnemyWave")) {
            Character.Character character = collision.GetComponent<Character.Character>();
            character.AddImbalance(Mathf.Max(0, this.strength - character.GetDecoratedStatus().resilience));
            Destroy(this.gameObject);
        }
    }
}
