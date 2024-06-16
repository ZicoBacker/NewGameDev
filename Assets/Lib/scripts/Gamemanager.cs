using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    //game manager singleton yippie
    public static Gamemanager instance { get; private set; }
    public Transform playerPosition;
    public GameObject player;

    public PlayerController playerScript;

    public TextMeshProUGUI playerHealthText;

    public float playerHealth = 0f;



    // on script start
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform;
    }
    public void UpdateHealth(float health)
    {
        playerHealth = health;
        playerHealthText.text = "hp: " + health;
    }
}
