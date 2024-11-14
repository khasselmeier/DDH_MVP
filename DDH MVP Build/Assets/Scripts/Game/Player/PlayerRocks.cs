using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocks : MonoBehaviour
{
    [Header("Rock Stats")]
    public int damage;
    public int curAmmo;
    public int maxAmmo;
    public float bulletSpeed;
    public float shootRate;

    private float lastShootTime;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    private PlayerBehavior player;

    void Awake()
    {
        //get required components
        player = GetComponent<PlayerBehavior>();
    }

    public void TryShoot()
    {
        //Debug.Log("Try Throwing Rock");

        //can we throw rocks?
        if (curAmmo <= 0 || Time.time - lastShootTime < shootRate)
            return;

        //reduce ammo
        curAmmo--;
        lastShootTime = Time.time;

        //update ammo UI
        GameUI.instance.UpdateAmmoText();

        //spawn the bullet
        SpawnBullet(bulletSpawnPos.position, Camera.main.transform.forward);
    }

    void SpawnBullet(Vector3 pos, Vector3 dir)
    {
        //Debug.Log("Spawn Rock");

        //spawn and orientate it
        GameObject bulletObj = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bulletObj.transform.forward = dir;

        //get the bullet script
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        //intialize it and set the velocity
        bulletScript.Initialize();
        bulletScript.rig.velocity = dir * bulletSpeed;
    }

    public void GiveAmmo(int ammoToGive)
    {
        curAmmo = Mathf.Clamp(curAmmo + ammoToGive, 0, maxAmmo);

        //update ammo UI
        GameUI.instance.UpdateAmmoText();
    }
}
