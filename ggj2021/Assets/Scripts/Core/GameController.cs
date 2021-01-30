
using sound;

namespace ui
{

    public class GameController : Singleton<GameController>
    {
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
            SoundController.Play("crowlie_firstforest", true);
        }
    }
}