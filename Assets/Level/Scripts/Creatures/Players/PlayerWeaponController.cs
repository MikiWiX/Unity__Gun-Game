using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

namespace Characters
{
    public class PlayerWeaponController : MonoBehaviour
    {
        public GameObject weapon;

        private BasicWeapon weaponScript;

        private void OnEnable()
        {
            weaponScript = weapon.GetComponent<BasicWeapon>();
            weaponScript.automatic = true;
        }

        private void FixedUpdate()
        {
            Vector2 dir = checkFireDirection();

            weaponScript.Rotate(dir);

            if (dir.x != 0 || dir.y != 0)
            {
                weaponScript.holdTrigger();
            }
            else
            {
                weaponScript.releaseTrigger();
            }
        }

        private Vector2 checkFireDirection()
        {
            float xDir = Input.GetAxisRaw("ShootDirectionX");
            float yDir = Input.GetAxisRaw("ShootDirectionY");
            if(xDir != 0 || yDir != 0)
            {
                return new Vector2(xDir, yDir);
            }
            return SC_MobileControls.instance.GetJoystick("JoystickRight");
        }
    }
}

