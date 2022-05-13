using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LV
{
    public class UIHandler : MonoBehaviour
    {
        public Image[] images;
        private Text text;
        private Text energyText;
        public float energyPercent = 100;
        public float barPercent = 0;

        private void Awake()
        {
            energyText = images[3].GetComponentInChildren<Text>();
        }

        public void StartTaskBars()
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (i != 3)
                {
                    text = images[i].GetComponentInChildren<Text>();
                    images[i].fillAmount = 0.5f;
                    barPercent = images[i].fillAmount * 100;
                    text.text = Math.Round(barPercent, 1) + " %";
                }               
            }
           
        }

        public void HandleTaskBar(int idImage)
        {

            if (energyPercent > 0)
            {
                if (images[idImage].fillAmount < 1)
                {
                    text = images[idImage].GetComponentInChildren<Text>();

                    if (idImage == 2)
                    {
                        images[idImage].fillAmount += 0.010f;
                        images[3].fillAmount += 0.0001f;
                    }
                    else
                    {
                        images[idImage].fillAmount += 0.001f;
                        images[3].fillAmount -= 0.001f / 2;
                    }

                    barPercent = images[idImage].fillAmount * 100;
                    text.text = Math.Round(barPercent, 1) + " %";

                    energyPercent = images[3].fillAmount * 100;
                    energyText.text = Math.Round(energyPercent, 1) + " %";
                }
            }
        }
    }
}