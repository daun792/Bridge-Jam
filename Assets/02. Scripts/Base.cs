using UnityEngine.SceneManagement;
using DG.Tweening;

public enum SceneName
{
    Title,
    Game
}

public class Base : Singleton<Base>
{
    private readonly SoundManager sound;
    private readonly UIManager ui;
    private readonly SettingData setting;

    public partial class Manager
    {
        public static SoundManager Sound => instance.sound;
        public static UIManager UI => instance.ui;
    }

    public class Data
    {
        public static SettingData Setting => instance.setting;
    }

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
