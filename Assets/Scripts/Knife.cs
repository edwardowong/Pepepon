using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 7f;

    private int minDmg, maxDmg;
    private Rigidbody2D rb;
    private TextMesh damageTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        string weapon = PlayerPrefs.GetString("Weapon");                                                // Change weapon based on selection
        if (weapon == "Knife" || !PlayerPrefs.HasKey("Weapon"))
        {
            minDmg = 2;
            maxDmg = 5;
        }
        else if (weapon == "Fireball")
        {
            minDmg = 5;
            maxDmg = 7;
        }
        rb = gameObject.GetComponent<Rigidbody2D>();
        damageTextPrefab = Resources.Load("damageIndicator").GetComponent<TextMesh>();
        rb.velocity = transform.right * speed;
        Object.Destroy(gameObject, 1.5f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Block"))
        {
            int damageDealt = Random.Range(minDmg, maxDmg);
            collision.gameObject.GetComponent<EnemyController>().Damage(damageDealt);
            TextMesh textObject = Instantiate(damageTextPrefab, gameObject.transform.position + new Vector3(0, 4, 0), Quaternion.identity);
            textObject.text = damageDealt.ToString();
            Object.Destroy(gameObject, 0.05f);
            Object.Destroy(textObject.gameObject, 1.5f);
        }
    }
}
