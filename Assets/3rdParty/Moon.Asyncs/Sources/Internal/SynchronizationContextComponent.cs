using UnityEngine;

namespace Moon.Asyncs.Internal
{
    public class SynchronizationContextComponent : MonoBehaviour
    {
        private const string SynchronizationContextName = "MangoSynchronizationContext";

        private static SynchronizationContextComponent _instance = null;
        private MangoSynchronizationContext _context;

        internal static void Initialize(MangoSynchronizationContext context)
        {
            if (_instance != null) return;
            GameObject go = new GameObject(SynchronizationContextName);
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<SynchronizationContextComponent>();
            _instance._context = context;
        }

        private void Update()
        {
            _context?.Update();
        }
    }
}