using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeEditor : MonoBehaviour
{
    private const int MinVolume = -80;
    private const int VolumeModifier = 20;
    private const string NamePostfix = "Volume";

    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] private Slider _slider;
    [SerializeField] [CanBeNull] private Toggle _toggle;

    private float _volume = 0;
    private bool _isSoundOn = true;

    private void Start()
    {
        _mixerGroup.audioMixer.GetFloat(_mixerGroup.name + NamePostfix, out _volume);
        _slider.value = (float)Math.Pow(10, _volume / VolumeModifier);
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(ChangeVolume);
        _toggle?.onValueChanged.AddListener(MuteVolume);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(ChangeVolume);
        _toggle?.onValueChanged.RemoveListener(MuteVolume);
    }

    private void ChangeVolume(float volume)
    {
        var calculatedVolume = ConvertVolume(volume);

        if (_isSoundOn)
            _mixerGroup.audioMixer.SetFloat(_mixerGroup.name + NamePostfix, calculatedVolume);

        _volume = calculatedVolume;
    }

    private void MuteVolume(bool isSoundOn)
    {
        if (isSoundOn)
            _mixerGroup.audioMixer.SetFloat(_mixerGroup.name + NamePostfix, _volume);
        else
            _mixerGroup.audioMixer.SetFloat(_mixerGroup.name + NamePostfix, MinVolume);

        _isSoundOn = isSoundOn;
    }

    private float ConvertVolume(float volume) =>
        Mathf.Log10(volume) * VolumeModifier;
}