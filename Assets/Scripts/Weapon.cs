using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject weaponPrefab;

    // Start is called before the first frame update
    void Start()
    {
        string weapon = PlayerPrefs.GetString("Weapon");
        if (weapon == "Knife" || !PlayerPrefs.HasKey("Weapon"))
        {
            weaponPrefab = Resources.Load("knife") as GameObject;

        }
        else if (weapon == "Fireball")
        {
            weaponPrefab = Resources.Load("fireball") as GameObject;
        }
    }

    public void Shoot()
    {
        GameObject newObj = Instantiate(weaponPrefab, spawnPoint.position, spawnPoint.rotation);
    }

}
