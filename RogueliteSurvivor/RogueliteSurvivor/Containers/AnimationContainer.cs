using Newtonsoft.Json.Linq;

namespace RogueliteSurvivor.Containers
{
    public class AnimationContainer
    {
        public AnimationContainer() { }
        public int FirstFrame { get; set; }
        public int LastFrame { get; set; }
        public float PlaybackSpeed { get; set; }
        public int NumDirections { get; set; }
        public bool Repeatable { get; set; }

        public static AnimationContainer ToAnimationContainer(JToken animation)
        {
            return !animation.HasValues ? null : new AnimationContainer()
            {
                FirstFrame = (int)animation["firstFrame"],
                LastFrame = (int)animation["lastFrame"],
                PlaybackSpeed = (float)animation["playbackSpeed"],
                NumDirections = (int)animation["numDirections"],
                Repeatable = (bool)animation["repeatable"],
            };
        }
    }
}
