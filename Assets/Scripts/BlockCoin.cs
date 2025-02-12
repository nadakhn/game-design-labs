using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    public AudioClip coinSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: to add code that updates game manager on mario's count coin

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 defaultPosition = transform.localPosition;
        Vector3 animatedPosition = defaultPosition + Vector3.up * 2f;

        yield return Move(defaultPosition, animatedPosition);
        yield return Move(animatedPosition, defaultPosition);

        AudioSource.PlayClipAtPoint(coinSound, transform.position);
        Destroy(gameObject);

    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0;
        float duration = 0.25f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;

    }


}
