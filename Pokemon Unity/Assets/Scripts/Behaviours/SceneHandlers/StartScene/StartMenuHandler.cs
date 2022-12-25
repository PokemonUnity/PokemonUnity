using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class StartMenuHandler : MonoBehaviour
{
    [SerializeField] GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        bool hasSaveFile = SaveLoad.Load();
        Selectable selectable = continueButton.GetComponent<Button>();
        if (!hasSaveFile) {
            for (int i = 0; i < Selectable.allSelectablesArray.Length; i++) {
                if (Selectable.allSelectablesArray[i] == selectable) {
                    if (i == Selectable.allSelectablesArray.Length - 1) {
                        selectable = Selectable.allSelectablesArray[i-1];
                        break;
                    } else {
                        selectable = Selectable.allSelectablesArray[i+1];
                        break;
                    }
                }
            }
            continueButton.SetActive(false);
        }
        selectable.Select();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
