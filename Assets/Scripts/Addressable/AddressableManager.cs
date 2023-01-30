using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
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
        [SerializeField] private GameObject loader;
        private GameObject playerController;
        private InputAction quit;

        private InputActionMap _currentMap;

        private bool _clearPreviousScene;
        private SceneInstance _previousLoadedScene;

        private void Start()
        {
            loader.SetActive(true);
            Addressables.InitializeAsync().Completed += AddressableManagerCompleted;
        }

        private void Update()
        {
            if (loader.activeSelf
                && playerArmatureAssetReference.Asset != null
                && musicAssetReference.Asset != null
                && unityLogoAssetReferenceTexture2D.Asset != null)
            {
                //loader
                loader.SetActive(false);
            }
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
            playerArmatureAssetReference.LoadAssetAsync<GameObject>().Completed += (playerArmatureAsset) =>
            {
                tex.text += "\n Addressable loading character...";
                playerArmatureAssetReference.InstantiateAsync().Completed += (playerArmatureGameObject) =>
                {
                    tex.text += "\n Addressable instantiating character...";
                    playerController = playerArmatureGameObject.Result;
                    cinemachineVirtualCamera.Follow = playerController.transform.Find(nameRootCharacter);
                    playerController.AddComponent<ChunkSpawner>().PlayerPosition(new Vector3(55 * 14, 1, 55 * 14));
                    tex.text += "\n Addressable instance character complete...";
                };
            };
        }

        public void LoadAddressableLevel(String addressableKey)
        {
            if (_clearPreviousScene)
            {
                Addressables.UnloadSceneAsync(_previousLoadedScene).Completed += (asyncHandle) =>
                {
                    _clearPreviousScene = false;
                    _previousLoadedScene = new SceneInstance();
                    tex.text += $"\n Uploaded scene {addressableKey} successfully";
                };
            }

            Addressables.LoadSceneAsync(addressableKey, LoadSceneMode.Additive).Completed += (asyncHandle) =>
            {
                _clearPreviousScene = true;
                _previousLoadedScene = asyncHandle.Result;
                tex.text += $"\n Loaded scene {addressableKey} successfully";
            };
        }

        private void OnDestroy()
        {
            playerArmatureAssetReference.ReleaseInstance(playerController);
            unityLogoAssetReferenceTexture2D.ReleaseAsset();
        }
    }
}