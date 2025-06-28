using UnityEngine;

public class RopeBehavior : MonoBehaviour {
    public GameObject ropePrefab;
    public GameObject head;
    public GameObject tail;
    public int ropeCount = 32;

    //private GameObject headLink, tailLink;

    private void Start() {
        float distance = Vector3.Distance(head.transform.position, tail.transform.position);
        Vector3 direction = (tail.transform.position - head.transform.position).normalized;
        Vector3 scale = direction * distance / (ropeCount - 1);

        Vector3 curPos = head.transform.position;
        var lastRigidBody = head.GetComponent<Rigidbody2D>();
        for (int i = 0; i < ropeCount; ++i) {
            var rope = Instantiate(ropePrefab, curPos, Quaternion.identity, this.transform);
            rope.GetComponent<HingeJoint2D>().connectedBody = lastRigidBody;
            rope.transform.localScale = Vector3.Max(scale, new Vector3(0.5f, 0.5f));

            if (i == ropeCount - 1) {
                var joint = rope.AddComponent<HingeJoint2D>();
                joint.anchor = Vector2.right;
                joint.connectedBody = tail.GetComponent<Rigidbody2D>();
            }

            lastRigidBody = rope.GetComponent<Rigidbody2D>();
            curPos += scale;
        }
    }
}
