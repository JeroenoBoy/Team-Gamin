using UnityEngine;

namespace Util
{
    /// <summary>
    /// Your simple and everyday singleton class
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance { get; private set; }


        protected virtual void OnEnable()
        {
            if (!instance || instance == this) instance = this as T;
            else
            {
                Destroy(this);
                Debug.LogError("Instance already exists!");
            }
        }


        protected virtual void OnDisable()
        {
            if(instance == this) instance = null;
        }
    }
}