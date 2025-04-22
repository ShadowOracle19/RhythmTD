using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibratorManager : MonoBehaviour
{
    public GameObject audioCalibratorVisuals;
    public GameObject inputCalibratorVisuals;

    public AudioSource testSound;
    private List<float> inputTiming = new List<float>();

    private float lastSoundTime;

    public float audioLatencyOffset;
    public float inputLatencyOffset;

    [SerializeField]private bool currentlyCalibrating = false;

    public GameObject visualCue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && currentlyCalibrating)
        {
            float inputTime = Time.realtimeSinceStartup;
            inputTiming.Add(inputTime - lastSoundTime);
        }
    }

    public void StartCalibrator()
    {
        StartCoroutine(PlayAudioCalibratorSounds());
    }

    IEnumerator PlayAudioCalibratorSounds()
    {
        currentlyCalibrating=true;
        inputTiming.Clear();
        while (inputTiming.Count < 5)
        {
            lastSoundTime = Time.realtimeSinceStartup;
            testSound.Play();
            yield return new WaitForSeconds(0.75f);
        }
        CalculateAudioOffsetAverage(inputTiming);
    }

    public void CalculateAudioOffsetAverage(List<float> _inputTiming)
    {
        float total = 0;

        foreach (var timing in _inputTiming)
        {
            total += timing;
        }
        audioLatencyOffset = total / _inputTiming.Count;
        Debug.Log(audioLatencyOffset);

        StartCoroutine(InputLatencyCalibrator());

        GameManager.Instance.audioOffset = audioLatencyOffset;
    }

    IEnumerator InputLatencyCalibrator()
    {
        inputTiming.Clear();
        visualCue.SetActive(false);
        while (inputTiming.Count < 5)
        {
            visualCue.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            visualCue.SetActive(false);

            lastSoundTime = Time.realtimeSinceStartup;

            yield return new WaitForSeconds(0.75f - 0.1f);
        }

        CalculateInputOffsetAverage(inputTiming);
    }

    public void CalculateInputOffsetAverage(List<float> _inputTiming)
    {
        float total = 0;

        foreach (var timing in _inputTiming)
        {
            total += timing;
        }
        inputLatencyOffset = total / _inputTiming.Count;
        Debug.Log(inputLatencyOffset);

        currentlyCalibrating = false;
        GameManager.Instance.inputOffset = inputLatencyOffset;

        EndCalibration();
    }

    public void EndCalibration()
    {

    }
}
