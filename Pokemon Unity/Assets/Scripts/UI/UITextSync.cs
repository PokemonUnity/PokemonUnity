using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UITextSync : MonoBehaviour
{
    [Description(@"Syncs the text values between two Text Mesh Pro components

If empty, defaults to the first childs Text Mesh Pro component (if it has one)")]
    [SerializeField] TMPro.TextMeshProUGUI parentText;
    TMPro.TextMeshProUGUI shadowText;

    // Start is called before the first frame update
    void Start()
    {
        shadowText = GetComponent<TMPro.TextMeshProUGUI>();
        if (parentText == null) parentText = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        shadowText.text = parentText.text;
    }
}
