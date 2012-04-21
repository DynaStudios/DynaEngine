using System;
using System.Collections.Generic;

namespace DynaStudios.Sound
{
    /// <summary>
    /// The SoundManager manages all game sound files and is able to load, unload, play sounds.
    /// </summary>
    public class SoundManager
    {

        private FMOD.System _fmodSystem;
        private FMOD.Channel _fmodChannel;
        private Dictionary<string, FMOD.Sound> _loadedSounds;

        public SoundManager()
        {
            _loadedSounds = new Dictionary<string, FMOD.Sound>();

            FMOD.RESULT result = FMOD.Factory.System_Create(ref _fmodSystem);
            errCheck(result);
            
        }

        /// <summary>
        /// Loads given Sound into Game Engine
        /// </summary>
        /// <param name="filepath">Filepath to the sound file</param>
        /// <param name="name">Unique name for the sound</param>
        public void loadSound(String name, String filepath)
        {
            FMOD.Sound newSound;
            FMOD.RESULT result;
        }

        /// <summary>
        /// Unloads given sound name from Engine
        /// </summary>
        /// <param name="name">Name of the sound</param>
        public void unloadSound(String name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void playSound(String name)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loop"></param>
        public void playSound(String name, bool loop)
        {

        }

        private void initFmod()
        {

        }

        private void errCheck(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                //timer.Stop();
                //MessageBox.Show("FMOD error! " + result + " - " + FMOD.Error.String(result));
                Environment.Exit(-1);
            }
        }

    }
}
