using UnityEngine;

public class FruitObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int type { get; private set; }

    public bool SendedMergeSignal { get; private set; }

    public void Prepare(Sprite sprite, int index, float scale)
    {
        spriteRenderer.sprite = sprite;
        type = index;
        transform.localScale = Vector3.one * scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var fruitObject = other.transform.GetComponent<FruitObject>();
        if (!fruitObject)
            return;

        if (fruitObject.type != type)
            return;

        if (fruitObject.SendedMergeSignal)
            return;

        SendedMergeSignal = true;
        GameManager.Instance.Merge(this, fruitObject);
    }
}