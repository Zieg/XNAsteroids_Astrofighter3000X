using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids.Helpers
{
        /// <summary>
        /// Provides a single class for random numbers gemeration
        /// </summary>
        public static class RandomGenerator
        {
            #region Fields

            static Random rand;

            #endregion

            #region Public Methods

            /// <summary>
            /// Initialize the random number generator
            /// </summary>
            public static void Init()
            {
                rand = new Random();
            }

            /// <summary>
            /// Returns a non-negative random integer less than maxValue
            /// </summary>
            /// <param name="maxValue">Exclusive maximum value</param>
            /// <returns>A Random number</returns>
            public static int Next(int maxValue)
            {
                return rand.Next(maxValue);
            }

            /// <summary>
            /// Returns a number between 0.0 and 1.0
            /// </summary>
            /// <returns>A Random number</returns>
            public static double NextDouble()
            {
                return rand.NextDouble();
            }

            /// <summary>
            /// Returns a nonnegative random number less than maxValue (exclusive)
            /// </summary>
            /// <param name="maxValue">Exclusive max value</param>
            /// <returns>A Random number</returns>
            public static float NextFloat(float maxValue)
            {
                return (float)rand.NextDouble() * maxValue;
            }

            #endregion

        }

    
}
