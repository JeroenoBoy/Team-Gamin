using System;
using UnityEngine;

namespace AI
{
    public class EvilAI : MonoBehaviour
    {
        public newStatPoints statPoints    { get; private set; }
        public BehaviourMenu behaviourMenu { get; private set; }
        public SpawnManager  spawner        { get; private set; }


        private void Awake()
        {
            spawner       = GetComponent<SpawnManager>();
            statPoints    = gameObject.AddComponent<newStatPoints>();
            behaviourMenu = gameObject.AddComponent<BehaviourMenu>();

            statPoints.enabled    = false;
            spawner.statPoints    = statPoints;
            spawner.behaviourMenu = behaviourMenu;
        }


        private void Start()
        {
            this.InitData();
        }



        public void BeforeSpawn()
        {
            this.Randomize();
        }
    }
}