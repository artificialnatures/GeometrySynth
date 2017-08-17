using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GeometrySynth.Constants;
using GeometrySynth.Interfaces;
using GeometrySynth.FunctionModules;

namespace GeometrySynth.Control
{
    public class SceneController : MonoBehaviour
    {
        public ShapeData defaultShape;
        public List<ShapeData> shapes;

        public Shape GetShape(int shapeIndex)
        {
            if (shapeIndex < shapes.Count)
            {
                return shapes[shapeIndex].shape;
            }
            return defaultShape.shape;
        }
        public GameObject GetPrefabForShape(Shape shape)
		{
            var matchingShape = shapes.DefaultIfEmpty(defaultShape).FirstOrDefault(s => s.shape == shape);
            if (matchingShape.prefab != null)
            {
                return matchingShape.prefab;
            }
            return defaultShape.prefab;
		}
        public bool OnShapeModuleCreated(Connectable module)
        {
            if (module.Function == ModuleFunction.SHAPE)
            {
                var shapeModule = module as Creator;
                CreateSceneNode(shapeModule);
                return true;
            }
            return false;
        }
        public SceneNode CreateSceneNode(Creator shapeModule)
        {
            var sceneObject = new GameObject("SceneNode");
            var shapePrefab = GetPrefabForShape(shapeModule.Shape);
            var sceneNode = sceneObject.AddComponent<SceneNode>();
            sceneNode.Initialize(this, shapeModule, shapePrefab);
            sceneNodes.Add(sceneNode);
            return sceneNode;
        }
        public bool DestroySceneNode(Creator module)
        {
            
            return false;
        }
        public SceneNode GetSceneNode(Connectable module)
        {
            return sceneNodes.FirstOrDefault(n => n.Module == module);
        }
        void Start()
        {
            sceneNodes = new List<SceneNode>();
        }
        void Update()
        {

        }
        private List<SceneNode> sceneNodes;
    }
}