using System.Collections.Generic;

using GeometrySynth.Constants;

namespace GeometrySynth.Control
{
    public class SynthScene
    {
        public SynthScene()
        {
            sceneNodes = new List<SceneNode>();
        }
        private List<SceneNode> sceneNodes;
    }
}