using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManuever : MonoBehaviour
{

    public float dodge;
    public float smoothing;
    public float tilt;
    public Vector2 startWait;
    public Vector2 manuverTime;
    public Vector2 manuverWait;
    public Boundary boundary;

    private float targetManuver;
    private float currentSpeed;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = rb.velocity.y;
        StartCoroutine("Evade");
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            targetManuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(manuverTime.x, manuverTime.y));
            targetManuver = 0;
            yield return new WaitForSeconds(Random.Range(manuverWait.x, manuverWait.y));
        }
    }

    void FixedUpdate()
    {
        float newManuver = Mathf.MoveTowards(rb.velocity.x, targetManuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector3(newManuver, 0, currentSpeed);
        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0,
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
        );
    }
}
