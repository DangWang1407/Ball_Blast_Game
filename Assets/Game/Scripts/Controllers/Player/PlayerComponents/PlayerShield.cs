using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class PlayerShield : MonoBehaviour
    {
        [SerializeField] private GameObject shieldPrefab;
        public GameObject Shield { get; set; }

        private PlayerController playerController;
        public void Initialize(PlayerController playerController)
        {
            this.playerController = playerController;
            CreateShield();
        }

        public void CreateShield()
        {
            Debug.Log("Shield is created");
            Shield = Instantiate(shieldPrefab);
            Shield.transform.SetParent(transform);
            Shield.transform.localPosition = Vector3.zero;
            Shield.tag = "Shield";
            Shield.SetActive(false);
        }

        public void ActivateShield(float duration)
        {
            Debug.Log("Shield is activated");
            StartCoroutine(ShieldCoroutine(duration));
        }

        private IEnumerator ShieldCoroutine(float duration)
        {
            if (Shield == null) yield break;

            Shield.SetActive(true);
            yield return new WaitForSeconds(duration);
            Shield.SetActive(false);
        }
    }
}