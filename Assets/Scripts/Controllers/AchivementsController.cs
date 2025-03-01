using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchivementsController : MonoBehaviour
{
    [SerializeField] private List<GameObject> catsList;
    private void Start() {
        CheckAchivements();
    }

    private void CheckAchivements()
    {
        for (int i = 0; i < catsList.Count; i++)
        {
            if(GameController.Instance.IsAchievementUnlocked(i))
            {
                catsList[i].SetActive(true);
            }
        }
    }
}
