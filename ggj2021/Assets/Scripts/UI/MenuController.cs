/*
 * Created by Hugo Uchoas Borges <hugouchoas@outlook.com>
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace ui
{
    public class MenuController : MonoBehaviour
    {
        // ----------------------------------------------------------------------------------
        // ========================== START ============================
        // ----------------------------------------------------------------------------------

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            LoadSettings();
        }

        // ----------------------------------------------------------------------------------
        // ========================== Buttons ============================
        // ----------------------------------------------------------------------------------

        public void PlayGame()
        {
            LoadScene("GameScene");
        }

        public void ExitGame()
        {
            GameDebug.Log($"Exiting Game");

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
        // ========================== Save/Load ============================
        // ----------------------------------------------------------------------------------

        [Header("Player Prefs (Load/Save)")]
        [Space]
        [SerializeField]
        private string _languageKey = "language";

        private void SaveSettings()
        {
            //PlayerPrefs.SetInt(_languageKey, (int)selectedLanguage);
        }

        private void LoadSettings()
        {
            //selectedLanguage = (Language)PlayerPrefs.GetInt(_languageKey);
        }

        // ----------------------------------------------------------------------------------
        // ========================== Auxiliar Methods ============================
        // ----------------------------------------------------------------------------------

        private void LoadScene(string sceneName)
        {
            GameDebug.Log($"Load Scene: {sceneName}", util.LogType.Scene);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        // called second
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameDebug.Log("OnSceneLoaded: " + scene.name, util.LogType.Scene);
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
