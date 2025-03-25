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

    // ȿ���� ��� �Լ� �߰�
    public void PlaySfx(SFX sfx)
    {
        int index = (int)sfx; // Enum�� ������ �迭 �ε����� ��ȯ

        if (index >= 0 && index < sfxSource.Length)
        {
            sfxAudioSource.PlayOneShot(sfxSource[index]);
        }
        else
        {
            Debug.LogWarning("SFX �ε��� ���� �ʰ�: " + sfx);
        }
    }
}

