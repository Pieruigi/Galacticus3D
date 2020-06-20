using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OMTB
{
    public class CameraFadeController : MonoBehaviour
    {
        [SerializeField]
        Image image;

        float time = 0.5f;

        static CameraFadeController instance;
        public static CameraFadeController Instance
        {
            get { return instance; }
        }

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        /**
         * Fades the camera in
         * A callback can be used to report caller on complete
         * */
        public void FadeIn(UnityAction callback = null)
        {
            StartCoroutine(FadeInCoroutine(callback));
        }

        /**
         * Fades the camera out
         * A callback can be used to report caller on complete
         * */
        public void FadeOut(UnityAction callback = null)
        {
            StartCoroutine(FadeOutCoroutine(callback));
        }

        /**
         * Fades the camera out, waits 'lenght' seconds and then fades in
         * A callback can be used to report caller on complete
         * */
        public void FadeOutIn(float length, UnityAction callback)
        {
            StartCoroutine(FadeOutInCoroutine(length, callback));
        }

        public IEnumerator FadeInCoroutine(UnityAction callback = null)
        {
            Color c = Color.black;
            c.a = 0;
            LeanTween.color((RectTransform)image.transform, c, time);

            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public IEnumerator FadeOutCoroutine(UnityAction callback = null)
        {
            Color c = Color.black;
            c.a = 1;
            LeanTween.color((RectTransform)image.transform, c, time);

            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }

        public IEnumerator FadeOutInCoroutine(float length, UnityAction callback)
        {
            yield return FadeOutCoroutine();

            yield return new WaitForSeconds(length);

            yield return FadeInCoroutine(callback);
        }

    }
}

