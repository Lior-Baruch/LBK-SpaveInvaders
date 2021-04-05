using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.ServiceInterfaces
{
     public interface ISoundManager
    {
        public event EventHandler<EventArgs> BackgroundMusicVolumeChanged;

        public event EventHandler<EventArgs> SoundsEffectsVolumeChanged;

        public event EventHandler<EventArgs> ToggleSoundChanged;

        float BackgroundMusicVolume { get; set; }

        float SoundsEffectsVolume { get; set; }

        bool ToggleSound { get; set; }


    }
}
