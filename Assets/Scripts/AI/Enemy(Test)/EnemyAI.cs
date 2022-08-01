using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAI : NetworkBehaviour
{
    Rigidbody2D enemyRb;
    [SerializeField] float speed = 1;

    //Patroluing
    float patroulingTimer;
    float directionLenght;
    Vector2 patroulingDirection = Vector2.left;


    StateMachine<EnemyAI> stateMachine { get; set; }

    private void Start()
    {
        patroulingTimer = Random.Range(1f, 7f);
        enemyRb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine<EnemyAI>(this);
        stateMachine.ChangeState(PatroulState.Instance);
    }


    private void Update()
    {
        stateMachine.Update();
    }


    public void Patrouling()
    {
        patroulingTimer -= Time.deltaTime;

        if(patroulingTimer <= 0)
        {
            patroulingTimer = Random.Range(1f, 7f);
            if (patroulingDirection == Vector2.left) patroulingDirection = Vector2.right;
            else if(patroulingDirection == Vector2.right) patroulingDirection = Vector2.left;
        }
        transform.Translate(patroulingDirection * Time.deltaTime * speed);

    }

}
