using Moon.Asyncs.Internal;
using UnityEngine;

namespace Moon.Asyncs
{
    public static class AudioExtensions
    {
        public static AsyncState PlayOneShotAsync(this AudioSource source, AudioClip clip, float volume = 1f)
        {
            return Planner.Chain().AddCommand(new AsyncCommandAudio(source, clip, volume));
        }
    }
}
