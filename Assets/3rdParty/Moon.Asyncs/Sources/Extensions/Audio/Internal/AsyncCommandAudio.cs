using Moon.Asyncs.Internal.Commands;
using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncCommandAudio: AsyncCommand
    {
        private readonly AudioSource _source;
        private readonly AudioClip _clip;
        private readonly float _volume;

        public AsyncCommandAudio(AudioSource source, AudioClip clip, float volume)
        {
            _source = source;
            _clip = clip;
            _volume = volume;
        }

        protected override AsyncState CallStart()
        {
            _source.PlayOneShot(_clip, _volume);
            return new AsyncStateAudioSource(_source);
        }


    }
}
