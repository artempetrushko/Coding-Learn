using System.Collections;
using System.Collections.Generic;
using EmeraldAI;
using UnityEngine;

public enum ShootMode
{
    Auto,
    Queue,
    None,
    Single
}
public class Weapon : MonoBehaviour
{
    [SerializeField] private EmeraldAIAbility _bullet;
    [SerializeField] private int _magazineSize;
    [SerializeField] private float _magazineReloadTime;
    [SerializeField] private float _weaponKd;
    [SerializeField] private Transform _shootPoint;
    public Transform TargetPoint;
    [SerializeField] private float _queueSize;
    public ShootMode ShootMode;
    

    private float _totalMagazineSize;

    private float _totalReloadTime;
    private float _totalQueueSize;
    private bool _isShooting;
    private float _reloadTimer;
    private float _kdTimer;

    public EmeraldAISystem AISystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartShooting()
    {
        _isShooting = true;
    }

    public void StopShooting()
    {
        if (ShootMode == ShootMode.Auto)
        {
            _isShooting = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_isShooting)
        {
            _shootPoint.LookAt(TargetPoint);
            var bullet=Instantiate(_bullet.AbilityEffect,_shootPoint.position,Quaternion.identity);
            bullet.GetComponent<Bullet>().StartMovement(TargetPoint,_shootPoint);
            if (AISystem)
            {
                bullet.GetComponent<Bullet>().AiSystem=AISystem;
            }
            
            _isShooting = false;
        }
        
    }
}
