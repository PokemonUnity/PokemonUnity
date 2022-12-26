using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextSingleSelect : MonoBehaviour
{
    [SerializeField] GameObject textInputPrefab;
    [Description(@"Spawns a prefab for each choice provided. No layout is applied. Apply a layout component to this GameObject to organize their position.
Looks for TextMeshProUGUI components in any GameObject in the prefabs hierarchy.
If possible, sets the Navigation property on each instantiated prefab.")]
    [SerializeField] Navigation.Mode Navigation;
    [SerializeField] List<string> choices;

    void Start()
    {
        if (choices.Count == 0) Debug.LogError("No selection choices provided");
        if (textInputPrefab == null) Debug.LogError("No text prefab provided");
        for (int i = 0; i < choices.Count; i++) {
            var choicePrefab = Instantiate(textInputPrefab);
            choicePrefab.transform.SetParent(transform);
            choicePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            Selectable selectable = choicePrefab.transform.FindFirst<Selectable>();
            if (selectable is not null) {
                Navigation nav = selectable.navigation;
                nav.mode = Navigation;
            }
            TMPro.TextMeshProUGUI text = choicePrefab.transform.FindFirst<TMPro.TextMeshProUGUI>();
            text.text = choices[i];
        }
    }
}
