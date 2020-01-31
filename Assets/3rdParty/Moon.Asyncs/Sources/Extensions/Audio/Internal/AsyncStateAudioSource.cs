using UnityEngine;

namespace Moon.Asyncs.Internal
{
    internal class AsyncStateAudioSource : AsyncState
    {
        AudioSource _audioSource;

        public AsyncStateAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public override void Terminate()
        {
             _audioSource.Stop();
        }

        public override void Update()
        {
            if (_audioSource != null)
            {
                isComplete = !_audioSource.isPlaying;
            }
            else
            {
                isComplete = true;
            }
        }
    }
}
