using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;

[RequireComponent(typeof(ActionBasedSnapTurnProvider))]
public class WorkAround : MonoBehaviour
{
    private ActionBasedSnapTurnProvider m_SnapTurnProvider;
    private IEnumerator m_WaitForXRSystemEnumerator;

    private bool IsXRSystemActive()
    {
        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            return false;
        }
        return XRGeneralSettings.Instance.Manager.isInitializationComplete;
    }
    public void OnEnable()
    {
        if (m_SnapTurnProvider == null)
        {
            m_SnapTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
        }

        bool isXRSystemActive = IsXRSystemActive();
        Action OnXRSystemActive = () =>
        {
            if (m_SnapTurnProvider == null) throw new Exception("Snap turn provider deleted");
            m_SnapTurnProvider.enabled = true;
        };
        if (isXRSystemActive)
        {
            OnXRSystemActive.Invoke();
        }
        else
        {
            m_SnapTurnProvider.enabled = false; //disable component, as it will throw exceptions while xr system is not active

            if (m_WaitForXRSystemEnumerator == null)
            {
                m_WaitForXRSystemEnumerator = WaitForXRSystem(OnXRSystemActive);
                StartCoroutine(m_WaitForXRSystemEnumerator);
            }
        }

    }

    IEnumerator WaitForXRSystem(Action onInitComplete)
    {
        while (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            yield return null;
        }
        while (!XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            yield return null;
        }

        onInitComplete?.Invoke();
    }
}