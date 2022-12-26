using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UITextSync : MonoBehaviour
{
    TMPro.TextMeshProUGUI sourceText;
    
    [SerializeField] bool syncText = true;
    [SerializeField] bool syncFontSize = true;
    [Description(@"Syncs the text values between two Text Mesh Pro components
    
If empty, defaults to the first childs Text Mesh Pro component (if it has one)")]
    [SerializeField] bool syncMargin = true;
    [Space()]
    [SerializeField] List<TMPro.TextMeshProUGUI> textsToSync;

    // Start is called before the first frame update
    void Start()
    {
        sourceText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (TMPro.TextMeshProUGUI text in textsToSync) {
            if (syncText) text.text = sourceText.text;
            if (syncFontSize) text.fontSize = sourceText.fontSize;
            if (syncMargin) text.margin = sourceText.margin;
        }
    }
}
