using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UITextSync : MonoBehaviour
{
    [Description("Defaults to this GameObjects TextMeshPro (if available)")]
    public TMPro.TextMeshProUGUI sourceText;
    public bool syncText = true;
    public bool syncFontSize = true;
    [Description(@"Syncs the text values between two Text Mesh Pro components
    
If empty, defaults to the first childs Text Mesh Pro component (if it has one)")]
    public bool syncMargin = true;
    [Space()]
    public List<TMPro.TextMeshProUGUI> textsToSync;

    // Start is called before the first frame update
    void Start()
    {
        if (sourceText is null) sourceText = GetComponent<TMPro.TextMeshProUGUI>();
        if (sourceText is null) Debug.LogError("No TextMeshProUGUI provided or found");
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceText is null) return;
        foreach (TMPro.TextMeshProUGUI text in textsToSync) {
            if (syncText) text.text = sourceText.text;
            if (syncFontSize) text.fontSize = sourceText.fontSize;
            if (syncMargin) text.margin = sourceText.margin;
        }
    }
}
