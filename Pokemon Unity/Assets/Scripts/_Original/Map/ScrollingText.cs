using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public string text;
    public float speedPerChar = 0.3f;
    public int textMaxWidth;

    private TextMeshPro textMeshPro;
    
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        StartCoroutine(scroll());
    }

    public IEnumerator scroll()
    {
        while (true)
        {
            for (int i = 0; i < text.Length; ++i)
            {
                char c = text[i];

                textMeshPro.text = c + textMeshPro.text;

                yield return new WaitForSeconds(speedPerChar);
            }
            
            
        }
    } 
}
