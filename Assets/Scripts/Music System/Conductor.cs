using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Conductor : MonoBehaviour
{
    #region dont touch this
    private static Conductor _instance;
    public static Conductor Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Conductor is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public AudioSource drums;
    public AudioSource bass;
    public AudioSource piano;
    public AudioSource guitarH;
    public AudioSource guitarM;
    

    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    //[SerializeField] public Intervals[] _intervals;
    public List<Intervals> _intervals = new List<Intervals>();

    private void Update()
    {
        foreach(Intervals interval in _intervals.ToArray())
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }

    public void RemoveInterval(Intervals interval, GameObject gameObject)
    {
        _intervals.Remove(interval);
        Destroy(gameObject);
    }

}

[System.Serializable]
public class Intervals
{
    [SerializeField] public float _steps;
    [SerializeField] public UnityEvent _trigger;
    private int _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    //checks if we crossed a new beat
    public void CheckForNewInterval(float interval)
    {
        if(Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
