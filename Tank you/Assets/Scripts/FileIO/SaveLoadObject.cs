using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveLoadObject
{
    private int lives;
    private string sceneName;
    private bool opened;

    public SaveLoadObject(int lives, string sceneName, bool opened)
    {
        this.lives = lives;
        this.sceneName = sceneName;
        this.opened = opened;
    }

    public int getLives()
    {
        return lives;
    }

    public string getSceneName()
    {
        return sceneName;
    }

    public bool isOpened()
    {
        return opened;
    }
}
