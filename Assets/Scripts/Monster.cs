using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public float RotateSpeed = 1f;
    public float RotateRadius = 0.5f;

    private Vector2 _centre;
    private float _angle;

    // Use this for initialization
    void Start () {

        _centre = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        _angle += RotateSpeed * Time.deltaTime;

        var newPosition = new Vector3(transform.position.x + (Mathf.Sin(_angle) * RotateRadius), transform.position.y, transform.position.z + (Mathf.Cos(_angle) * RotateRadius));
        transform.position = newPosition;
    }
}
