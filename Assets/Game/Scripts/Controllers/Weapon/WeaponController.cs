using Game.Scriptable;
using System.Collections;
using UnityEngine;

namespace Game.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] public WeaponData weaponData;

        //Components
        private WeaponShooting weaponShooting;
        private WeaponPooling weaponPooling;
        private NormalShot normalShot;
        private BurstShot burstShot;
        private DiagonalShot diagonalShot;

        private void Start()
        {
            normalShot = GetComponent<NormalShot>();
            burstShot = GetComponent<BurstShot>();
            diagonalShot = GetComponent<DiagonalShot>();

            normalShot.Initialize();
            burstShot.Initialize();
            diagonalShot.Initialize();


            weaponShooting = GetComponent<WeaponShooting>();
            weaponPooling = GetComponent<WeaponPooling>();

            Debug.Log("weaponShooting = " + weaponShooting);
            Debug.Log("weaponPooling = " + weaponPooling);




            weaponShooting.Initialize(this);
            weaponPooling.Initialize(this);
        }

        private void FixedUpdate()
        {
            weaponShooting.FixedUpdate();
        }

        public IEnumerator ResetAfterDuration(float duration, System.Action resetAction)
        {
            yield return new WaitForSeconds(duration);
            resetAction?.Invoke();
        }
    }
}