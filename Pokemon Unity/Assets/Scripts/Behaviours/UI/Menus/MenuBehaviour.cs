using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] string menuName = "A menu";
    public bool isFirstMenu = false;
    Canvas canvas;

    public string MenuName { get => menuName; }

    protected void Start() {
        MenuHandler.Singleton.Menus.Add(this);
        if (isFirstMenu) MenuHandler.Singleton.ChangeMenuInstantly(menuName);
        canvas = GetComponent<Canvas>();
        if (!isFirstMenu) gameObject.SetActive(false);
    }

    protected void OnDestroy() {
        MenuHandler.Singleton.Menus.Remove(this);
    }

    public virtual Selectable GetFirstSelectable() {
        return canvas.transform.FindFirst<Selectable>();
    }

    public void SelectFirstSelectable() {
        Selectable selectable = GetFirstSelectable();
        if (selectable == null) {
            Debug.LogError("Could not find a selectable in the Canvas " + canvas.gameObject.name);
            return;
        }
        AudioSourcesToggleMute(selectable.gameObject, true);
        selectable.Select(); // selecting is just navigating
        AudioSourcesToggleMute(selectable.gameObject, false);
    }

    void AudioSourcesToggleMute(GameObject gameObject, bool? value = null) {
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        foreach (var source in sources) {
            if (value.HasValue)
                source.mute = value.Value;
            else
                source.mute = !source.mute;
        }
    }

    public void ChangeMenu(string name) => MenuHandler.Singleton.ChangeMenu(name);
}
