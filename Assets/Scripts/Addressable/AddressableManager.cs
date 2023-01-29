using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Addressable
{
    [Serializable]
    public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
    {
        public AssetReferenceAudioClip(string guid) : base(guid)
        {
        }
    }

    public class AddressableManager : MonoBehaviour
    {
        [SerializeField] private Text tex;
        [SerializeField] private AssetReference playerArmatureAssetReference;
        [SerializeField] private AssetReference musicAssetReference;
        [SerializeField] private AssetReferenceTexture2D unityLogoAssetReferenceTexture2D;

        [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

        //UI
        [SerializeField] private RawImage _rawImageLogo;
        [SerializeField] private string nameRootCharacter;
        private GameObject playerController;
        private InputAction quit;

        private InputActionMap _currentMap;

        private void Start()
        {
            Addressables.InitializeAsync().Completed += AddressableManagerCompleted;
        }

        private void LoadLogo(AsyncOperationHandle<Texture2D> asyncOperationHandle)
        {
            _rawImageLogo.texture = asyncOperationHandle.Result;
            Color currentColor = _rawImageLogo.color;
            currentColor.a = 1.0f;
            _rawImageLogo.color = currentColor;
            tex.text += "\n  Addressable load image Completed...";
        }

        private void AddressableManagerCompleted(AsyncOperationHandle<IResourceLocator> resourcesLocator)
        {
            tex.text += "\n AdressableManagerCompleted...";
            LoadCharacter();
            LoadMusic();
            LoadImageLogo();
        }

        private void LoadImageLogo()
        {
            unityLogoAssetReferenceTexture2D.LoadAssetAsync<Texture2D>().Completed += LoadLogo;
        }

        private void LoadMusic()
        {
            musicAssetReference.LoadAssetAsync<AudioClip>().Completed += (clip) =>
            {
                var audioSorce = gameObject.AddComponent<AudioSource>();
                audioSorce.clip = clip.Result;
                audioSorce.playOnAwake = true;
                audioSorce.loop = true;
                audioSorce.volume = 0.02f;
                audioSorce.Play();
                tex.text += "\n Addressable load music complete...";
            };
        }

        private void LoadCharacter()
        {
            playerArmatureAssetReference.InstantiateAsync().Completed += (go) =>
            {
                playerController = go.Result;
                cinemachineVirtualCamera.Follow = playerController.transform.Find(nameRootCharacter);
                playerController.AddComponent<ChunkSpawner>().PlayerPosition(new Vector3(55*14, 1, 55*14));
                tex.text += "\n Addressable load character complete...";
            };
        }

        private void OnDestroy()
        {
            playerArmatureAssetReference.ReleaseInstance(playerController);
            unityLogoAssetReferenceTexture2D.ReleaseAsset();
        }
    }
}