using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Utilities
{
    public class PpvUtils : MonoBehaviour
    {
        // post processing
        public PostProcessVolume volume;
        private Bloom _ppbl;
        private Vignette _ppvg;
        private ChromaticAberration _ppca;

        public static PpvUtils Instance { get; private set; }

        private void Awake()
        {
            Instance ??= this;

            _ppbl = volume.profile.GetSetting<Bloom>();
            _ppvg = volume.profile.GetSetting<Vignette>();
            _ppca = volume.profile.GetSetting<ChromaticAberration>();
        }

        private void Start()
        {
            Disable();
        }

        public void Activate()
        {
            _ppvg.enabled.value = true;
            _ppca.enabled.value = true;
        }

        public void Disable()
        {
            _ppvg.enabled.value = false;
            _ppca.enabled.value = false;
        }

        public void DisablePp()
        {
            foreach (var setting in volume.profile.settings)
            {
                setting.enabled.value = false;
            }
        }

    }
}
