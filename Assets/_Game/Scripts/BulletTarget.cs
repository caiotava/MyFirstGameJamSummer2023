using System.Linq;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    public float force = 5f;

    private Player player;
    private Rigidbody2D rb;
    private Vector3 targetpos;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = FindAnyObjectByType<Player>();
    }

    public void Update()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            if (player is null) return;
            targetpos = enemy.transform.position;
            var direction = targetpos - player?.transform?.position;
            rb.velocity = new Vector2(direction.Value.x, direction.Value.y) * force;
        }
    }
}