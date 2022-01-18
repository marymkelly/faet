using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timing_Functions : MonoBehaviour
{
    public float timeStart = 60;
    public float testStart = 60;
    public Text textBox;
    public Text testBox;

    private float updateCount = 0;
    private float fixedUpdateCount = 0;
    private float updateUpdateCountPerSecond;
    private float updateFixedUpdateCountPerSecond;

    bool timerActive = false;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = timeStart.ToString();
        // StartCoroutine(Loop());
    }

    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        textBox.text = Mathf.Round(timeStart).ToString();

        // updateCount += 1;
        // testBox.text = "Update: " + updateCount + ", Fixed Update: " + fixedUpdateCount;
    }

    void FixedUpdate()
    {
        testStart -= Time.fixedDeltaTime;
        testBox.text = Mathf.Round(testStart).ToString();

        // fixedUpdateCount += 1;
    }

    public void timerButton() {
        timerActive = !timerActive;
    }

    IEnumerator Loop() {
        while (true)
        {
            yield return new WaitForSeconds(1);
            updateUpdateCountPerSecond = updateCount;
            updateFixedUpdateCountPerSecond = fixedUpdateCount;

            updateCount = 0;
            fixedUpdateCount = 0;
        }
    }
}
