using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    protected static T _instance = null;

    /// <summary>
    /// 싱글톤이 생성되어있는지 확인하고 생성되어있으면 리턴.
    /// 생성되어 있지 않으면 생성 후 리턴.
    /// </summary>
    public static T instance
    {
        get
        {
            _instance ??= new();
            return _instance;
        }
    }
}

public class BaseHelper : MonoBehaviour
{
    private static GameObject _helperObject = null;

    private void Awake()
    {
        if (_helperObject != null)
        {
            DestroyImmediate(_helperObject);
        }

        _helperObject = gameObject;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
}