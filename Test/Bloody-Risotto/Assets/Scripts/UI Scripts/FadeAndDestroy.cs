using System.Collections;
using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    SpriteRenderer render;
    float delay = 1f;
    bool isDelayed = false;
    float ft = 1f;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!isDelayed)
        {
            delay -= Time.deltaTime;
        }

        if (delay <= 0f)
        {
            isDelayed = true;
        }
        StartCoroutine(FadeOut());
        if (render.color.a <= 0f)
        {
            Destroy(gameObject, 1f);
        }
    }

    private IEnumerator FadeOut()
    {
        Color c = render.color;
        if (isDelayed)
        {
            while (ft >= -0.01f)
            {
                c.a = ft;
                render.color = c;
                ft -= 0.005f;
                yield return new WaitForSeconds(.1f);
            }
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }
    }
}