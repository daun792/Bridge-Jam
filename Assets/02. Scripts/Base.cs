using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : Singleton<Base>
{
    #region Load Scene
    public static void LoadScene(SceneName _type)
    {
        DOTween.KillAll();
        SceneManager.LoadScene((int)_type);
    }

    public static void LoadSceneAdditive(SceneName _type)
    {
        SceneManager.LoadScene((int)_type, LoadSceneMode.Additive);
    }
    #endregion
}
