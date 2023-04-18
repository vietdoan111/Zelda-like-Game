using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform swordTrans;
    public float swingSpeed = 720.0f;
    public float swingTime;
    public Transform swordTip;
    public Vector2 swordRange = new Vector2(1.0f, 1.0f);
    public LayerMask enemyMask;

    Vector3 rotateEnd;

    private void OnEnable()
    {
        rotateEnd = swordTrans.eulerAngles;
        rotateEnd += new Vector3(0, 0, 90.0f);
    }

    private void FixedUpdate()
    {
        SwingSword();
    }

    public void SwingSword()
    {
        //Sword swing animation
        swordTrans.Rotate(new Vector3(0, 0, swingSpeed) * Time.deltaTime);

        //Detect enemy
        DetectSwordHit();

        //Sword swing animtion end
        Vector3 check = rotateEnd - swordTrans.eulerAngles;
        if (Mathf.Abs(check.z) < 3.0f) gameObject.SetActive(false);
    }

    public void DetectSwordHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(swordTip.position, swordRange, -45.0f, enemyMask);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);
        }
    }
}