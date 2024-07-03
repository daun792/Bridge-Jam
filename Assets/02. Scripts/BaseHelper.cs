using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    protected static T _instance = null;

    /// <summary>
    /// �̱����� �����Ǿ��ִ��� Ȯ���ϰ� �����Ǿ������� ����.
    /// �����Ǿ� ���� ������ ���� �� ����.
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