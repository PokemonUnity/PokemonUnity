using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] string menuName = "A menu";
    [SerializeField] Canvas canvas;

    public string MenuName { get => menuName; }

    void Start() {
        if (canvas == null) Debug.LogError("No canvas was provided");
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
}
