using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClassToReturnObject : MonoBehaviour
    {
        private static ClassToReturnObject s_Instance;
        public static ClassToReturnObject Singleton => s_Instance;

        public Func<GameObject> ObjectGetter;
        public GameObject obj;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
        }

        private void OnEnable()
        {
            ObjectGetter += Getter;
        }

        private GameObject Getter()
        {
            return obj;
        }
    }
}