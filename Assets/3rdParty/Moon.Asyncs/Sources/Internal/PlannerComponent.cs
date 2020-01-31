using UnityEngine;

namespace Moon.Asyncs.Internal
{
    public class PlannerComponent : MonoBehaviour 
	{
        private const string PlannerName = "AsyncPlanner";

        private PlannerSynchronizationContext _context;
        private static PlannerComponent _instance = null;

        internal static void Initialize(PlannerSynchronizationContext context)
        {
            if (_instance != null) return;
            GameObject go = new GameObject(PlannerName);
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<PlannerComponent>();
            _instance._context = context;
        }

        private void Update() 
		{
            _context?.Update();
        }
    }
}
