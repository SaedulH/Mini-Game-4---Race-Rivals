using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LoadingScreen : NonPersistentSingleton<LoadingScreen>
{
    [field: SerializeField] public Animator Anim { get; protected set; }
    [field: SerializeField] public CanvasGroup FullCanvas { get; protected set; }
    [field: SerializeField] public CanvasGroup LevelInfoCanvas { get; protected set; }
    [field: SerializeField] public Image Level_Image { get; protected set; }
    [field: SerializeField] public TMP_Text Level_Title { get; protected set; }
    [field: SerializeField] public TMP_Text Level_Summary { get; protected set; }
    [field: SerializeField] public TMP_Text Loading_Text { get; protected set; }
    [field: SerializeField] public Slider LoadingBar { get; protected set; }
    [field: SerializeField, Min(0f)] public float CurrentProgress { get; protected set; }
    [field: SerializeField, Min(0f)] public float MaxProgress { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Anim = GetComponent<Animator>();
    }

    public async Task SetLevelInfo(Sprite levelImage, string levelTitle, string levelSummary, int totalWeight)
    {
        Level_Image.sprite = levelImage;
        Level_Title.text = levelTitle;
        Level_Summary.text = levelSummary;
        Loading_Text.text = "Loading...";

        CurrentProgress = 0f;
        MaxProgress = totalWeight;

        FullCanvas.alpha = 0f;
        LevelInfoCanvas.alpha = 0f;
        LoadingBar.value = 0f;

        await Task.CompletedTask;
    }

    public async Task ShowLoadingScreen()
    {
        Debug.Log("Show Loading Screen");
        await LerpFadeIn(FullCanvas, 0.5f);

        await Task.Delay(100);

        await LerpFadeIn(LevelInfoCanvas, 0.5f);
    }

    public async Task HideLoadingScreen()
    {
        Debug.Log("Hide Level info canvas");
        await LerpFadeOut(LevelInfoCanvas, 0.5f);

        Debug.Log("Hide Fullcanvas");
        await LerpFadeOut(FullCanvas, 0.5f);
    }

    public void UpdateLoadingProgress(float weight)
    {
        CurrentProgress += weight;
        LoadingBar.value = CurrentProgress / MaxProgress;
    }

    public void UpdateLoadingDescription(string currentTask)
    {
        Loading_Text.text = currentTask;
    }

    private async Task LerpFadeIn(CanvasGroup canvas, float duration = 1f)
    {
        float time = 0f;
        canvas.alpha = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(0f, 1f, time / duration);
            await Task.Yield();
        }

        canvas.alpha = 1f;
        await Task.CompletedTask;
    }

    private async Task LerpFadeOut(CanvasGroup canvas, float duration = 1f)
    {
        float time = 0f;
        canvas.alpha = 1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1f, 0f, time / duration);
            await Task.Yield();
        }

        canvas.alpha = 0f;
        await Task.CompletedTask;
    }
}
