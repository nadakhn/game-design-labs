using UnityEngine;

public class Goomba : MonoBehaviour
{

    public delegate void GoombaFlattenedHandler(GameObject goomba);
    public static event GoombaFlattenedHandler OnGoombaFlattened;


    public Sprite flatSprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // Means Mario hit from above
                {
                    Debug.Log("from da top!!");
                    OnGoombaFlattened?.Invoke(gameObject);
                    Flatten();
                }
            }
        }
    }

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite; // when flatten, change to flatsprite
        Destroy(gameObject, 0.5f);
    }

}