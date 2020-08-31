
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBtnItem : MonoBehaviour
{
    public bool isLocked = true;
    public void OnClick()
    {
        if (!isLocked)
        {
            GameSounds.PlayOneShot("lip");
            SceneManager.LoadScene(gameObject.name);
        }
    }
}
