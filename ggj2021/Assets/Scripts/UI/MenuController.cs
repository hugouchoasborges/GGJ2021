/*
 * Created by Hugo Uchoas Borges <hugouchoas@outlook.com>
 */

using sound;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using util;

namespace ui
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private AudioListener audioListener;

        [SerializeField] private GameObject[] toDisable;

        [SerializeField] private string menuSound = "crowlie_firstforest";

        // ----------------------------------------------------------------------------------
        // ========================== START ============================
        // ----------------------------------------------------------------------------------

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            SoundController.Play(menuSound, true);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Scene Loading============================
        // ----------------------------------------------------------------------------------

        private void LoadScene(string sceneName)
        {
            GameDebug.Log($"Load Scene: {sceneName}", util.LogType.Scene);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        private void LoadScene(int sceneIndex)
        {
            GameDebug.Log($"Load Scene of Index: {sceneIndex}", util.LogType.Scene);
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }

        // called second
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameDebug.Log("OnSceneLoaded: " + scene.name, util.LogType.Scene);

            // Play Scene loaded
            if (scene.name != "MenuScene")
            {
                _canvas.enabled = false;
                background.gameObject.SetActive(false);
                //StartCoroutine(TransitionBGCoroutine());

                if(toDisable != null)
                    foreach (GameObject item in toDisable)
                        item.SetActive(false);

                audioListener.enabled = false;
            }
        }

        private IEnumerator TransitionBGCoroutine(float duration = 1f)
        {
            Color startColor = new Color(170, 170, 170, 255);
            Color endColor = new Color(170, 170, 170, 0);

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                //right here, you can now use normalizedTime as the third parameter in any Lerp from start to end
                background.color = Color.Lerp(startColor, endColor, normalizedTime);
                yield return null;
            }

            background.gameObject.SetActive(false);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Buttons ============================
        // ----------------------------------------------------------------------------------

        public void PlayGame()
        {
            //LoadScene(playScene);
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ExitGame()
        {
            GameDebug.Log($"Exiting Game", util.LogType.Scene);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            return;
#endif
#if UNITY_WEBGL
            return;
#endif

            Application.Quit();
        }

        // ----------------------------------------------------------------------------------
        // ========================== Enable/Disable ============================
        // ----------------------------------------------------------------------------------

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
