using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{


    private Vector2 _direction = Vector2.down;//yön

    [SerializeField] private float _GameSpeed = .3f;


    private void Start()
    {
        StartCoroutine(routine: SnakeMove());
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && _direction!=Vector2.right)
        {
            _direction = Vector2.left;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) && _direction != Vector2.left)
        {
            _direction = Vector2.right;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) && _direction != Vector2.down)
        {
            _direction = Vector2.up;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _direction != Vector2.up)
        {
            _direction = Vector2.down;
        }





    }
    private IEnumerator SnakeMove()//Yilan surekli haraket etsin diye yazilan fonkstiuon
    {
        while (true)
        {
            var position = transform.position;
            position += (Vector3)_direction;//direction v2 oldugundan 3e cevirdik
            this.transform.position = position;

            yield return new WaitForSeconds(_GameSpeed);
        }
    }
}
