using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSingleton : MonoBehaviour
{
    static PlayerInput singleton;

    public static PlayerInput Singleton { get => singleton; }

    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        if (singleton is null) singleton = input;
        else Destroy(this);
    }
}
