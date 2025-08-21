using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game.PowerUp
{
    public class Shield : PowerUpEffect
    {
        [SerializeField] private GameObject shieldPrefab;
        private GameObject shield;

        protected override void OnActivate()
        {
            CreateAndActivateShield();
        }

        protected override void OnDeactivate()
        {
            if (shield != null)
            {
                shield.SetActive(false);
                Destroy(shield);
            }
        }

        private void CreateAndActivateShield()
        {
            if (shieldPrefab == null) shieldPrefab = GameManager.Instance.Data.shieldPrefab;
            if (shieldPrefab != null)
            {
                shield = Instantiate(shieldPrefab, transform);
                shield.transform.localPosition = Vector3.zero;
                shield.tag = "Shield";
                shield.SetActive(true);
            }
        }
    }
}