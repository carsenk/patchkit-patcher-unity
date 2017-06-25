using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PatchKit.Patcher.Unity.UI
{
    [RequireComponent(typeof(Animator))]
    public class MessagePanel : MonoBehaviour
    {
        public Button PlayButton;

        public Button CheckButton;

        public Text CheckButtonText;

        private bool _canInstallApp;

        private bool _canCheckForAppUpdates;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            UnityPatcher.Instance.State.ObserveOnMainThread().Subscribe(state =>
            {
                _animator.SetBool("IsOpened", state == UnityPatcherState.WaitingForUserDecision);
            }).AddTo(this);

            UnityPatcher.Instance.CanStartApp.ObserveOnMainThread().Subscribe(canStartApp =>
            {
                PlayButton.interactable = canStartApp;
            }).AddTo(this);

            UnityPatcher.Instance.CanInstallApp.ObserveOnMainThread().Subscribe(canInstallApp =>
            {
                _canInstallApp = canInstallApp;
                if (_canInstallApp)
                {
                    CheckButtonText.text = "Install";
                }
                CheckButton.interactable = _canInstallApp || _canCheckForAppUpdates;
            }).AddTo(this);

            UnityPatcher.Instance.CanCheckForAppUpdates.ObserveOnMainThread().Subscribe(canCheckForAppUpdates =>
            {
                _canCheckForAppUpdates = canCheckForAppUpdates;
                if (_canCheckForAppUpdates)
                {
                    CheckButtonText.text = "Check for updates";
                }
                CheckButton.interactable = _canInstallApp || _canCheckForAppUpdates;
            }).AddTo(this);

            PlayButton.onClick.AddListener(OnPlayButtonClicked);
            CheckButton.onClick.AddListener(OnCheckButtonClicked);

            _animator.SetBool("IsOpened", false);
            PlayButton.interactable = false;
            CheckButton.interactable = false;
            CheckButtonText.text = "Check for updates";
        }

        private void OnPlayButtonClicked()
        {
            UnityPatcher.Instance.SetUserDecision(UnityPatcher.UserDecision.StartApp);
        }

        private void OnCheckButtonClicked()
        {
            if(_canInstallApp)
            {
                UnityPatcher.Instance.SetUserDecision(UnityPatcher.UserDecision.InstallApp);
            }
            else if(_canCheckForAppUpdates)
            {
                UnityPatcher.Instance.SetUserDecision(UnityPatcher.UserDecision.CheckForAppUpdates);
            }
        }
    }
}