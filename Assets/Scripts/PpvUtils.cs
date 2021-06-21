using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PpvUtils : MonoBehaviour
{

    // post processing
    private PostProcessVolume _pp;
    private Bloom _ppbl;
    private Vignette _ppvg;
    private ChromaticAberration _ppca;

    public static PpvUtils Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _pp = GetComponent<PostProcessVolume>();
        _ppbl = _pp.profile.GetSetting<Bloom>();
        _ppvg = _pp.profile.GetSetting<Vignette>();
        _ppca = _pp.profile.GetSetting<ChromaticAberration>();
    }

    private void Start()
    {
        ExitSlowMo();
    }

    public void EnterSlowMo()
    {
        _ppvg.enabled.value = true;
        _ppca.enabled.value = true;
    }

    public void ExitSlowMo()
    {
        _ppvg.enabled.value = false;
        _ppca.enabled.value = false;
    }

    public void DisablePp()
    {
        foreach (var setting in _pp.profile.settings)
        {
            setting.enabled.value = false;
        }
    }

}
