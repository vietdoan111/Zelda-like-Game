using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatTreantArm : GreatTreant
{

    public float lookRadius = 10f;

    public float speed;

    public Transform target;

    Vector2 startingPos;

    public GameObject projectile;

    public float TimeBetweenShot;

    public float nextShotTime;

    Vector2 shootingPos;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        state = GreatTreantState.stop;
        rb = this.GetComponent<Rigidbody2D>();
        startingPos = rb.position;
    }

    private void Update()
    {
        if (state == GreatTreantState.stagger) return;
        if (health < 7)
        {
            state = GreatTreantState.enraged;
            shootingPos.x = transform.position.x;
            shootingPos.y = transform.position.y - 3;
            if (Time.time > nextShotTime)
            {
                nextShotTime = Time.time + TimeBetweenShot;
                EnemyProjectile fire = Instantiate(projectile, shootingPos, Quaternion.identity).GetComponent<EnemyProjectile>();
                fire.targetPos = target.position;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == GreatTreantState.stagger) return;
        if (Vector2.Distance(transform.position, target.position) <= lookRadius && Vector2.Distance(transform.position, startingPos) <= 1.5f)
        {
            Chase();
        }
        else if (state != GreatTreantState.enraged)
        {
            Retreat();
        }
    }

    public void Chase()
    {
        if (state != GreatTreantState.normal) return;
        state = GreatTreantState.normal;
        Vector2 lookDir = target.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void Retreat()
    {
        if (state != GreatTreantState.normal) return;
        state = GreatTreantState.normal;
        StartCoroutine(retreatCo());
    }

    IEnumerator retreatCo()
    {
        yield return new WaitForSeconds(0.2f);
        state = GreatTreantState.normal;
        transform.position = Vector2.MoveTowards(transform.position, startingPos, speed * Time.deltaTime);
    }
}
