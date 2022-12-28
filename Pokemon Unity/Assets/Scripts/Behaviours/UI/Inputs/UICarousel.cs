using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UICarousel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TargetText;
    int savedIndex;
    public int ActiveIndex = 0;
    public List<string> CarouselItems;
    public UnityEvent<string> OnChange;

    // Start is called before the first frame update
    void Start()
    {
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        savedIndex = ActiveIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetText is null) return;
        if (ActiveIndex != savedIndex) {
            savedIndex = ActiveIndex;
            OnChange.Invoke(CarouselItems[ActiveIndex]);
        }
        TargetText.text = CarouselItems[ActiveIndex];
    }

}
