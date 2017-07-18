using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Multimeter : EletricEquipment {

    [SerializeField] private EletricConnection highConnection;
    [SerializeField] private EletricConnection groundConnection;
    [SerializeField] private Text display;

    private void OnEnable()
    {
        StartCoroutine(UpdateDisplay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateDisplay()
    {
        while (enabled)
        {
            float displayVal = 0f;
            if (!highConnection.isAvailable && !groundConnection.isAvailable)
            {
                displayVal = highConnection.voltage - groundConnection.voltage;
            }

            display.text = string.Format("{0:f2}", displayVal);
            yield return new WaitForSeconds(.1f);
        }
    }
}
