using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.UI
{
    public class CoinUI : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text text;

        string textFormat = "CurrentBalance: {0} - BankBalance: {1}";

        // Start is called before the first frame update
        void Start()
        {
            CoinManager.Instance.OnChange += HandleOnChange;
            text.text = string.Format(textFormat, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnChange(int currentBalance, int bankBalance)
        {
            text.text = string.Format(textFormat, currentBalance, bankBalance);
        }
    }

}
