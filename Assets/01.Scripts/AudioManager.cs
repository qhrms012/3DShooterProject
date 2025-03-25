using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip[] sfxSource;
    public enum SFX
    {
        Shot,
        Reload,
        Hit,
        GameOver
    }

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);

    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
    public void SetBgmVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    // 효과음 재생 함수 추가
    public void PlaySfx(SFX sfx)
    {
        int index = (int)sfx; // Enum의 순서를 배열 인덱스로 변환

        if (index >= 0 && index < sfxSource.Length)
        {
            sfxAudioSource.PlayOneShot(sfxSource[index]);
        }
        else
        {
            Debug.LogWarning("SFX 인덱스 범위 초과: " + sfx);
        }
    }
}

