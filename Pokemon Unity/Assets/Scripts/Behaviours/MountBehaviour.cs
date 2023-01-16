using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MountBehaviour : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] Sprites;

    public Sprite Sprite { get => spriteRenderer.sprite; set => spriteRenderer.sprite = value; }

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateMount(bool enabled, string spriteName = "", EMovementDirection direction = EMovementDirection.Down) {
        spriteRenderer.enabled = enabled;
        if (!spriteRenderer.enabled) {
            Sprites = Resources.LoadAll<Sprite>("PlayerSprites/" + spriteName);
            UpdateDirection(direction);
        }
    }

    [Obsolete("Moved from int based direction to Vector3", false)]
    public void UpdateDirection(EMovementDirection direction = EMovementDirection.Down) {
        if (spriteRenderer.enabled) {
            spriteRenderer.sprite = Sprites[(int)direction];
        }
    }

    public void UpdateDirection(Vector3 direction) {
        if (spriteRenderer.enabled) {
            Debug.LogError("Need to convert Vector3 to int to parse sprite sheets");
            //spriteRenderer.sprite = Sprites[(int)direction];
        }
    }

    public void LoadMount(string mountName) {
        Sprites = Resources.LoadAll<Sprite>("PlayerSprites/" + mountName);
    }

    public IEnumerator StillMount(float speed) {
        Vector3 holdPosition = transform.position;
        float hIncrement = 0f;
        while (hIncrement < 1) {
            hIncrement += (1 / speed) * Time.deltaTime;
            transform.position = holdPosition;
            yield return null;
        }
        transform.position = holdPosition;
    }
}
