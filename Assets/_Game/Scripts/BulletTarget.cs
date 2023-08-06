using System.Linq;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{
    public float force = 5f;

    private Enemy[] enemy;
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
        enemy = FindObjectsOfType<Enemy>();
        for (var i = 0; i < enemy.Count(); i++)
        {
            if (enemy[i] is null) return;
            if (player is null) return;
            targetpos = enemy[i].transform.position;
            var direction = targetpos - player.transform.position;
            rb.velocity = new Vector2(direction.x, direction.y) * force;
        }
    }
}