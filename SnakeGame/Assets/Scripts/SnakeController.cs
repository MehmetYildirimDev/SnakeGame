using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    private Vector2 AreaLimit = new Vector2(13, 24);
    private Vector2 _direction = Vector2.down;//yön

    [SerializeField] private float _GameSpeed = .3f;
    [SerializeField] private GameObject tailPrefeb;
    [SerializeField] private GameObject food;
    private List<Transform> _snake = new List<Transform>();

    [SerializeField] private UnityEngine.UI.Text ScoreText;
    [SerializeField] private UnityEngine.UI.Text GameOverText;

    private bool _grow;

    private int score;

    public int Score
    {
        get { return score; }
        set { score = value;
            ScoreText.text = score.ToString();        
        }
    }


    private void Start()
    {
        Score = 0;

        ChangePositionFood();
        StartCoroutine(routine: SnakeMove());
        _snake.Add(this.transform);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) && _direction != Vector2.right)
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

            if (_grow)
            {
                _grow = false;
                Grow();
            }

            for (int i = _snake.Count - 1; i > 0; i--)//listeye tersten gezme
            {
                _snake[i].position = _snake[i - 1].position;
            }

            var position = transform.position;
            position += (Vector3)_direction;//direction v2 oldugundan 3e cevirdik
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            this.transform.position = position;

            yield return new WaitForSeconds(_GameSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            _grow = true;
        }

        if (other.CompareTag("Wall"))
        {
            Dead();
        }
    }

    private void Grow()
    {
        Debug.Log("Grow");
        Score++;

        var tail = Instantiate(tailPrefeb);
        _snake.Add(tail.transform);
        _snake[_snake.Count - 1].position = _snake[_snake.Count - 2].position;
        ChangePositionFood();

    }

    private void ChangePositionFood()
    {

        Vector2 newFoodPosition;
        do
        {
            var x = (int)Random.Range(1, AreaLimit.x);
            var y = (int)Random.Range(1, AreaLimit.y);
            newFoodPosition = new Vector2(x, y);

        } while (!CanSpawnFood(newFoodPosition));


        food.transform.position = (Vector3)newFoodPosition;
    }

    private bool CanSpawnFood(Vector3 newposition)
    {
        foreach (var item in _snake)
        {
            var x = Mathf.RoundToInt(item.position.x);
            var y = Mathf.RoundToInt(item.position.y);
            
            if (item.transform.position == newposition)
            {
                return false;
            }
        }
        return true;    
    }

    private void Dead()
    {
        Debug.Log("dead");

        GameOverText.gameObject.SetActive(true);
        StopAllCoroutines();

    }


}
