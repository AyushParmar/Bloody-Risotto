using System.Collections;
using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    SpriteRenderer render;
    [SerializeField]float delay = 1f;
    float ft = 1f;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        InvokeRepeating("FadeOut",delay,0.005f);
    }
    void Update()
    {
        if (render.color.a <= 0f)
        {
            Destroy(gameObject, 1f);
        }
    }

    private void FadeOut()
    {
        Color c = render.color;
        c.a = ft;
        render.color = c;
        ft -= 0.005f;
    }
}