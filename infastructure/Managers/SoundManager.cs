using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Infrastructure.Managers
{
    public class SoundManager : GameService, ISoundManager

    {
        private readonly Dictionary<string, SoundEffect> m_soundEffectDict = new Dictionary<string, SoundEffect>();
        private readonly Dictionary<string, SoundEffect> m_BackRoundSongDict = new Dictionary<string, SoundEffect>();
        private float m_BackgroundMusicVolume = 1;
        private float m_SoundsEffectsVolume = 1;
        private bool m_ToggleSound = true;
        private SoundEffectInstance m_BGEffect;
        private const float k_Pitch = 0;
        private const float k_Pan = 0;

        public event EventHandler<EventArgs> BackgroundMusicVolumeChanged;
        public event EventHandler<EventArgs> SoundsEffectsVolumeChanged;
        public event EventHandler<EventArgs> ToggleSoundChanged;

        public SoundManager(Game i_Game) : base(i_Game)
        {
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISoundManager), this);
        }

        public float BackgroundMusicVolume
        {
            get
            {
                return m_BackgroundMusicVolume;
            }
            set
            {
                m_BackgroundMusicVolume = value;
                m_BGEffect.Volume = m_BackgroundMusicVolume;
                if (BackgroundMusicVolumeChanged != null)
                {
                    BackgroundMusicVolumeChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void PlaySoundEffect(string i_Name) 
        {
            if (m_ToggleSound)
            {
                m_soundEffectDict[i_Name].Play(m_SoundsEffectsVolume, k_Pitch, k_Pan);
            }
        }

        public void AddSoundEffect(string i_Name, SoundEffect i_SoundEffect)
        {
            m_soundEffectDict.Add(i_Name, i_SoundEffect);
        }

        public void PlayBackRoundSong()
        {
            m_BGEffect.IsLooped = true;
            m_BGEffect.Pan = k_Pan;
            m_BGEffect.Pitch = k_Pitch;
            m_BGEffect.Volume = m_BackgroundMusicVolume;
            m_BGEffect.Play();
        }

        public void UnMuteBackRoundSONG() 
        {
            m_BGEffect.Resume();
        }

        public void MuteBackRoundSong()
        {
            if (!m_ToggleSound && m_BGEffect != null)
            {
                m_BGEffect.Pause();
            }
        }

        public void AddBackRoundSong(string i_Name, SoundEffect i_SoundEffect)
        {
            m_BackRoundSongDict.Add(i_Name, i_SoundEffect);
            m_BGEffect = m_BackRoundSongDict[i_Name].CreateInstance();
        }

        public float SoundsEffectsVolume
        {
            get
            {
                return m_SoundsEffectsVolume;
            }
            set
            {
                m_SoundsEffectsVolume = value;
                if (SoundsEffectsVolumeChanged != null)
                {
                    SoundsEffectsVolumeChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool ToggleSound
        {
            get 
            { 
                return m_ToggleSound; 
            }
            set
            {
                m_ToggleSound = value;
                if (!m_ToggleSound)
                {
                    MuteBackRoundSong();
                }
                else
                {
                    UnMuteBackRoundSONG();
                }

                if (ToggleSoundChanged != null)
                {
                    ToggleSoundChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }
       
        
    }
}
