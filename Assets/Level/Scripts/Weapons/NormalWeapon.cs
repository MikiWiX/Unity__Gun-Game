using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    public class NormalWeapon : BasicWeapon
    {
        protected override void ShootSpawn(Vector2 bulletSpawnPos, Quaternion bulletRotation)
        {
            Bullet localBullet = LevelTools.SpawnWithScript<Bullet>(bullet, bulletSpawnPos, bulletRotation);
            localBullet.speed = 50;
            localBullet.timeLeft = 0.5f;
        }

    }
}

