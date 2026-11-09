using System.Collections.Generic;
using UnityEngine;

namespace ChameleonGame
{
    public enum Animals 
    {
        Octopus,
        Snake,
        Ocelot,
        Mantis,
        Raven,
        Wolf,
        Fox
    }

    public struct AnimalStat
    {
        public float moveSpeed;
        public float footStepLoudness;
        public float visibility;
        public float hearDistance;
        public float seeDistance;

        public AnimalStat(float speed, float loud, float vis, float hear, float see)
        {
            moveSpeed = speed;
            footStepLoudness = loud;
            visibility = vis;
            hearDistance = hear;
            seeDistance = see;
        }
    }

    public static class AnimalData
    {
        public static readonly Dictionary<Animals, AnimalStat> Stats = new Dictionary<Animals, AnimalStat>
        {
            { Animals.Octopus, new AnimalStat(10f, 1f, 1f, 3f, 3f) },
            { Animals.Snake,   new AnimalStat(5f, 1f, 1, 3,15) },
            { Animals.Ocelot,  new AnimalStat(5f, 0.2f, 0.2f, 3f, 3f) },
            { Animals.Mantis,  new AnimalStat(5f, 0.1f, 0.1f, 3f, 3f) },
            { Animals.Raven,   new AnimalStat(5f, 0.1f, 0.9f, 3f, 3f) },
            { Animals.Wolf,    new AnimalStat(5f, 0.8f, 0.8f, 3f, 3f) },
            { Animals.Fox,    new AnimalStat(1f, 0.1f, 0.8f, 2f, 1f) },

        };
    }
}
