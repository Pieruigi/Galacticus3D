using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Referer<T> : MonoBehaviour
    {
        static List<Referer<T>> referers = new List<Referer<T>>();

        T reference;
        public T Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        // Start is called before the first frame update
        void Awake()
        {
            referers.Add(this);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            referers.Remove(this);
        }

        public static IList<Referer<T>> GetReferers(T reference)
        {
            return referers.FindAll(o => o.reference.Equals(reference));
        }
    }

}
