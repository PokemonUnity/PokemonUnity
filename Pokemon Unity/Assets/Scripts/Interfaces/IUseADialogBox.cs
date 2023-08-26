using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Intended for us on a Prefab with a DialogueBoxBehaviour on its parent component</summary>
public interface IUseADialogBox {
    public Image Image { get; }
    public GameSetting<Sprite> FrameSetting { get; }
    public TMPro.TextMeshProUGUI Text { get; }
    public TMPro.TextMeshProUGUI TextShadow { get; }
    public void Initialize(DialogBox dialogBox);
}
