using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private float transitionTime = 1;

    #region SINGLETON
    public static SceneLoader instance = null;
    private void Awake() {
        if (!instance) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        #endregion
        //Awake
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            LoadLevel("Battle");
        }
    }

    public void LoadLevel(string levelName) {
        StartCoroutine(Load(levelName));
    }

    private IEnumerator Load(string levelName) {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }
}
