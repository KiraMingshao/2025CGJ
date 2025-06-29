using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class WaveController : MonoBehaviour {
    public int strength;
    public float maxAliveTime = 100f;
    public float offsety;
    private new Rigidbody2D rigidbody;

    private void Awake() {
        this.rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        this.transform.position += new Vector3(0, offsety, 0);
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

    public void Rescale(float newScale) {
        var oldy = this.transform.localScale.y;
        this.transform.localScale = new Vector3(this.transform.localScale.x, newScale, 1);
        //this.transform.position = new Vector3(
        //    this.transform.position.x,
        //    this.transform.position.y,
        //    this.transform.position.z
        //);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerWave") || collision.CompareTag("EnemyWave")) {
            WaveController anotherWave = collision.GetComponent<WaveController>();
            if (this.rigidbody.velocity.x * collision.gameObject.GetComponent<Rigidbody2D>().velocity.x > 0) {
                this.strength += anotherWave.strength;
                this.Rescale(this.strength);
                anotherWave.strength = 0;
            } else {
                int temp = this.strength;
                this.strength -= anotherWave.strength;
                this.Rescale(this.strength);
                anotherWave.strength -= temp;
                anotherWave.Rescale(anotherWave.strength);
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
