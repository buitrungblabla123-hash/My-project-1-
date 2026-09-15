using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float duration = 0.25f;

    private float timer = 0f;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / duration;

        transform.localScale = Vector3.Lerp(
            startScale,
            startScale * 2f,
            progress
        );

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}