using UnityEngine;
using UnityEngine.Video;

namespace MockARFoundation.Internal
{
    public class VideoMockCamera : IMockCamera
    {
        public Texture texture => player.texture;
        public bool isPrepared => player.isPrepared;

        private readonly VideoPlayer player;

        public VideoMockCamera(string videoPath)
        {
            var gameObject = new GameObject(typeof(VideoMockCamera).ToString());
            Object.DontDestroyOnLoad(gameObject);

            player = gameObject.AddComponent<VideoPlayer>();

            player.source = VideoSource.Url;
            player.url = "file://" + videoPath;
            player.playOnAwake = true;
            player.isLooping = true;
            player.skipOnDrop = true;
            player.renderMode = VideoRenderMode.APIOnly;
            player.audioOutputMode = VideoAudioOutputMode.None;
            player.SetDirectAudioMute(0, true);
            player.playbackSpeed = 1;
        }

        public void Dispose()
        {
            player.Stop();
            Object.DestroyImmediate(player.gameObject);
        }
    }
}
