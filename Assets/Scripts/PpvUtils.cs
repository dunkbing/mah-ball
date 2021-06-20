using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PpvUtils : MonoBehaviour
{

    // post processing
    private PostProcessVolume _pp;
    private Bloom _ppbl;
    private Vignette _ppvg;

    public static PpvUtils Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _pp = GetComponent<PostProcessVolume>();
        _ppbl = _pp.profile.GetSetting<Bloom>();
        _ppvg = _pp.profile.GetSetting<Vignette>();

        NoVignette();
    }

    public void Vignette()
    {
        _ppvg.enabled.value = true;
    }

    public void NoVignette()
    {
        _ppvg.enabled.value = false;
    }

}
