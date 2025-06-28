using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WaveController : MonoBehaviour {
    public int strength;
    public int initSpeed;
    public float maxAliveTime = 100f;
    private new Rigidbody2D rigidbody;

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        this.rigidbody.velocity = initSpeed * Time.deltaTime * 100 * Vector2.right;
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
        if (collision.CompareTag("Wave")) {
            WaveController anotherWave = collision.GetComponent<WaveController>();
            if (this.rigidbody.velocity.x * collision.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0) {
                this.strength += anotherWave.strength;
                anotherWave.strength = 0;
            } else {
                int temp = this.strength;
                this.strength -= anotherWave.strength;
                anotherWave.strength -= temp;
            }
        } else if (collision.CompareTag("Enemy")) {
            Enemy.Enemy enemy = collision.GetComponent<Enemy.Enemy>();
            enemy.imbalance += this.strength;
            Destroy(this.gameObject);
        } else if (collision.CompareTag("Player")) {
            Character.Character character = collision.GetComponent<Character.Character>();
            character.status.imbalance += this.strength;
            Destroy(this.gameObject);
        }
    }
}
