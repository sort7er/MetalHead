using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;

[RequireComponent(typeof(ActionBasedSnapTurnProvider))]
public class WorkAround : MonoBehaviour
{
    private ActionBasedSnapTurnProvider m_SnapTurnProvider;
    private ContinuousTurnProviderBase m_ContinousTurnProvider;
    private TeleportationProvider m_TeleportationProvider;
    private ContinuousMoveProviderBase m_ContinousMoveProvider;
    private IEnumerator m_WaitForXRSystemEnumerator;

    private bool IsXRSystemActive()
    {
        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            return false;
        }
        return XRGeneralSettings.Instance.Manager.isInitializationComplete;
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

    public void EnableSnap()
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
    public void EnableTurning()
    {
        if (m_ContinousTurnProvider == null)
        {
            m_ContinousTurnProvider = GetComponent<ActionBasedContinuousTurnProvider>();
        }

        bool isXRSystemActive = IsXRSystemActive();
        Action OnXRSystemActive = () =>
        {
            if (m_ContinousTurnProvider == null) throw new Exception("Snap turn provider deleted");
            m_ContinousTurnProvider.enabled = true;
        };
        if (isXRSystemActive)
        {
            OnXRSystemActive.Invoke();
        }
        else
        {
            m_ContinousTurnProvider.enabled = false;

            if (m_WaitForXRSystemEnumerator == null)
            {
                m_WaitForXRSystemEnumerator = WaitForXRSystem(OnXRSystemActive);
                StartCoroutine(m_WaitForXRSystemEnumerator);
            }
        }
    }
    public void EnableTeleport()
    {
        if (m_TeleportationProvider == null)
        {
            m_TeleportationProvider = GetComponent<TeleportationProvider>();
        }

        bool isXRSystemActive = IsXRSystemActive();
        Action OnXRSystemActive = () =>
        {
            if (m_TeleportationProvider == null) throw new Exception("Snap turn provider deleted");
            m_TeleportationProvider.enabled = true;
        };
        if (isXRSystemActive)
        {
            OnXRSystemActive.Invoke();
        }
        else
        {
            m_TeleportationProvider.enabled = false;

            if (m_WaitForXRSystemEnumerator == null)
            {
                m_WaitForXRSystemEnumerator = WaitForXRSystem(OnXRSystemActive);
                StartCoroutine(m_WaitForXRSystemEnumerator);
            }
        }
    }
    public void EnableMove()
    {
        if (m_ContinousMoveProvider == null)
        {
            m_ContinousMoveProvider = GetComponent<ActionBasedContinuousMoveProvider>();
        }

        bool isXRSystemActive = IsXRSystemActive();
        Action OnXRSystemActive = () =>
        {
            if (m_ContinousMoveProvider == null) throw new Exception("Snap turn provider deleted");
            m_ContinousMoveProvider.enabled = true;
        };
        if (isXRSystemActive)
        {
            OnXRSystemActive.Invoke();
        }
        else
        {
            m_ContinousMoveProvider.enabled = false;

            if (m_WaitForXRSystemEnumerator == null)
            {
                m_WaitForXRSystemEnumerator = WaitForXRSystem(OnXRSystemActive);
                StartCoroutine(m_WaitForXRSystemEnumerator);
            }
        }
    }
}