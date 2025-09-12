// Quick SRP Batcher toggle from the Unity Editor menu.
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class SrpBatcherMenu
{
    private const string Root = "Tools/Rendering/SRP Batcher";
    private const string TogglePath = Root + "/Toggle";
    private const string EnablePath = Root + "/Enable";
    private const string DisablePath = Root + "/Disable";

    [MenuItem(TogglePath)]
    private static void Toggle()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = !GraphicsSettings.useScriptableRenderPipelineBatching;
        LogState();
    }

    // Validation method updates the checkmark state in the menu
    [MenuItem(TogglePath, true)]
    private static bool ToggleValidate()
    {
        Menu.SetChecked(TogglePath, GraphicsSettings.useScriptableRenderPipelineBatching);
        return true;
    }

    [MenuItem(EnablePath)]
    private static void Enable()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = true;
        LogState();
    }

    [MenuItem(DisablePath)]
    private static void Disable()
    {
        GraphicsSettings.useScriptableRenderPipelineBatching = false;
        LogState();
    }

    private static void LogState()
    {
        Debug.Log($"SRP Batcher: {(GraphicsSettings.useScriptableRenderPipelineBatching ? "Enabled" : "Disabled")}");
    }
}
#endif

