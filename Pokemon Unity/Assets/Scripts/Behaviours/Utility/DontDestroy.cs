using UnityEngine;

[AddComponentMenu("Pokemon Unity/Dont Destroy On Load")]
public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
