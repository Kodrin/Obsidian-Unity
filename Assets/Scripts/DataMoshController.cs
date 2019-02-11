using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class DataMoshController : MonoBehaviour
{
    [SerializeField] Lasp.FilterType _filterType;
    [SerializeField] float _amplify = 0;

    float _entropy;
    float _noiseContrast;

    void Start()
    {
        var mosh = GetComponent<Datamosh>();

        mosh.entropy = _entropy;

        mosh.noiseContrast = _noiseContrast;
    }

    void Update()
    {
        var rms = Lasp.AudioInput.CalculateRMSDecibel(_filterType) + _amplify;
        var level = 1 + rms * 0.1f;
        level = Mathf.Clamp(level, 0.0f, 1.0f);
        _entropy = level;
    }
}
