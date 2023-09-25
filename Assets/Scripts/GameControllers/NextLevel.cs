using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    bool nextLevel;
    private void Update()
    {
        AllFruitsCollected();
    }
    public void AllFruitsCollected()
    {
        if (transform.childCount == 0)
        {
            nextLevel = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (nextLevel == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
