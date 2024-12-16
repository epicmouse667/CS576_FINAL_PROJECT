using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level_Manager : MonoBehaviour
{
    public Button level1;
    public Button level2;
    // Start is called before the first frame update
    void Start()
    {
        level1.onClick.AddListener(delegate { LoadLevel(1); });
        level2.onClick.AddListener(delegate { LoadLevel(2); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    //TODO: Update level name
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("level" + level);
    }
}
