using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class BasicWeapon : MonoBehaviour
    {
        public GameObject bullet;

        public float shootingRate = 1;
        public bool automatic = true;

        public float localBulletOffsetX = 0;
        public float localBulletOffsetY = 0;

        private Vector3 bulletSpawnOffset;

        private bool triggerIsHeld = false;

        private float shootInterval;
        private float shootCooldown;
        private bool shouldShoot = false;

        protected void OnEnable()
        {
            if(transform.parent == null)
            {
                Debug.Log("Weapon must have a parent in order to shoot!");
            }
            bulletSpawnOffset = new Vector2(localBulletOffsetX, localBulletOffsetX);
        }
        void Start()
        {
            shootInterval = 1 / shootingRate;
        }

        void FixedUpdate()
        {
            // UPDATE TIMER
            if (shootCooldown > 0)
            {
                shootCooldown -= Time.fixedDeltaTime;
            }

            if (automatic)
            {
                if (triggerIsHeld)
                {
                    if (shootCooldown <= 0)
                    {
                        // SHOOT
                        Shoot();
                    }
                }
            } 
            else
            {
                if (shouldShoot)
                {
                    if (shootCooldown <= 0)
                    {
                        // SHOOT
                        Shoot();
                    }
                    // SHOULD NOT SHOOT
                    shouldShoot = false;
                }
            }
        }

        public void Shoot()
        {
            Vector2 bulletSpawnPos = transform.localToWorldMatrix.MultiplyPoint3x4(bulletSpawnOffset);
            Quaternion bulletRotation = bullet.transform.rotation * transform.rotation;
            ShootSpawn(bulletSpawnPos, bulletRotation);
            shootCooldown = shootInterval;
        }

        protected virtual void ShootSpawn(Vector2 bulletSpawnPos, Quaternion bulletRotation)
        {
            
        }

        private Vector2 shootDir;
        private Vector3 reflectY = new Vector3(1, -1, 1);
        public void Rotate(Vector2 rot)
        {
            Rotate(rot.x, rot.y);
        }
        public void Rotate(float xDir, float yDir)
        {
            //if weapon CHANGED direction right-left, mirror it vertically
            if ((shootDir.x >= 0 && xDir < 0) || (shootDir.x < 0 && xDir >= 0))
            {
                gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, reflectY);
            }

            // rotate
            shootDir.x = xDir;
            shootDir.y = yDir;

            gameObject.transform.localPosition = shootDir;
            gameObject.transform.localRotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.right, shootDir.normalized), Vector3.forward);

        }

        public void pressTrigger()
        {
            shouldShoot = true;
        }
        public void holdTrigger()
        {
            triggerIsHeld = true;
        }
        public void releaseTrigger()
        {
            triggerIsHeld = false;
        }

    }
}

