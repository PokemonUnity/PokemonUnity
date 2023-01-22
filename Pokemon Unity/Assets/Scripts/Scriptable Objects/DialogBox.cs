using MarkupAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialog Box", menuName = "Pokemon Unity/UI/Dialog Box")]
public class DialogBox : ScriptableObject {
    //public static DialogBox Singleton;
    [TitleGroup("General", contentBox: true)]
    [SerializeField] 
    new string name;
    [Description("The root GameObject of the prefab should have a DialogBoxBehaviour component")]
    public DialogueBoxBehaviour Prefab;
    [TitleGroup("Frame Sprite", contentBox: true)]
    [HideIf("useSettingFrame")]
    public Sprite Frame;
    [ShowIf("useSettingFrame")]
    public GameSetting<Sprite> FrameSetting;
    [Description("The spawned dialog box will prioritize using Frame Setting over Frame")]
    [SerializeField]
    bool useSettingFrame = false;
    [EndGroup]
    [Space]

    [TitleGroup("Text", contentBox: true)]
    public Color TextColor;
    public Color TextShadowColor;

    public string Name { get => name; }
}