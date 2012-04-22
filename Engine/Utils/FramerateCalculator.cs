using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DynaStudios.Utils
{
    public class FramerateCalculator
    {
        private long _currentTime;
        private int _frameCount;
        private long _lastTime;

        //Private Vars
        private Stopwatch _watch;

        public FramerateCalculator()
        {
            _watch = new Stopwatch();
            _watch.Start();
        }

        //Public Vars
        public int Framerate { get; set; }

        public void CalculateFramePerSecond()
        {
            _frameCount++;

            _currentTime = _watch.ElapsedMilliseconds;

            long timeDifference = _currentTime - _lastTime;

            //If more then 1 Second difference: Recalculate fps
            if (timeDifference > 1000 && _lastTime != 0)
            {
                Framerate = (int)(_frameCount / (timeDifference / 1000.0f));

                _lastTime = _currentTime;

                _frameCount = 0;
            }
            else
            {
                if (_lastTime == 0)
                {
                    _lastTime = _currentTime;
                }
            }

        }
    }
}