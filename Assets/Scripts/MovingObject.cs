using UnityEngine;

// Parent class for moving objects, Player and Animal(s) alike
public abstract class MovingObject : MonoBehaviour
{
    public LayerMask blockingLayer; // check for collisions

	protected float speed = 0f;

    private BoxCollider2D boxCollider; // attached to this object
    protected Rigidbody2D rb;


    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected abstract void Move();

	//protected abstract void OnCollisionEnter(Collision collision);
}
