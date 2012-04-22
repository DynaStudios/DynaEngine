using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;

namespace DynaStudios.IO
{
    public class SoundManager
    {

        private ISoundEngine _soundEngine;
        private Dictionary<string, ISoundSource> _loadedSounds;

        private Engine _engine;

        public SoundManager(Engine engine)
        {
            _engine = engine;

            _soundEngine = new ISoundEngine();
            _loadedSounds = new Dictionary<string, ISoundSource>();
        }

        public void Play2D(string soundName)
        {
            if (!_loadedSounds.ContainsKey(soundName))
            {
                throw new Exception("Sound is not loaded!");
            }
            _soundEngine.Play2D(_loadedSounds[soundName], false, false, false);
        }

        public void Play2D(string soundName, bool loop)
        {
            if (!_loadedSounds.ContainsKey(soundName))
            {
                throw new Exception("Sound is not loaded!");
            }
            _soundEngine.Play2D(_loadedSounds[soundName], loop, false, false);
        }

        public void AddSound(string soundName, string filepath)
        {
            if (!_loadedSounds.ContainsKey(soundName)) {
                _loadedSounds.Add(soundName, _soundEngine.AddSoundSourceFromFile(filepath));
            }
        }

        public void UnloadSound(string soundName)
        {
            if (_loadedSounds.ContainsKey(soundName))
            {
                _loadedSounds.Remove(soundName);
            }
        }
    }
}
