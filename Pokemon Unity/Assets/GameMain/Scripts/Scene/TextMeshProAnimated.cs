using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using TMPro;
using UnityEngine;
using UnityEngine.Events;


public enum Emotion { happy, sad, suprised, angry };
[System.Serializable] public class ColorEvent : UnityEvent<PokemonUnity.Colors> { }

[System.Serializable] public class EmotionEvent : UnityEvent<Emotion> { }

[System.Serializable] public class ActionEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable] public class TextRevealEvent : UnityEngine.Events.UnityEvent<char> { }

[System.Serializable] public class DialogueEvent : UnityEngine.Events.UnityEvent { }

public class TMP_Animated //: TextMeshProUGUI
{
	/*
	//[SerializeField] private float speed = 10;
	[SerializeField] private byte speed { get { return PokemonUnity.Game.textSpeed; } set { PokemonUnity.Game.textSpeed = value; } }
	//ToDo: While button is held down, text speed is increased (fast-forward)
	private static float secPerChar
	{
		get
		{
			//return 1f / speed;
			int txtSpd = PokemonUnity.Game.textSpeed + 1;
			return 1 / (16 + (txtSpd * txtSpd * 9));
		}
	}
	//ToDo: if button is pressed, text dialog skips to the end
	private bool InstantLine;
	public ColorEvent onColorChange;
	public EmotionEvent onEmotionChange;
	public ActionEvent onAction;
	public TextRevealEvent onTextReveal;
	public DialogueEvent onDialogueFinish;

	public void ReadText(string newText)
	{
		text = string.Empty;
		// split the whole text into parts based off the <> tags 
		// even numbers in the array are text, odd numbers are tags
		string[] subTexts = newText.Split('<', '>');

		// textmeshpro still needs to parse its built-in tags, so we only include noncustom tags
		string displayText = "";
		for (int i = 0; i < subTexts.Length; i++)
		{
			if (i % 2 == 0)
				displayText += subTexts[i];
			else if (!isCustomTag(subTexts[i].Replace(" ", "")))
				displayText += $"<{subTexts[i]}>";
		}
		// check to see if a tag is our own
		bool isCustomTag(string tag)
		{
			return tag.StartsWith("speed=") || tag.StartsWith("pause=") || tag.StartsWith("emotion=") || tag.StartsWith("color=") || tag.StartsWith("colorend") || tag.StartsWith("action");
		}

		// send that string to textmeshpro and hide all of it, then start reading
		text = displayText;
		InstantLine = false;
		maxVisibleCharacters = 0;
		StartCoroutine(Read());

		IEnumerator Read()
		{
			int subCounter = 0;
			int visibleCounter = 0;
			while (subCounter < subTexts.Length)
			{
				InstantLine = false;
				//ToDo: if PARA => new dialog entry; reset maxvisible back to zero, start from next array entry
				//ToDo: Split `string` into new text bubble here
				// if custom tag
				if (subCounter % 2 == 1)
				{
					yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
				}
				else
				{
					//ToDo: if `SkipAhead()` set maxvisiblechar to reveal entire string.
					while (visibleCounter < subTexts[subCounter].Length)
					{
						//ToDo: if LINE => line break; reset maxvisible from two lines of text to one, start text on new line
						onTextReveal.Invoke(subTexts[subCounter][visibleCounter]);
						visibleCounter++;
						maxVisibleCharacters++;
						if (!InstantLine) 
							yield return new WaitForSeconds(secPerChar);
					}
					visibleCounter = 0;
					InstantLine = false;
				}
				subCounter++;
			}
			yield return null;

			WaitForSeconds EvaluateTag(string tag)
			{
				if (tag.Length > 0)
				{
					if (tag.StartsWith("speed="))
					{
						//speed = float.Parse(tag.Split('=')[1]);
					}
					else if (tag.StartsWith("pause="))
					{
						return new WaitForSeconds(float.Parse(tag.Split('=')[1]));
					}
					else if (tag.StartsWith("emotion="))
					{
						onEmotionChange.Invoke((Emotion)System.Enum.Parse(typeof(Emotion), tag.Split('=')[1]));
					}
					else if (tag.StartsWith("action="))
					{
						onAction.Invoke(tag.Split('=')[1]);
					}
					else if (tag.StartsWith("keyword="))
					{
						onColorChange.Invoke((PokemonUnity.Color)int.Parse(tag.Split('=')[1]));
					}
				}
				return null;
			}
			onDialogueFinish.Invoke();
		}
	}

	public void SkipAhead()
	{
		InstantLine = true;
	}*/
}

/*// <summary>
/// </summary>
/// https://deltadreamgames.com/unity-tmp-hyperlinks/
[RequireComponent(typeof(TextMeshProUGUI))]
public class Hyperlinks : MonoBehaviour, UnityEngine.EventSystems.IPointerClickHandler
{
	public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink((TMP_Text)pTextMeshPro, Input.mousePosition, pCamera);
		if (linkIndex != -1)
		{ // was a link clicked?
			TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

			//if link starts with http
			if(linkInfo.GetLinkID().StartsWith("http"))
				// open the link id as a url, which is the metadata we added in the text field
				Application.OpenURL(linkInfo.GetLinkID());
		}
	}
	List<Color32[]> SetLinkToColor(int linkIndex, Color32 color)
	{
		TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];

		var oldVertColors = new List<Color32[]>(); // store the old character colors

		for (int i = 0; i < linkInfo.linkTextLength; i++)
		{ // for each character in the link string
			int characterIndex = linkInfo.linkTextfirstCharacterIndex + i; // the character index into the entire text
			var charInfo = pTextMeshPro.textInfo.characterInfo[characterIndex];
			int meshIndex = charInfo.materialReferenceIndex; // Get the index of the material / sub text object used by this character.
			int vertexIndex = charInfo.vertexIndex; // Get the index of the first vertex of this character.

			Color32[] vertexColors = pTextMeshPro.textInfo.meshInfo[meshIndex].colors32; // the colors for this character
			oldVertColors.Add(vertexColors.ToArray());

			if (charInfo.isVisible)
			{
				vertexColors[vertexIndex + 0] = color;
				vertexColors[vertexIndex + 1] = color;
				vertexColors[vertexIndex + 2] = color;
				vertexColors[vertexIndex + 3] = color;
			}
		}

		// Update Geometry
		pTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

		return oldVertColors;
	}
}*/

//Pokemon Script Tags
//Wait: Delay in seconds...
//Prompt: Gives you option to select a reply
//Line: Line break; pushes text on to new line, while continuing flow of dialog
//Cont: Similar to `Line`, but pushes the first line of text off screen to continue flow of dialog 
//Para: Resets the text box with a new paragraph entry
//TextRam: Loads text from game's ram via command
//PlaySound
//@: `Player presses button to continue`
//#MON: shortcut for "Pokemon"
//<PLAYER>:
//Pause: Predetermined amount of time to wait before continuing flow of dialog
//Done: exits dialog and returns game event handler back over to script in progress