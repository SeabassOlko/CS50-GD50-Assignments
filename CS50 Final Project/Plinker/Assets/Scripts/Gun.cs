using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform camera;

    public AudioSource GunShot;
    public AudioSource ReloadSound;

    public GameObject reticle;

    public GameObject GameCont;
    private GameController GameController;
    public ParticleSystem muzzleFlash;

    public LayerMask Target, HeadTarget, Hostage;

    TargetController TargetHit;

    public Animator animator;
    
    public int CurrentAmmo = 10;

    private int MaxAmmo = 10;

    private float ReloadTime = 1.5f;
    private float RecoilTime = 0.35f;
    private bool IsReloading = false;
    private bool IsShooting = false;

    // aim down sights
    private bool ADS = false;

    private float AimDownSightSpeed = 1.7f;

    private float maxDistance = 100f;

    private void Awake() 
    {
        GameController = GameCont.GetComponent<GameController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsReloading || IsShooting)
            return;

        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetMouseButtonDown(1) && ADS == false)
        {
            StartCoroutine(StartAimDownSights());
        }
        if (Input.GetMouseButtonUp(1) && ADS == true)
        {
            StartCoroutine(StopAimDownSights());
        }
    }

    IEnumerator Shoot()
    {
        IsShooting = true;
        muzzleFlash.Play();
        GunShot.Play();
        CurrentAmmo--;

        if (ADS)
            animator.SetBool("ADSRecoil", true);
        else
            animator.SetBool("Recoil", true);


        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, Hostage))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.Hostage();
            }
        }
        else if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, HeadTarget))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.HeadShot();
            }
        }
        else if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, Target))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.Hit();
            }
        }

        yield return new WaitForSeconds(RecoilTime);

        if (ADS)
            animator.SetBool("ADSRecoil", false);
        else
            animator.SetBool("Recoil", false);

        IsShooting = false;
    }

    IEnumerator Reload()
    {

        IsReloading = true;
        ReloadSound.Play();

        if (ADS)
            animator.SetBool("ADSReload", true);
        else
            animator.SetBool("Reload", true);

        yield return new WaitForSeconds(ReloadTime);

        if (ADS)
            animator.SetBool("ADSReload", false);
        else
            animator.SetBool("Reload", false);

        CurrentAmmo = MaxAmmo;
        IsReloading = false;

    }

    IEnumerator StartAimDownSights()
    {
        ADS = true;
        reticle.SetActive(false);
        animator.SetBool("AimDownSights", true);

        yield return new WaitForSeconds(AimDownSightSpeed);

        animator.SetBool("ADSIdle", true);

    }

    IEnumerator StopAimDownSights()
    {
        ADS = false;

        animator.SetBool("ADSIdle", false);

        animator.SetBool("AimDownSights", false);

        yield return new WaitForSeconds(AimDownSightSpeed);

        reticle.SetActive(true);
    }
}
