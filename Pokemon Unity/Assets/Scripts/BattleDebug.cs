using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BattleDebug : MonoBehaviour
{
    public void sendStartBattle()
    {
        Debug.Log("Starting Wild Battle");
        GameObject.Find("Player").SendMessage("startWildBattle");
    }

    public void sendWithdrawToBall()
    {
        Debug.Log("Withdraw Follower");
        GameObject.Find("Follower").SendMessage("withdrawToBall");
    }

    public void sendReleaseFromBall()
    {
        Debug.Log("Release Follower");
        GameObject.Find("Follower").SendMessage("releaseFromBall");
    }

    public void jump()
    {
        GameObject.Find("Player").SendMessage("playerJump");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BattleDebug))]
class BattleDebugEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var battleDebug = (BattleDebug) target;
        if (GUILayout.Button("Start Wild Battle"))
        {
            battleDebug.sendStartBattle();
        }
        if (GUILayout.Button("Withdraw Follower"))
        {
            battleDebug.sendWithdrawToBall();
        }
        if (GUILayout.Button("Release Follower"))
        {
            battleDebug.sendReleaseFromBall();
        }
        if (GUILayout.Button("Player Jump"))
        {
            battleDebug.jump();
        }
        
    }
}
#endif