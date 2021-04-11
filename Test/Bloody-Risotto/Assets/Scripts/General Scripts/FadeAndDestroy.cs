using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    private void Start()
    {
        if (gameObject.name.Contains("Exclamation"))
        {
            StartCoroutine(FadeOut(gameObject));
        }
    }

    public IEnumerator FadeOut(GameObject toFade)
    {
        SpriteRenderer render;
        render = toFade.GetComponent<SpriteRenderer>();
        float ft = 1f;
        bool pause = true;
        bool destroyCalled = false;

        while (true)
        {
            if(pause)
            {
                pause = false;
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(0.005f);
            Color c = render.color;
            c.a = ft;
            render.color = c;
            ft -= 0.005f;
            if (render.color.a <= 0f && !destroyCalled)
            {
                Destroy(gameObject, 1f);
                destroyCalled = true;
                yield return null ;
            }
            if (destroyCalled)
            {
                yield return null;
            }
        }
    }
}