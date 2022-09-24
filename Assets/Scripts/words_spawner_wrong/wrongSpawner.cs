using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wrongSpawner : MonoBehaviour
{
    public word_item_wrong_action[] wordItem;

    public IEnumerator wait(string[] wrongWords, float wait)
    {
        for (int i = 0; i < wrongWords.Length; i++)
        {
            wordItem[i].Drop(wrongWords[i].ToString());
            yield return new WaitForSeconds(wait);
        }
    }
    public void SpawnWrongAnswers(string[] wrongWords)
    {
        StartCoroutine(wait(wrongWords, .04f));
    }
}
