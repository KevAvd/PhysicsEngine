using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics
{
    internal class GameTime
    {
        public double ElapsedSeconds { get; set; }
        public DateTime InitialTime { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GameTime()
        {
            SetInitTime();
        }

        /// <summary>
        /// Set initial time
        /// </summary>
        public void SetInitTime()
        {
            InitialTime = DateTime.Now;
        }

        /// <summary>
        /// Set elapsed seconds
        /// </summary>
        public void SetElapsedSeconds()
        {
            ElapsedSeconds = (DateTime.Now - InitialTime).TotalSeconds;
        }

        /// <summary>
        /// Get number of frames per seconds
        /// </summary>
        /// <returns> frame per seconds </returns>
        public double GetFPS()
        {
            return 1 / ElapsedSeconds;
        }
    }
}
