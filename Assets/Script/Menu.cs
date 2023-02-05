using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private RootEnemy rootEnemy = null;

    private void Awake()
    {
        rootEnemy.SetUp(null, 9001,null);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
