using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    #region End user utilities
    public float animationSpeed = 1f;
    #endregion

    #region UI Variables
    private Image bg;
    private Image topSlide;

    private Text selectedText;

    private Image pokemonIcon;
    private Image pokedexIcon;
    private Image bagIcon;
    private Image trainerIcon;
    private Image saveIcon;
    private Image settingsIcon;

    public Image activeIcon;
    public List<Image> icons;
    public float radius = 65f;
    public float offset = -100f;
    private bool cycle;

    private bool pauseAnim;
    #endregion

    #region Command variables
    private CanvasUIHandler canvasUI;
    private int index;
    public string[] texts = {"Pokémon","Pokédex","Bag","Trainer","Save Game","Settings"};
    #endregion

    void Awake(){
        //Setting up Icons and menu graphics
        canvasUI = GameObject.Find("CanvasUI").GetComponent<CanvasUIHandler>();
        bg = GameObject.Find("PauseMenu").GetComponent<Image>();
        topSlide = bg.transform.Find("TopPaused").GetComponent<Image>();
        index = 0;

        bg.color = new Color(0,0,0,0);
        Debug.Log(bg);
        pokemonIcon = bg.transform.Find("Pokemon").GetComponent<Image>();
        bagIcon = bg.transform.Find("Bag").GetComponent<Image>();
        pokedexIcon = bg.transform.Find("Pokedex").GetComponent<Image>();
        saveIcon = bg.transform.Find("Save").GetComponent<Image>();
        settingsIcon =bg.transform.Find("Settings").GetComponent<Image>();
        trainerIcon=bg.transform.Find("Trainer").GetComponent<Image>();

        selectedText = bg.transform.Find("SelectedText").GetComponent<Text>();

        
        icons.Add(pokemonIcon);
        icons.Add(pokedexIcon);
        icons.Add(bagIcon);
        icons.Add(trainerIcon);
        icons.Add(saveIcon);
        icons.Add(settingsIcon);
        cycle = true;

        //Debug.Log(icons[index]);

        //Setting up the initial icons position in the screen
        for(int i = 0; i<icons.Count; i++){
            icons[i].DOColor(new Color(1f,1f,1f,1f),1f);
        }
        
        //this.gameObject.SetActive(false);

    }

    public void setInactive(){
        this.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(index);
        if (pauseAnim==false)
            updateIconPositions();
            updateSelectedText();
        
        if (index <icons.Count)
            activeIcon = icons[index];

        foreach(Image icon in icons){
            if (activeIcon != icon){
                resizeInactiveIcon(icon);
            }
        }
        if (pauseAnim==false)
            animateActiveIcon(activeIcon);
    }

    public void updateSelectedText(){
            selectedText.text = texts[index];
    }

    public void updateIconPositions(){
        for(int i = 0; i<icons.Count; i++){
            float theta = (2*Mathf.PI /icons.Count) *i;
            float xPos = Mathf.Cos(theta-0.175f*icons.Count*index)+offset;
            float yPos = Mathf.Sin(theta-0.175f*icons.Count*index);
            icons[i].rectTransform.DOAnchorPos(new Vector2(xPos,yPos) * radius,1f/animationSpeed);
        }
    }

    public void checkIndex(){
        if (index <=-1){
            index = icons.Count-1;
        }else if (index>=icons.Count){
            index = 0;
        }
    }

    public void resizeInactiveIcon(Image inactIcon){
        inactIcon.rectTransform.DOScale(new Vector3(1,1,1),1f);
        inactIcon.rectTransform.DORotate(new Vector3(0f,0f,0f),1f);
    }

    public void animateActiveIcon(Image actIcon){

        actIcon.rectTransform.DOScale(new Vector3(1.3f,1.3f,1.3f),1f);
        if (actIcon.rectTransform.eulerAngles.z>14.5f && actIcon.rectTransform.eulerAngles.z<= 15f){
            cycle=true;
        }else if (actIcon.rectTransform.eulerAngles.z>345f && actIcon.rectTransform.eulerAngles.z<=346f){
            cycle=false;
        }
        if (cycle==false){
            actIcon.rectTransform.DORotate(new Vector3(0f,0f,15f),1f);
        }
        if (cycle==true){
            actIcon.rectTransform.DORotate(new Vector3(0f,0f,-15f),1f);
        }


        //Debug.Log(cycle);
    }

    public IEnumerator openAnim(){
        bg.gameObject.SetActive(true);
        pauseAnim=true;
        bg.DOColor(new Color(0f,0f,0f,0.6f),0.5f/animationSpeed);
        selectedText.DOColor(new Color(1f,1f,1f,1f),0.5f/animationSpeed);
        index = 0;
        cycle = true;
        for(int i = 0; i<icons.Count; i++){
            float theta = (2*Mathf.PI /icons.Count) *i;
            float xPos = Mathf.Cos(theta)+offset;
            float yPos = Mathf.Sin(theta);
            icons[i].rectTransform.DOAnchorPos(new Vector2(xPos,yPos),1f/animationSpeed);
            icons[i].DOColor(new Color(1f,1f,1f,1f),1f);
        }
        topSlide.rectTransform.DOAnchorPos(new Vector2(0f,50f),0.5f/animationSpeed);
        pauseAnim=false;
        yield return null;
    }

    public IEnumerator closeAnim(){
        topSlide.rectTransform.DOAnchorPos(new Vector2(0f,160f),0.5f/animationSpeed);
        pauseAnim=true;
        selectedText.DOColor(new Color(1f,1f,1f,0f),0.5f/animationSpeed);
        bg.DOColor(new Color(0f,0f,0f,0.0f),0.5f);
        for(int i = 0; i<icons.Count; i++){
            float theta = (2*Mathf.PI /icons.Count) *i;
            float xPos = Mathf.Cos(theta)+offset;
            float yPos = Mathf.Sin(theta);
            icons[i].rectTransform.DOAnchorPos(new Vector2(xPos+offset*radius,yPos),1f/animationSpeed);
            icons[i].DOColor(new Color(1f,1f,1f,0f),0.5f/animationSpeed);
        }
        yield return new WaitForSeconds(0.5f/animationSpeed);
        bg.gameObject.SetActive(false);
        yield return null;

    }

    public IEnumerator control(){
        this.gameObject.SetActive(true);

        yield return StartCoroutine("openAnim");

        bool running = true;
        while (running){
            if (Input.GetAxisRaw("Vertical") < 0){
                index +=1;
                yield return new WaitForSeconds(0.2f);
            }
            if (Input.GetAxisRaw("Vertical") > 0){
                index -=1;
                yield return new WaitForSeconds(0.2f);
            }
            if (Input.GetAxisRaw("Horizontal") > 0){
                index +=1;
                yield return new WaitForSeconds(0.2f);
            }
            if (Input.GetAxisRaw("Horizontal") < 0){
                index -=1;
                yield return new WaitForSeconds(0.2f);
            }
            checkIndex();

            if (Input.GetButtonDown("Select")){
                if (index==0 && Game.Player.Party.Length>0){

                    StartCoroutine(canvasUI.FadeFromTo(this.gameObject,canvasUI.scenes[2]));
                    while (canvasUI.fading==true){
                        yield return null;
                    }

                }
            }

            if (bg.color.a==0.6f){
                if (Input.GetButton("Start") || Input.GetButton("Back"))
                {
                    running = false;
                }
            }
            
            yield return null;
        }

        yield return StartCoroutine("closeAnim");

    }
}
