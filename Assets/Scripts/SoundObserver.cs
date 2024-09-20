using UnityEngine;

public class SoundObserver : MonoBehaviour, IObserver
{
    [SerializeField] private AudioClip _fireSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioSource _audioSource;

    public void OnNotify(string eventType)
    {
        if (eventType == "WeaponFired")
        {
            PlaySound(_fireSound);
        }
        else if (eventType == "WeaponReloaded")
        {
            PlaySound(_reloadSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (_audioSource != null && clip != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }

    private void Start()
    {
        WeaponStateMachine weaponStateMachine = FindObjectOfType<WeaponStateMachine>();
        if (weaponStateMachine != null)
        {
            weaponStateMachine.RegisterObserver(this);
        }
    }

    private void OnDestroy()
    {
        WeaponStateMachine weaponStateMachine = FindObjectOfType<WeaponStateMachine>();
        if (weaponStateMachine != null)
        {
            weaponStateMachine.RemoveObserver(this);
        }
    }
}
