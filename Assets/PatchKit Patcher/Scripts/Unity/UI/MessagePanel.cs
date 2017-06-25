using PatchKit.Unity.Patcher;
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
            PatchKit.Unity.Patcher.Patcher.Instance.State.ObserveOnMainThread().Subscribe(state =>
            {
                _animator.SetBool("IsOpened", state == PatcherState.WaitingForUserDecision);
            }).AddTo(this);

            PatchKit.Unity.Patcher.Patcher.Instance.CanStartApp.ObserveOnMainThread().Subscribe(canStartApp =>
            {
                PlayButton.interactable = canStartApp;
            }).AddTo(this);

            PatchKit.Unity.Patcher.Patcher.Instance.CanInstallApp.ObserveOnMainThread().Subscribe(canInstallApp =>
            {
                _canInstallApp = canInstallApp;
                if (_canInstallApp)
                {
                    CheckButtonText.text = "Install";
                }
                CheckButton.interactable = _canInstallApp || _canCheckForAppUpdates;
            }).AddTo(this);

            PatchKit.Unity.Patcher.Patcher.Instance.CanCheckForAppUpdates.ObserveOnMainThread().Subscribe(canCheckForAppUpdates =>
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
            PatchKit.Unity.Patcher.Patcher.Instance.SetUserDecision(PatchKit.Unity.Patcher.Patcher.UserDecision.StartApp);
        }

        private void OnCheckButtonClicked()
        {
            if(_canInstallApp)
            {
                PatchKit.Unity.Patcher.Patcher.Instance.SetUserDecision(PatchKit.Unity.Patcher.Patcher.UserDecision.InstallApp);
            }
            else if(_canCheckForAppUpdates)
            {
                PatchKit.Unity.Patcher.Patcher.Instance.SetUserDecision(PatchKit.Unity.Patcher.Patcher.UserDecision.CheckForAppUpdates);
            }
        }
    }
}