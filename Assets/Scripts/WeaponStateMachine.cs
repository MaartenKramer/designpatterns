using System.Collections.Generic;
using UnityEngine;

public class WeaponStateMachine : MonoBehaviour, ISubject
{

    //Different states for weapon
    private enum WeaponState
    {
        Charged,
        Empty,
        Reloading
    }

    [SerializeField] private WeaponState _currentState = WeaponState.Charged;
    [SerializeField] private float _reloadTime = 2.0f;
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private int _maxBulletHoles = 5;

    private ObjectPool<BulletHole> _bulletHolePool;
    private List<IObserver> _observers = new List<IObserver>();

    private void Start()
    {
        _bulletHolePool = new ObjectPool<BulletHole>(_bulletHolePrefab.GetComponent<BulletHole>(), _maxBulletHoles);
    }

    private void Update()
    {
        InputHandler();
    }


    //Checks for inputs to based on the state
    private void InputHandler()
    {
        if (_currentState == WeaponState.Charged && Input.GetKeyDown(KeyCode.Mouse0))
        {
            FireWeapon();
        }
        else if (_currentState == WeaponState.Empty && Input.GetKeyDown(KeyCode.R))
        {
            StartReloading();
        }
    }

    //Fires weapon
    private void FireWeapon()
    {
        Debug.Log("Weapon fired! Creating a bullet hole...");

        BulletHole bulletHole = _bulletHolePool.GetObject();
        bulletHole.transform.position = transform.position + transform.forward * 2.0f;

        _currentState = WeaponState.Empty;

        NotifyObservers("WeaponFired");
    }


    //Reloads weapon
    private void StartReloading()
    {
        Debug.Log("Reloading...");
        _currentState = WeaponState.Reloading;

        // Notify observers that the weapon is reloading
        NotifyObservers("WeaponReloaded");

        StartCoroutine(ReloadCoroutine());
    }


    private System.Collections.IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(_reloadTime);
        Debug.Log("Reload complete! Weapon is now Charged.");
        _currentState = WeaponState.Charged;
    }

    public void RegisterObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(string eventType)
    {
        foreach (var observer in _observers)
        {
            observer.OnNotify(eventType);
        }
    }
}
