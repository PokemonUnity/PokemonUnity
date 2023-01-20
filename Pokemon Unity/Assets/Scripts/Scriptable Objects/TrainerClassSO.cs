using MarkupAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trainer Class", menuName = "Pokemon Unity/Trainer Class")]
public class TrainerClassSO : ScriptableObject
{
    public string ClassName;
    public Sprite VsScreen;
    public int BasePrizeMoney;

    //private static string[] classString = new string[] {
    //    "Trainer",
    //    "Ace Trainer",
    //    "Youngster",
    //    "Rival",
    //    "Team Neo-Rocket Grunt",
    //    "Bug Catcher",
    //    "Champion",
    //    "Hiker",
    //    "Lass",
    //    "Fisherman",
    //    "Gym Leader"
    //};

    [Foldout("Audio", box: false)]
    public AudioTrack SpottedAudioTrack;
    public AudioTrack BattleBackgroundMusic;
    public AudioTrack VictoryBackgroundMusic;
    public AudioTrack LowHpBackgroundMusic;
    [EndGroup("Audio")]

    public List<SpriteAnimation> Animations;
}
