using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private Animator anim;

    public static event Action<Vector2> Movement;
    public static event Action Dash;
    float hMovement, vMovement = 0;

    private bool animTriggered = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        moveInput();
        dashInput();
        attackInput();
        shootInput();
        stabInput();
        quitInput();
    }

    private void moveInput()
    {
        float nextHValue = animTriggered? 0 : Input.GetAxis("Horizontal");
        float nextVValue = animTriggered? 0 : Input.GetAxis("Vertical");

        if (nextHValue == hMovement && nextVValue == vMovement)
            return;
        hMovement = nextHValue;
        vMovement = nextVValue;
        Movement?.Invoke(new Vector2(hMovement, vMovement).normalized);
    }

    private void dashInput()
    {
        if (!animTriggered && Input.GetButtonDown("Dash"))
            Dash?.Invoke();
    }

    private void attackInput()
    {
        if (animTriggered || !Input.GetButtonDown("Attack"))
            return;
        anim.SetBool("Attack", true);
        animTriggered = true;
    }

    private void shootInput()
    {
        if (!animTriggered && Input.GetButtonDown("Shoot"))
        {
            animTriggered = true;
            anim.SetTrigger("Shoot");
        }
        if (!animTriggered && Input.GetButtonDown("TwoWayShot"))
        {
            animTriggered = true;
            anim.SetTrigger("TwoWayShot");
        }
    }

    private void stabInput()
    {
        if (animTriggered || !Input.GetButtonDown("Stab"))
            return;
        animTriggered = true;
        anim.SetTrigger("Stab");
    }

    public void StopAttacking()
    {
        anim.SetBool("Attack", false);
        animTriggered = false;
    }

    public void DisableTriggered()
    {
        animTriggered = false;
    }

    private void quitInput()
    {
        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
