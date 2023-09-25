using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winpoint : MonoBehaviour
{
    public Text endGame;
    public GameObject bee;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (bee == null)
            {
                GetComponent<Animator>().enabled = true;
                endGame.text = "¡You Win!";
            }
        }
    }
}
