using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TowerTest BulletParent;
    public int remainingPierce;
    public float BulletLifeTime;

    private void Awake()
    {
        BulletParent = gameObject.transform.parent.gameObject.GetComponent<TowerTest>();
        remainingPierce = BulletParent.Pierce;
        BulletLifeTime = BulletParent.BulletLifeTime;

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
        gameObject.transform.SetParent(GameObject.FindWithTag("GameMap").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            remainingPierce--;
            collision.gameObject.GetComponent<Enemy>().TakeDamage(BulletParent.Damage);

            if (remainingPierce <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(BulletLifeTime);
        Destroy(gameObject);
    }
}
