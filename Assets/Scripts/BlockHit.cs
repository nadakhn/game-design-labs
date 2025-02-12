using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject spawnItem; //rn its just coin
    public int maxHits =-1;
    public Sprite emptyBlock;
    public Sprite blinkingBlock;
    private Sprite defaultBlock;
    private bool isAnimating;
    private bool isBlinking;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultBlock = spriteRenderer.sprite;

        if (blinkingBlock != null)
        {
            isBlinking = true;
            StartCoroutine(Blink());
        }  
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAnimating && maxHits!=0 && collision.gameObject.CompareTag("Player"))
        {
            //check if the collision is from top or bottom
            if (Vector2.Dot((collision.transform.position - transform.position).normalized, Vector2.up) < -0.25f)
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxHits --;
        if (isBlinking)
        {
            isBlinking = false;
            StopCoroutine(Blink());
        }

        if (maxHits ==0) 
        {
            spriteRenderer.sprite = emptyBlock;
        }

        StartCoroutine(Animate());

        if (spawnItem != null)
        {
            Instantiate(spawnItem, transform.position, Quaternion.identity);
        }

    }

    private IEnumerator Animate()
    {
        isAnimating= true;

        Vector3 defaultPosition = transform.localPosition;
        Vector3 animatedPosition = defaultPosition + Vector3.up * 0.5f;

        yield return Move(defaultPosition, animatedPosition);
        yield return Move(animatedPosition, defaultPosition);

        isAnimating= false;

    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed =0;
        float duration = 0.125f;

        while (elapsed<duration)
        {
            float t = elapsed/duration;
            transform.localPosition = Vector3.Lerp(from, to ,t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;

    }

    private IEnumerator Blink()
    {
        while (isBlinking && maxHits!=0 )
        {
            spriteRenderer.sprite = blinkingBlock;
            yield return new WaitForSeconds(0.25f);

            spriteRenderer.sprite = defaultBlock;
            yield return new WaitForSeconds(0.25f);
        }
    }


 
}
