using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/Battle/Battle Environment")]
public class BattleEnvironmentBehaviour : MonoBehaviour
{
    public BattleEnvironment BattleEnvironment;

    [Header("Sprite Renderers")]
    public Image Background;
    public Image PlayerBattleBase;
    public Image EnemyBattleBase;

    void OnValidate() {
        if (BattleEnvironment == null) {
            Debug.LogError("No BattleEnvironment provided", gameObject);
            return;
        }

        if (Background == null) Debug.LogError("No background Image component provided", gameObject);
        else Background.sprite = BattleEnvironment.Background;

        if (Background == null) Debug.LogError("No player battle base Image component provided", gameObject);
        else EnemyBattleBase.sprite = BattleEnvironment.BattleBase;

        if (Background == null) Debug.LogError("No enemy battle base Image component provided", gameObject);
        else PlayerBattleBase.sprite = BattleEnvironment.BattleBase;
    }
}
