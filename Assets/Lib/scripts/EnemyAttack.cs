using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private GameObject attackArea;
    // Start is called before the first frame update
    void Start()
    {
        attackArea = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void StartAttack()
    {
        attackArea.SetActive(true);
    }

    void EndAttack()
    {
        attackArea.SetActive(false);
    }
}
