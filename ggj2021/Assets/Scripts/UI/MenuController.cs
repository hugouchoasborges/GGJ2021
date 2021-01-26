/*
 * Created by Hugo Uchoas Borges <hugouchoas@outlook.com>
 */

using sound;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

using Language = LocalisationSystem.Language;

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
            GameDebug.Log("play".Localised());
        }

        public void ExitGame()
        {
            GameDebug.Log($"Exiting Game");
            if (Util.IsEditor())
            {
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }

            if (Util.IsWebGL())
            {
                return;
            }

            Application.Quit();
        }

        // ----------------------------------------------------------------------------------
        // ========================== Language Settings ============================
        // ----------------------------------------------------------------------------------


        [SerializeField]
        private TMP_Dropdown languageDropdown;

        public Language selectedLanguage
        {
            get => LocalisationSystem.language;
            set
            {
                LocalisationSystem.language = value;

                // Update the Dropdown selector
                languageDropdown.value = (int)value;

                // Updating all localised texts
                System.Object[] localisedStrings = Resources.FindObjectsOfTypeAll(typeof(TextLocaliserUI));

                foreach (var str in localisedStrings)
                {
                    var text = ((TextLocaliserUI)str).gameObject.GetComponent<TextMeshProUGUI>();
                    text.text = ((TextLocaliserUI)str).localisedString.value;
                }
            }
        }

        public void OnChangeLaguage(int value)
        {
            selectedLanguage = (Language)value;
            SaveSettings();
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
            PlayerPrefs.SetInt(_languageKey, (int)selectedLanguage);
        }

        private void LoadSettings()
        {
            selectedLanguage = (Language)PlayerPrefs.GetInt(_languageKey);
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
