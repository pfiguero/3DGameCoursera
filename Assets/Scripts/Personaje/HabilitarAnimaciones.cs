using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HabilitarAnimaciones : MonoBehaviour {

    public Animator animator;

    void Start() {

        animator = this.GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name.Equals("pantallaIntro"))
        {
            animator.SetBool("pantallaIntro", true);
        }
        else
        {
            animator.SetBool("pantallaIntro", false);
        }

    }
}
