using System;
using UnityEngine;

namespace Game.Scripts.Utils
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }


        protected virtual void Awake()
        {
            if (!instance) instance = this as T;
            else
            {
                Destroy(this);
                Debug.LogError("Instance already exists!");
            }
        }


        private void OnDestroy()
        {
            if(instance == this) instance = null;
        }
    }
}