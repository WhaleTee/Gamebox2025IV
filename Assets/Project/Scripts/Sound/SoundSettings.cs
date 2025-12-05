using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Reflex.Attributes;
using Misc;
using Extensions;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Sound
{
    public class SoundSettings : Singleton<SoundSettings>
    {
        [SerializeField] private AudioMixer m_audioMixer;
        [SerializeField] private float m_volumeMultiplier = 30f;
        private Dictionary<SoundType, SoundDataContainer> soundData;
        private Dictionary<SoundType, float> previousValues;
        private Dictionary<SoundType, List<SoundType>> childrenMap;
        private CancellationTokenSource tokenSource;
        private bool isSaveEnabled = true;

        public float GetVolume(SoundType type)
        {
            float volume = 0;
            var mixerGroup = soundData[type].MixerGroup;
            if (mixerGroup.audioMixer.GetFloat(VolumeParam(type), out float db))
                return Mathf.Pow(10f, db / m_volumeMultiplier);
            return volume;
        }

        [Inject]
        private void Inject(SoundSettingsInjectionData di)
        {
            childrenMap = new()
            {
                { SoundType.Master, new() { SoundType.Music, SoundType.UI, SoundType.Voice, SoundType.Ambient, SoundType.SFX } },
                { SoundType.SFX, new() { SoundType.Combat, SoundType.Movement, SoundType.Environment } }
            };

            soundData = new();
            soundData.Populate((SoundType[])Enum.GetValues(typeof(SoundType)));
            foreach (var data in di.Datas)
                soundData[data.Type] = data;

            CacheCurrentVolumes();
            Init();
        }

        private void Init()
        {
            foreach (var pair in soundData)
            {
                var data = pair.Value;
                var groupName = data.MixerGroup.name;
                string volumeName = VolumeParam(pair.Key);
                string toggleName = ToggleParam(pair.Key);

                if (PlayerPrefs.HasKey(volumeName))
                    data.Slider.value = PlayerPrefs.GetInt(volumeName, 1000) * 0.001f;
                else
                {
                    SetVolume(data.Slider.value, volumeName);
                    PlayerPrefs.SetFloat(volumeName, Mathf.RoundToInt(data.Slider.value * 1000));
                }

                if (PlayerPrefs.HasKey(toggleName))
                    data.Toggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(toggleName) == 1);
            }
        }

        private void HandleSoundToggle(bool flag, SoundType type)
        {
            SetGroupEnabledIterative(type, flag);
        }

        private void HandleVolumeChange(float vol, SoundType type)
        {
            var volumeParameter = VolumeParam(type);
            SetVolume(vol, volumeParameter);
            PlayerPrefs.SetInt(volumeParameter, Convert.ToInt32(vol * 1000));
        }

        private void SetVolume(float value, string volumeParameter)
        {
            value = Mathf.Max(value, 0.0001f);
            m_audioMixer.SetFloat(volumeParameter, Mathf.Log10(value) * m_volumeMultiplier);
        }

        private void SetGroupEnabledIterative(SoundType root, bool enabled)
        {
            foreach (var type in TraverseFrom(root))
                ApplyGroupState(type, enabled);
        }

        private IEnumerable<SoundType> TraverseFrom(SoundType root)
        {
            Stack<SoundType> stack = new();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                PushChildren(current, stack);
            }
        }

        private void PushChildren(SoundType parent, Stack<SoundType> stack)
        {
            if (!childrenMap.TryGetValue(parent, out var children))
                return;

            foreach (var child in childrenMap[parent])
                stack.Push(child);
        }

        private void ApplyGroupState(SoundType type, bool enabled)
        {
            var data = soundData[type];
            var slider = data.Slider;

            SavePreviousValueIfNeeded(type, slider, enabled);
            SetMixerVolume(type, enabled);
            slider.interactable = enabled;

            PlayerPrefs.SetInt(ToggleParam(type), enabled ? 1 : 0);
        }

        private void SavePreviousValueIfNeeded(SoundType type, Slider slider, bool enabled)
        {
            if (!enabled)
                previousValues[type] = slider.value;
        }

        private void SetMixerVolume(SoundType type, bool enabled)
        {
            var data = soundData[type];
            SetVolume(
                enabled ? previousValues[type] : data.Slider.minValue,
                VolumeParam(type)
            );
        }

        public void SetMixerVolume(SoundType type, float volume)
        {
            SetVolume(volume, VolumeParam(type));
        }

        private void CacheCurrentVolumes()
        {
            if (previousValues == null)
            {
                int volumesCount = Enum.GetValues(typeof(SoundType)).Length;
                previousValues = new(volumesCount);
            }
            else
                previousValues.Clear();

            if (soundData != null)
                foreach (var pair in soundData)
                {
                    float value = 0f;
                    if (pair.Value.Slider != null)
                        value = pair.Value.Slider.value;
                    previousValues.Add(pair.Key, value);
                }
            else
            {
                var keys = (SoundType[])Enum.GetValues(typeof(SoundType));
                foreach (var key in keys)
                    previousValues.Add(key, 0.2f);
            }
        }

        private string VolumeParam(SoundType type) =>
            $"{soundData[type].MixerGroup.name}Volume";
        private string ToggleParam(SoundType type) =>
            $"{soundData[type].MixerGroup.name}Toggle";

        private async UniTaskVoid SaveAsync(float interval, CancellationToken token)
        {
            int _interval = Mathf.CeilToInt(interval * 1000);
            while (isSaveEnabled && !token.IsCancellationRequested)
            {
                await UniTask.Delay(_interval, cancellationToken: token);
                PlayerPrefs.Save();
            }
        }

        private void OnEnable()
        {
            CacheCurrentVolumes();

            foreach (var data in soundData.Values)
            {
                var group = data.MixerGroup;
                var groupName = group.name;
                var groupType = data.Type;
                data.Slider.onValueChanged.AddListener(value => HandleVolumeChange(value, groupType));
                data.Toggle.onValueChanged.AddListener(value => HandleSoundToggle(value, groupType));
            }

            tokenSource = new();
            SaveAsync(0.5f, tokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            tokenSource?.Dispose();
            if (soundData == null)
                return;
            foreach (var data in soundData.Values)
            {
                data.Slider.onValueChanged.RemoveAllListeners();
                data.Toggle.onValueChanged.RemoveAllListeners();
            }
        }
    }
}