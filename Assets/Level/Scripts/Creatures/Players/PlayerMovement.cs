using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;
using System;

namespace Characters
{
    public class PlayerMovement : CreatureMotion
    {

        new void OnEnable()
        {   
            PlayerManager.setPlayer(gameObject);
            base.OnEnable();
        }

        void Update()
        {
            if (checkJump()) { base.Jump(); }
            if (checkDropDown()) { base.DropDown(); }
        }

        new private void FixedUpdate()
        {
            base.updateMovementDirection(checkMovementXAxis());

            base.FixedUpdate();

        }

        private bool checkJump()
        {
            if(Input.GetButtonDown("Jump") ||
                SC_MobileControls.instance.GetMobileButtonDown("Jump"))
            {
                return true;
            }
            return false;
        }
        private float checkMovementXAxis()
        {
            float a = Input.GetAxisRaw("Horizontal");
            float b = SC_MobileControls.instance.GetJoystick("JoystickLeft").x;
            return Math.Max(-1 , Math.Min(a+b , 1));
        }
        private bool checkDropDown()
        {
            if (Input.GetAxisRaw("Vertical") == -1 ||
                SC_MobileControls.instance.GetJoystick("JoystickLeft").y < -0.5f)
            {
                return true;
            }
            return false;
        }

    }
}
