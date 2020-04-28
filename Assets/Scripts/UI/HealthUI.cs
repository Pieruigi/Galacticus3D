using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OMTB.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        Image bar;

        Health health;

        // Start is called before the first frame update
        void Start()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            health.OnHealthChanged += HandleOnHealthChanged;
            bar.fillAmount = health.CurrentHealth / health.MaxHealth;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnHealthChanged()
        {
            bar.fillAmount = health.CurrentHealth / health.MaxHealth;
        }
    }

}
