using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeController : MonoBehaviour
{

    private Vector2 AreaLimit = new Vector2(13, 24);
    private Vector2 _direction = Vector2.down;//yön

    [SerializeField] private float _GameSpeed = .2f;

    [SerializeField] private GameObject tailPrefeb;
    [SerializeField] private GameObject food;
    [SerializeField] private GameObject poison;//zehir
    [SerializeField] private GameObject velocity;//Hýz

    private List<Transform> _snake = new List<Transform>();

    [SerializeField] private UnityEngine.UI.Text ScoreText;
    [SerializeField] private UnityEngine.UI.Text HealtText;
    [SerializeField] private UnityEngine.UI.Image GameOverBg;

    private bool _grow = false;

    private int score;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreText.text = score.ToString();
        }
    }
    private int healt;

    public int Healt
    {
        get { return healt; }
        set
        {
            healt = value;
            HealtText.text = healt.ToString();
        }
    }


    private void Start()
    {
        Score = 0;
        Healt = 3;

        ChangePosition();
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
        if (other.CompareTag("Velocity"))
        {
            VelocityF();
        }
        if (other.CompareTag("Poison"))
        {
            PoisonF();
        }

    }

    private void Grow()
    {

        Score++;
        

        var tail = Instantiate(tailPrefeb);
        _snake.Add(tail.transform);
        _snake[_snake.Count - 1].position = _snake[_snake.Count - 2].position;
        ChangePosition();

    }

    private void ChangePosition()
    {

        Vector2 newFoodPosition;
        Vector2 newVeloPosition;
        Vector2 newPoisonPosition;
        do
        {
            var x = (int)Random.Range(1, AreaLimit.x);
            var y = (int)Random.Range(1, AreaLimit.y);
            newFoodPosition = new Vector2(x, y);

            var x1 = (int)Random.Range(1, AreaLimit.x);
            var y1 = (int)Random.Range(1, AreaLimit.y);
            newVeloPosition = new Vector2(x1, y1); 
            
            var x2 = (int)Random.Range(1, AreaLimit.x);
            var y2 = (int)Random.Range(1, AreaLimit.y);
            newPoisonPosition = new Vector2(x2, y2);

        } while (!CanSpawn(newFoodPosition) && !CanSpawn(newVeloPosition) && !CanSpawn(newPoisonPosition));

        food.transform.position = (Vector3)newFoodPosition;
        velocity.transform.position = (Vector3)newVeloPosition;
        poison.transform.position = (Vector3)newPoisonPosition;

    }

    private bool CanSpawn(Vector3 newposition)
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

    private void VelocityF()
    {
        _GameSpeed = .75f * _GameSpeed;
        ChangePosition();
    }

    private void PoisonF()
    {

        Healt--;
        if (Healt<=0)
        {
            Dead();
        }
        
        int rand = (int)Random.Range(1, 3);

        if (rand==1)
        {
            _GameSpeed = 1.25f * _GameSpeed;
        }
        if (rand==2 && _snake.Count>1)
        {
           
            Destroy(_snake[_snake.Count - 1].gameObject);//Son kuyrugu yok et ;
            _snake.RemoveAt(_snake.Count - 1);
            Score--;//Score dusur
        }
        ChangePosition();
    }

    private void Dead()
    {
        GameOverBg.gameObject.SetActive(true);
        StopAllCoroutines();
    }

    public void ReplayButton()
    {
        SceneManager.LoadScene(0);
    }


}
