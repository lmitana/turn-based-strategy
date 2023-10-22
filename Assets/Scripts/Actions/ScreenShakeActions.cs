using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turnbased.Actions
{
    public class ScreenShakeActions : MonoBehaviour
    {
        void Start()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
            GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
        }

        void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.Instance.Shake();
        }

        private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(5f);
        }
    }
}