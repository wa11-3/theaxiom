using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    public abstract string shipName { get; }

    public abstract int atkPoints { get; }
    public abstract int spdPoints { get; }
    public abstract int hltPoints { get; }

    float horizontalMove = 0;
    float verticalMove = 0;

    Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        this.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        //Debug.Log(spdPoints);
    }

    private void FixedUpdate()
    {
        horizontalMove += Input.GetAxisRaw("Horizontal") * spdPoints;
        verticalMove = Input.GetAxisRaw("Vertical") * spdPoints;
        
        this.transform.rotation = Quaternion.AngleAxis(horizontalMove, Vector3.forward);
        _rigidbody.AddRelativeForce(new Vector2(0, verticalMove), ForceMode2D.Impulse);
    }
}
