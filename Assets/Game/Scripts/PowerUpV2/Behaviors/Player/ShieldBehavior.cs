using Game.Core;
using UnityEngine;

namespace Game.PowerUpV2
{
    public class ShieldBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject shieldPrefabOverride;
        private GameObject shieldInstance;

        public void Init(GameObject prefabOverride)
        {
            shieldPrefabOverride = prefabOverride;
            EnsureShield();
        }

        private void OnDisable()
        {
            DestroyShield();
        }

        public void EnsureShield()
        {
            if (shieldInstance != null) return;

            GameObject prefab = shieldPrefabOverride;
            if (prefab == null && GameManager.Instance != null)
            {
                prefab = GameManager.Instance.Data != null ? GameManager.Instance.Data.shieldPrefab : null;
            }
            if (prefab == null) return;

            shieldInstance = Object.Instantiate(prefab, transform);
            shieldInstance.transform.localPosition = Vector3.zero;
            shieldInstance.tag = "Shield";
            shieldInstance.SetActive(true);
        }

        public void DestroyShield()
        {
            if (shieldInstance != null)
            {
                shieldInstance.SetActive(false);
                Object.Destroy(shieldInstance);
                shieldInstance = null;
            }
        }
    }
}

