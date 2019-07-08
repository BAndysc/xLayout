using System;
using TMPro;
using UnityEngine;

namespace xLayout.Example
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmPro;

        public GameObject obj;

        private void Update()
        {
            tmPro.SetText(DateTime.Now.ToString());
        }
    }
}

