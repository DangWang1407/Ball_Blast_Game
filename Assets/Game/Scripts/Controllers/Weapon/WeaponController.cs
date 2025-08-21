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
        private WeaponStats weaponStats;

        private void Start()
        {
            normalShot = GetComponent<NormalShot>();
            normalShot.Initialize();

            weaponShooting = GetComponent<WeaponShooting>();
            weaponPooling = GetComponent<WeaponPooling>();
            weaponStats = GetComponent<WeaponStats>();

            Debug.Log("weaponShooting = " + weaponShooting);
            Debug.Log("weaponPooling = " + weaponPooling);




            weaponShooting.Initialize(this);
            weaponPooling.Initialize(this);
            weaponStats.Initialize(this);
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