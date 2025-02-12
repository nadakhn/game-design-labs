using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject spawnItem; //rn its just coin
    public int maxHits =-1;
    public Sprite emptyBlock;
    private bool isAnimating;

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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        maxHits --;
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


 
}
