using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public Button startBtn;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(LoadLevels);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadLevels()
    {
        SceneManager.LoadScene("Levels");
    }
}
