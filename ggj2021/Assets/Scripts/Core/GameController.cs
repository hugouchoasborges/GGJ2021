using player;
using sound;
using UnityEngine;
using UnityEngine.Events;
using camera;

namespace ui
{

    public class GameController : Singleton<GameController>
    {
        [Header("Components")]
        public PlayerController playerController;
        public UIController uiController;
        public CameraController camController;
        public CutsceneController cutsceneController;

        [Header("Colored Objects")]
        public Material defaultMaterial;
        [SerializeField] private SpriteRenderer[] _purpleObjects;
        [SerializeField] private SpriteRenderer[] _redObjects;
        [SerializeField] private SpriteRenderer[] _blueObjects;

        private void Start()
        {
            ConfigureGame();
            Init();
        }


        // ----------------------------------------------------------------------------------
        // ========================== INIT ============================
        // ----------------------------------------------------------------------------------


        private void ConfigureGame()
        {

        }

        private void Init()
        {
            // InitBGAudio
            SoundController.Play("crowlie_firstforest", true, false);
        }


        // ----------------------------------------------------------------------------------
        // ========================== GameFlow ============================
        // ----------------------------------------------------------------------------------

        public void SetPlayerState(PlayerController.PlayerState playerState)
        {
            playerController.SetState(playerState);
        }
        public void SetPlayerState(string state)
        {
            System.Enum.TryParse(state, out PlayerController.PlayerState playerState);
            SetPlayerState(playerState);
        }


        // ========================== Remove GrayScale ============================

        private void RemoveGrayScale(SpriteRenderer[] spriteRendererList)
        {
            foreach (var item in spriteRendererList)
            {
                item.material = defaultMaterial;
            }
        }

        public void RemoveGrayScalePurple() => RemoveGrayScale(_purpleObjects);
        public void RemoveGrayScaleRed() => RemoveGrayScale(_redObjects);
        public void RemoveGrayScaleBlue() => RemoveGrayScale(_blueObjects);


        // ----------------------------------------------------------------------------------
        // ========================== Development Test Cases ============================
        // ----------------------------------------------------------------------------------

        [System.Serializable]
        class KeyTestEntry
        {
            public KeyCode key;
            public UnityEvent events;
        }

        [Header("Development Test Case")]
        [SerializeField] private bool enableTestCase = false;
        [SerializeField] private KeyTestEntry[] keyTests;

        private void TEST_CheckInput()
        {
            if (keyTests == null)
                return;

            foreach (KeyTestEntry entry in keyTests)
                if (Input.GetKeyDown(entry.key))
                    entry.events?.Invoke();
        }


        // ========================== Update ============================

        private void Update()
        {
            if (enableTestCase)
            {
                TEST_CheckInput();
            }
        }
    }
}