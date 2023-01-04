using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSingleton : MonoBehaviour
{
    static PlayerInput singleton;
    
    public static PlayerInput Singleton { get => singleton; }

    void Awake()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        if (singleton is null) singleton = input;
        else Destroy(this);
    }

    public class PlayerInputNotFoundError : Exception {
        public PlayerInputNotFoundError() {
            Debug.LogError("No PlayerInputSingleton found");
        }
    }
}
