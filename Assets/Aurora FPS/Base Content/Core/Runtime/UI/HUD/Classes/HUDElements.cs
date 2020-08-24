/* ================================================================
   ---------------------------------------------------
   Project   :    Aurora FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017-2020 All rights reserved.
   ================================================================ */

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using EventSystem = UnityEngine.EventSystems.EventSystem;
using StandaloneInputModule = UnityEngine.EventSystems.StandaloneInputModule;

namespace AuroraFPSRuntime.UI
{
    /// <summary>
    /// Player HUD Components.
    /// </summary>
    [System.Serializable]
    public class HUDElements
    {
        // Base HUD properties.
        [Header("Base HUD Properties")]
        [SerializeField] private RectTransform rootHUDRect;

        // Health properties.
        [Header("Health Properties")]
        [SerializeField] private RectTransform healthRect;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Text healthCounter;

        // Weapon properties.
        [Header("Weapon Properites")]
        [SerializeField] private RectTransform weaponRect;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Text weaponName;
        [SerializeField] private Text bulletCounter;
        [SerializeField] private Text clipCounter;
        [SerializeField] private Text fireMode;
        [SerializeField] private RectTransform grenadeGroup;
        [SerializeField] private RectTransform grenadeTemplate;

        // Message properties.
        [Header("Message Properties")]
        [SerializeField] private RectTransform messageRect;
        [SerializeField] private Text messageTextField;
        [SerializeField] private Image messageImage;

        /// <summary>
        /// Processing health point.
        /// </summary>
        /// <param name="value">Value to draw.</param>
        /// <param name="currenthealth">Current health point.</param>
        /// <param name="maxHealth">Max health point.</param>
        public virtual void UpdateHealthElements(int value, int currenthealth, int maxHealth)
        {
            if (healthCounter != null)
            {
                healthCounter.text = value.ToString();
            }

            if (healthSlider != null)
            {
                healthSlider.value = Mathf.InverseLerp(0, maxHealth, currenthealth);
            }
        }

        /// <summary>
        /// Processing weapon information (name and image).
        /// </summary>
        /// <param name="name">Weapon name.</param>
        /// <param name="weaponSprite">Weapon sprite.</param>
        public virtual void UpdateWeaponElements(string name, Sprite weaponSprite = null)
        {
            if (weaponName != null && name != "")
            {
                weaponName.text = name;
            }
            else if (weaponName != null && name == null)
            {
                weaponName.text = "UnNamed Weapon: " + Random.Range(0, 99);
            }

            if (weaponImage != null && weaponSprite != null)
            {
                if (!weaponImage.gameObject.activeSelf)
                {
                    weaponImage.gameObject.SetActive(true);
                }
                weaponImage.sprite = weaponSprite;
            }
            else if (weaponImage != null && weaponSprite == null)
            {
                weaponImage.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Processing ammo information (bullets and clips).
        /// </summary>
        /// /// <param name="bulletCount">Weapon bullet count.</param>
        /// <param name="clipCount">Weapon clip count.</param>
        public virtual void UpdateAmmoElements(int bulletCount, int clipCount)
        {
            if (bulletCounter != null)
            {
                bulletCounter.text = bulletCount.ToString();
            }
            if (clipCounter != null)
            {
                clipCounter.text = clipCount.ToString();
            }
        }

        /// <summary>
        /// Processing ammo information (bullets and clips).
        /// </summary>
        /// /// <param name="bulletCount">Weapon bullet count.</param>
        /// <param name="clipCount">Weapon clip count.</param>
        public virtual void UpdateAmmoElements(string bulletCount, string clipCount)
        {
            if (bulletCounter != null)
            {
                bulletCounter.text = bulletCount;
            }
            if (clipCounter != null)
            {
                clipCounter.text = clipCount;
            }
        }

        /// <summary>
        /// Update fire mode.
        /// </summary>
        /// <param name="mode">Fire mode name.</param>
        public virtual void UpdateFireMode(string mode)
        {
            if (fireMode != null)
            {
                fireMode.text = mode;
            }
        }

        /// <summary>
        /// Generate new HUD elements.
        /// </summary>
        public void GenerateHUD()
        {
            // Creating Canvas.
            GameObject canvas = GameObject.FindObjectOfType<Canvas>()?.gameObject;
            if (canvas == null)
            {
                canvas = new GameObject("Canvas");
                canvas.layer = LayerMask.NameToLayer("UI");

                // Add required components.
                Canvas canvasComponent = canvas.AddComponent<Canvas>();
                CanvasScaler canvasScalerComponent = canvas.AddComponent<CanvasScaler>();
                GraphicRaycaster graphicRaycasterComponent = canvas.AddComponent<GraphicRaycaster>();

                // Setup required components.
                canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasComponent.pixelPerfect = false;
                canvasComponent.sortingOrder = 0;
                canvasComponent.targetDisplay = 0;
                canvasComponent.additionalShaderChannels = 0;

                canvasScalerComponent.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScalerComponent.referenceResolution = new Vector2(800, 600);
                canvasScalerComponent.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                canvasScalerComponent.matchWidthOrHeight = 0.5f;
                canvasScalerComponent.referencePixelsPerUnit = 100;

                graphicRaycasterComponent.ignoreReversedGraphics = true;
                graphicRaycasterComponent.blockingObjects = GraphicRaycaster.BlockingObjects.None;
            }

            // Creating event system.
            GameObject eventSystem = GameObject.FindObjectOfType<EventSystem>()?.gameObject;
            if (eventSystem == null)
            {
                eventSystem = new GameObject("Event System");

                // Add required components.
                EventSystem eventSystemComponent = eventSystem.AddComponent<EventSystem>();
                StandaloneInputModule standaloneInputModuleComponent = eventSystem.AddComponent<StandaloneInputModule>();

                // Setup required components.
                eventSystemComponent.firstSelectedGameObject = null;
                eventSystemComponent.sendNavigationEvents = true;
                eventSystemComponent.pixelDragThreshold = 10;

                standaloneInputModuleComponent.horizontalAxis = "Horizontal";
                standaloneInputModuleComponent.verticalAxis = "Vertical";
                standaloneInputModuleComponent.submitButton = "Submit";
                standaloneInputModuleComponent.cancelButton = "Cancel";
                standaloneInputModuleComponent.inputActionsPerSecond = 10.0f;
                standaloneInputModuleComponent.repeatDelay = 0.5f;
                standaloneInputModuleComponent.forceModuleActive = false;
            }

            // Creating HUD elements panel.
            GameObject hudPanel = new GameObject("HUD Elements", typeof(RectTransform));
            hudPanel.transform.SetParent(canvas.transform);

            // Setup required components.
            rootHUDRect = hudPanel.GetComponent<RectTransform>();
            rootHUDRect.localPosition = Vector3.zero;
            rootHUDRect.localRotation = Quaternion.identity;
            rootHUDRect.anchorMin = Vector2.zero;
            rootHUDRect.anchorMax = Vector2.one;
            rootHUDRect.pivot = Vector2.zero;
            rootHUDRect.anchoredPosition = Vector3.zero;
            rootHUDRect.sizeDelta = Vector2.zero;

            Color effectColor = new Color32(67, 67, 67, 128);

            // Creating weapon panel.
            GameObject weaponPanel = new GameObject("Weapon", typeof(RectTransform));
            weaponPanel.transform.SetParent(rootHUDRect);

            // Add required components.
            Image weaponPanelImage = weaponPanel.AddComponent<Image>();

            // Setup required components.
            weaponRect = weaponPanel.GetComponent<RectTransform>();
            weaponRect.localPosition = Vector3.zero;
            weaponRect.localRotation = Quaternion.identity;
            weaponRect.pivot = Vector2.zero;
            weaponRect.anchorMin = new Vector2(0.774f, 0.073f);
            weaponRect.anchorMax = new Vector2(1.0f, 0.205f);
            weaponRect.anchoredPosition = Vector3.zero;
            weaponRect.sizeDelta = Vector2.zero;

            weaponPanelImage.sprite = null;
            weaponPanelImage.color = new Color32(255, 255, 255, 50);
            weaponPanelImage.material = null;
            weaponPanelImage.raycastTarget = true;

            // Create weapon background.
            GameObject weaponBackground = new GameObject("Background", typeof(RectTransform));
            weaponBackground.transform.SetParent(weaponRect);

            // Get/Add required components.
            RectTransform weaponBackgroundRect = weaponBackground.GetComponent<RectTransform>();
            Image weaponBackgroundImage = weaponBackground.AddComponent<Image>();

            // Setup required componetns.
            weaponBackgroundRect.localPosition = Vector3.zero;
            weaponBackgroundRect.localRotation = Quaternion.identity;
            weaponBackgroundRect.pivot = Vector2.one / 2;
            weaponBackgroundRect.anchorMin = Vector2.zero;
            weaponBackgroundRect.anchorMax = Vector2.one;
            weaponBackgroundRect.anchoredPosition = Vector3.zero;
            weaponBackgroundRect.sizeDelta = Vector2.zero;

            weaponBackgroundImage.sprite = null;
            weaponBackgroundImage.color = new Color32(177, 177, 177, 50);
            weaponBackgroundImage.material = null;
            weaponBackgroundImage.raycastTarget = true;

            // Create weapon info.
            GameObject weaponInfo = new GameObject("Weapon Info", typeof(RectTransform));
            weaponInfo.transform.SetParent(weaponRect);

            // Get/Add required components.
            RectTransform weaponInfoRect = weaponInfo.GetComponent<RectTransform>();

            // Setup required components.
            weaponInfoRect.localPosition = Vector3.zero;
            weaponInfoRect.localRotation = Quaternion.identity;
            weaponInfoRect.pivot = Vector2.one / 2;
            weaponInfoRect.anchorMin = Vector2.zero;
            weaponInfoRect.anchorMax = new Vector2(0.53f, 1.0f);
            weaponInfoRect.anchoredPosition = Vector3.zero;
            weaponInfoRect.sizeDelta = Vector2.zero;

            // Create weapon image.
            GameObject weaponImageObject = new GameObject("Weapon Image", typeof(RectTransform));
            weaponImageObject.transform.SetParent(weaponInfoRect);

            // Add required components.
            weaponImage = weaponImageObject.AddComponent<Image>();

            // Add required effects.
            Shadow weaponImageShadowEffect = weaponImageObject.AddComponent<Shadow>();
            Outline weaponImageOutlineEffect = weaponImageObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform weaponImageRect = weaponImageObject.GetComponent<RectTransform>();
            weaponImageRect.localPosition = Vector3.zero;
            weaponImageRect.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            weaponImageRect.pivot = Vector2.one / 2;
            weaponImageRect.anchorMin = new Vector2(0.026f, 0.5f);
            weaponImageRect.anchorMax = new Vector2(0.977f, 0.956f);
            weaponImageRect.anchoredPosition = Vector3.zero;
            weaponImageRect.sizeDelta = Vector2.zero;

            weaponImage.sprite = null;
            weaponImage.color = Color.white;
            weaponImage.material = null;
            weaponImage.raycastTarget = true;
            weaponImage.type = Image.Type.Simple;
            weaponImage.useSpriteMesh = false;
            weaponImage.preserveAspect = true;

            weaponImageShadowEffect.effectColor = effectColor;
            weaponImageShadowEffect.effectDistance = new Vector2(-1.5f, -1.5f);
            weaponImageShadowEffect.useGraphicAlpha = true;

            weaponImageOutlineEffect.effectColor = effectColor;
            weaponImageOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            weaponImageOutlineEffect.useGraphicAlpha = true;

            // Create weapon name.
            GameObject weaponNameObject = new GameObject("Weapon Name", typeof(RectTransform));
            weaponNameObject.transform.SetParent(weaponInfoRect);

            // Add required components.
            weaponName = weaponNameObject.AddComponent<Text>();

            // Add required effects.
            Shadow weaponNameShadowEffect = weaponNameObject.AddComponent<Shadow>();
            Outline weaponNameOutlineEffect = weaponNameObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform weaponNameRect = weaponNameObject.GetComponent<RectTransform>();
            weaponNameRect.localPosition = Vector3.zero;
            weaponNameRect.localRotation = Quaternion.identity;
            weaponNameRect.pivot = Vector2.one / 2;
            weaponNameRect.anchorMin = Vector2.zero;
            weaponNameRect.anchorMax = new Vector2(1.0f, 0.5f);
            weaponNameRect.anchoredPosition = Vector3.zero;
            weaponNameRect.sizeDelta = Vector2.zero;

            weaponName.text = "wName";
            weaponName.fontStyle = FontStyle.Bold;
            weaponName.fontSize = 21;
            weaponName.lineSpacing = 1;
            weaponName.supportRichText = true;
            weaponName.alignment = TextAnchor.MiddleCenter;
            weaponName.alignByGeometry = false;
            weaponName.horizontalOverflow = HorizontalWrapMode.Wrap;
            weaponName.verticalOverflow = VerticalWrapMode.Truncate;
            weaponName.resizeTextForBestFit = true;
            weaponName.resizeTextMinSize = 1;
            weaponName.resizeTextMaxSize = 21;
            weaponName.color = Color.white;
            weaponName.material = null;
            weaponName.raycastTarget = true;

            weaponNameShadowEffect.effectColor = effectColor;
            weaponNameShadowEffect.effectDistance = new Vector2(1.5f, -1.5f);
            weaponNameShadowEffect.useGraphicAlpha = true;

            weaponNameOutlineEffect.effectColor = effectColor;
            weaponNameOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            weaponNameOutlineEffect.useGraphicAlpha = true;

            // Create ammo info.
            GameObject ammoInfo = new GameObject("Ammo Info", typeof(RectTransform));
            ammoInfo.transform.SetParent(weaponRect);

            // Get/Add required components.
            RectTransform ammoInfoRect = ammoInfo.GetComponent<RectTransform>();

            // Setup required components.
            ammoInfoRect.localPosition = Vector3.zero;
            ammoInfoRect.localRotation = Quaternion.identity;
            ammoInfoRect.pivot = Vector2.one / 2;
            ammoInfoRect.anchorMin = new Vector2(0.53f, 0.0f);
            ammoInfoRect.anchorMax = new Vector2(0.713f, 1.0f);
            ammoInfoRect.anchoredPosition = Vector3.zero;
            ammoInfoRect.sizeDelta = Vector2.zero;

            // Create bullet counter.
            GameObject bulletCounterObject = new GameObject("Bullet Counter", typeof(RectTransform));
            bulletCounterObject.transform.SetParent(ammoInfoRect);

            // Add required components.
            bulletCounter = bulletCounterObject.AddComponent<Text>();

            // Add required effects.
            Shadow bulletCounterShadowEffect = bulletCounterObject.AddComponent<Shadow>();
            Outline bulletCounterOutlineEffect = bulletCounterObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform bulletCounterRect = bulletCounterObject.GetComponent<RectTransform>();
            bulletCounterRect.localPosition = Vector3.zero;
            bulletCounterRect.localRotation = Quaternion.identity;
            bulletCounterRect.pivot = Vector2.one / 2;
            bulletCounterRect.anchorMin = new Vector2(0.0f, 0.5f);
            bulletCounterRect.anchorMax = Vector2.one;
            bulletCounterRect.anchoredPosition = Vector3.zero;
            bulletCounterRect.sizeDelta = Vector2.zero;

            bulletCounter.text = "31";
            bulletCounter.fontStyle = FontStyle.Bold;
            bulletCounter.fontSize = 25;
            bulletCounter.lineSpacing = 1;
            bulletCounter.supportRichText = true;
            bulletCounter.alignment = TextAnchor.LowerRight;
            bulletCounter.alignByGeometry = false;
            bulletCounter.horizontalOverflow = HorizontalWrapMode.Wrap;
            bulletCounter.verticalOverflow = VerticalWrapMode.Truncate;
            bulletCounter.resizeTextForBestFit = true;
            bulletCounter.resizeTextMinSize = 1;
            bulletCounter.resizeTextMaxSize = 25;
            bulletCounter.color = Color.white;
            bulletCounter.material = null;
            bulletCounter.raycastTarget = true;

            bulletCounterShadowEffect.effectColor = effectColor;
            bulletCounterShadowEffect.effectDistance = new Vector2(1.5f, -1.5f);
            bulletCounterShadowEffect.useGraphicAlpha = true;

            bulletCounterOutlineEffect.effectColor = effectColor;
            bulletCounterOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            bulletCounterOutlineEffect.useGraphicAlpha = true;

            // Create clip cointer.
            GameObject clipCounterObject = new GameObject("Clip Counter", typeof(RectTransform));
            clipCounterObject.transform.SetParent(ammoInfoRect);

            // Add required components.
            clipCounter = clipCounterObject.AddComponent<Text>();

            // Add required effects.
            Shadow clipCounterShadowEffect = clipCounterObject.AddComponent<Shadow>();
            Outline clipCounterOutlineEffect = clipCounterObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform clipCounterRect = clipCounterObject.GetComponent<RectTransform>();
            clipCounterRect.localPosition = Vector3.zero;
            clipCounterRect.localRotation = Quaternion.identity;
            clipCounterRect.pivot = Vector2.one / 2;
            clipCounterRect.anchorMin = Vector2.zero;
            clipCounterRect.anchorMax = new Vector2(1.0f, 0.5f);
            clipCounterRect.anchoredPosition = Vector3.zero;
            clipCounterRect.sizeDelta = Vector2.zero;

            clipCounter.text = "256";
            clipCounter.fontStyle = FontStyle.Normal;
            clipCounter.fontSize = 21;
            clipCounter.lineSpacing = 1;
            clipCounter.supportRichText = true;
            clipCounter.alignment = TextAnchor.MiddleCenter;
            clipCounter.alignByGeometry = false;
            clipCounter.horizontalOverflow = HorizontalWrapMode.Wrap;
            clipCounter.verticalOverflow = VerticalWrapMode.Truncate;
            clipCounter.resizeTextForBestFit = true;
            clipCounter.resizeTextMinSize = 1;
            clipCounter.resizeTextMaxSize = 21;
            clipCounter.color = Color.white;
            clipCounter.material = null;
            clipCounter.raycastTarget = true;

            clipCounterShadowEffect.effectColor = effectColor;
            clipCounterShadowEffect.effectDistance = new Vector2(1.5f, -1.5f);
            clipCounterShadowEffect.useGraphicAlpha = true;

            clipCounterOutlineEffect.effectColor = effectColor;
            clipCounterOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            clipCounterOutlineEffect.useGraphicAlpha = true;

            // Create grenade group.
            GameObject grenadeGroup = new GameObject("Grenade Group", typeof(RectTransform));
            grenadeGroup.transform.SetParent(weaponRect);

            // Get/Add required components.
            RectTransform grenadeGroupRect = grenadeGroup.GetComponent<RectTransform>();
            GridLayoutGroup grenadeGridLayout = grenadeGroup.AddComponent<GridLayoutGroup>();

            // Setup required components.
            grenadeGroupRect.localPosition = Vector3.zero;
            grenadeGroupRect.localRotation = Quaternion.identity;
            grenadeGroupRect.pivot = Vector2.one / 2;
            grenadeGroupRect.anchorMin = new Vector2(0.713f, 0.0f);
            grenadeGroupRect.anchorMax = Vector2.one;
            grenadeGroupRect.anchoredPosition = Vector3.zero;
            grenadeGroupRect.sizeDelta = Vector2.zero;

            grenadeGridLayout.padding = new RectOffset(0, 0, 8, 0);
            grenadeGridLayout.cellSize = new Vector2(27.0f, 23.0f);
            grenadeGridLayout.spacing = new Vector2(0.0f, 5.0f);
            grenadeGridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            grenadeGridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
            grenadeGridLayout.childAlignment = TextAnchor.UpperCenter;
            grenadeGridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grenadeGridLayout.constraintCount = 2;

            // Create grenade template
            GameObject grenadeTemplateObject = new GameObject("Grenade Template", typeof(RectTransform));
            grenadeTemplateObject.transform.SetParent(grenadeGroupRect);

            // Get/Add required components.
            grenadeTemplate = grenadeTemplateObject.GetComponent<RectTransform>();

            //Create grenade image.
            GameObject grenadeTemplateImageObject = new GameObject("Grenade Image", typeof(RectTransform));
            grenadeTemplateImageObject.transform.SetParent(grenadeTemplate);

            //Add required components.
            Image grenadeTemplateImage = grenadeTemplateImageObject.AddComponent<Image>();

            // Add required effects.
            Shadow grenadeTemplateImageShadowEffect = grenadeTemplateImageObject.AddComponent<Shadow>();
            Outline grenadeTemplateImageOutlineEffect = grenadeTemplateImageObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform grenadeTemplateImageRect = grenadeTemplateImageObject.GetComponent<RectTransform>();
            grenadeTemplateImageRect.localPosition = Vector3.zero;
            grenadeTemplateImageRect.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            grenadeTemplateImageRect.pivot = Vector2.one / 2;
            grenadeTemplateImageRect.anchorMin = Vector2.zero;
            grenadeTemplateImageRect.anchorMax = new Vector2(0.558f, 1.0f);
            grenadeTemplateImageRect.anchoredPosition = Vector3.zero;
            grenadeTemplateImageRect.sizeDelta = Vector2.zero;

            grenadeTemplateImage.sprite = null;
            grenadeTemplateImage.color = Color.white;
            grenadeTemplateImage.material = null;
            grenadeTemplateImage.raycastTarget = true;
            grenadeTemplateImage.type = Image.Type.Simple;
            grenadeTemplateImage.useSpriteMesh = false;
            grenadeTemplateImage.preserveAspect = true;

            grenadeTemplateImageShadowEffect.effectColor = effectColor;
            grenadeTemplateImageShadowEffect.effectDistance = new Vector2(-1.5f, -1.5f);
            grenadeTemplateImageShadowEffect.useGraphicAlpha = true;

            grenadeTemplateImageOutlineEffect.effectColor = effectColor;
            grenadeTemplateImageOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            grenadeTemplateImageOutlineEffect.useGraphicAlpha = true;

            // Create grenade cointer.
            GameObject grenadeTemplateCounterObject = new GameObject("Grenade Counter", typeof(RectTransform));
            grenadeTemplateCounterObject.transform.SetParent(grenadeTemplate);

            // Add required components.
            Text grenadeCounter = grenadeTemplateCounterObject.AddComponent<Text>();

            // Add required effects.
            Shadow grenadeCounterShadowEffect = grenadeTemplateCounterObject.AddComponent<Shadow>();
            Outline grenadeCounterOutlineEffect = grenadeTemplateCounterObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform grenadeCounterRect = grenadeTemplateCounterObject.GetComponent<RectTransform>();
            grenadeCounterRect.localPosition = Vector3.zero;
            grenadeCounterRect.localRotation = Quaternion.identity;
            grenadeCounterRect.pivot = Vector2.one / 2;
            grenadeCounterRect.anchorMin = new Vector2(0.558f, 0);
            grenadeCounterRect.anchorMax = Vector2.one;
            grenadeCounterRect.anchoredPosition = Vector3.zero;
            grenadeCounterRect.sizeDelta = Vector2.zero;

            grenadeCounter.text = "0";
            grenadeCounter.fontStyle = FontStyle.Normal;
            grenadeCounter.fontSize = 17;
            grenadeCounter.lineSpacing = 1;
            grenadeCounter.supportRichText = true;
            grenadeCounter.alignment = TextAnchor.MiddleCenter;
            grenadeCounter.alignByGeometry = false;
            grenadeCounter.horizontalOverflow = HorizontalWrapMode.Wrap;
            grenadeCounter.verticalOverflow = VerticalWrapMode.Truncate;
            grenadeCounter.resizeTextForBestFit = true;
            grenadeCounter.resizeTextMinSize = 1;
            grenadeCounter.resizeTextMaxSize = 17;
            grenadeCounter.color = Color.white;
            grenadeCounter.material = null;
            grenadeCounter.raycastTarget = true;

            grenadeCounterShadowEffect.effectColor = effectColor;
            grenadeCounterShadowEffect.effectDistance = new Vector2(1.5f, -1.5f);
            grenadeCounterShadowEffect.useGraphicAlpha = true;

            grenadeCounterOutlineEffect.effectColor = effectColor;
            grenadeCounterOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            grenadeCounterOutlineEffect.useGraphicAlpha = true;

            grenadeTemplateObject.SetActive(false);

            // Creating message info panel.
            GameObject messagePanel = new GameObject("Message", typeof(RectTransform));
            messagePanel.transform.SetParent(rootHUDRect);

            // Add required components.
            VerticalLayoutGroup messagePanelVerticalLayoutGroup = messagePanel.AddComponent<VerticalLayoutGroup>();

            // Setup required components.
            messageRect = messagePanel.GetComponent<RectTransform>();
            messageRect.localPosition = Vector3.zero;
            messageRect.localRotation = Quaternion.identity;
            messageRect.pivot = Vector2.one / 2;
            messageRect.anchorMin = new Vector2(0.325f, 0.288f);
            messageRect.anchorMax = new Vector2(0.674f, 0.423f);
            messageRect.anchoredPosition = Vector3.zero;
            messageRect.sizeDelta = Vector2.zero;

            messagePanelVerticalLayoutGroup.padding = new RectOffset(0, 0, 0, 0);
            messagePanelVerticalLayoutGroup.spacing = 0;
            messagePanelVerticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            messagePanelVerticalLayoutGroup.childControlWidth = false;
            messagePanelVerticalLayoutGroup.childControlHeight = false;
            messagePanelVerticalLayoutGroup.childScaleWidth = true;
            messagePanelVerticalLayoutGroup.childScaleHeight = true;
            messagePanelVerticalLayoutGroup.childForceExpandWidth = true;
            messagePanelVerticalLayoutGroup.childForceExpandHeight = true;

            // Creating message text field.
            GameObject messageTextFieldObject = new GameObject("Message Text Field", typeof(RectTransform));
            messageTextFieldObject.transform.SetParent(messageRect);

            // Add required components.
            messageTextField = messageTextFieldObject.AddComponent<Text>();

            // Add required effects.
            Shadow messageTextFieldShadowEffect = messageTextFieldObject.AddComponent<Shadow>();
            Outline messageTextFieldOutlineEffect = messageTextFieldObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform messageTextFieldRect = messageTextFieldObject.GetComponent<RectTransform>();
            messageTextFieldRect.localPosition = Vector3.zero;
            messageTextFieldRect.localRotation = Quaternion.identity;
            messageTextFieldRect.pivot = Vector2.one / 2;
            messageTextFieldRect.anchorMin = new Vector2(0.0f, 0.5f);
            messageTextFieldRect.anchorMax = Vector3.one;
            messageTextFieldRect.anchoredPosition = Vector3.zero;
            messageTextFieldRect.sizeDelta = new Vector2(322.39f, 31.79f);

            messageTextField.text = "None";
            messageTextField.fontStyle = FontStyle.Normal;
            messageTextField.fontSize = 15;
            messageTextField.lineSpacing = 1;
            messageTextField.supportRichText = true;
            messageTextField.alignment = TextAnchor.MiddleCenter;
            messageTextField.alignByGeometry = false;
            messageTextField.horizontalOverflow = HorizontalWrapMode.Wrap;
            messageTextField.verticalOverflow = VerticalWrapMode.Truncate;
            messageTextField.resizeTextForBestFit = true;
            messageTextField.resizeTextMinSize = 1;
            messageTextField.resizeTextMaxSize = 15;
            messageTextField.color = Color.white;
            messageTextField.material = null;
            messageTextField.raycastTarget = true;

            messageTextFieldShadowEffect.effectColor = effectColor;
            messageTextFieldShadowEffect.effectDistance = new Vector2(1.5f, -1.5f);
            messageTextFieldShadowEffect.useGraphicAlpha = true;

            messageTextFieldOutlineEffect.effectColor = effectColor;
            messageTextFieldOutlineEffect.effectDistance = new Vector2(0.75f, 0.75f);
            messageTextFieldOutlineEffect.useGraphicAlpha = true;

            // Creating message image.
            GameObject messageImageObject = new GameObject("Message Image", typeof(RectTransform));
            messageImageObject.transform.SetParent(messageRect);

            // Add required components.
            messageImage = messageImageObject.AddComponent<Image>();

            // Add required effects.
            Shadow messageImageShadowEffect = messageImageObject.AddComponent<Shadow>();
            Outline messageImageOutlineEffect = messageImageObject.AddComponent<Outline>();

            // Setup required components.
            RectTransform messageImageRect = messageImageObject.GetComponent<RectTransform>();
            messageImageRect.localPosition = Vector3.zero;
            messageImageRect.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            messageImageRect.pivot = Vector2.one / 2;
            messageImageRect.anchorMin = new Vector2(0.359f, 0.028f);
            messageImageRect.anchorMax = new Vector2(0.652f, 0.442f);
            messageImageRect.anchoredPosition = Vector3.zero;
            messageImageRect.sizeDelta = new Vector2(322.39f, 38.358f);

            messageImage.sprite = null;
            messageImage.color = Color.white;
            messageImage.material = null;
            messageImage.raycastTarget = true;
            messageImage.type = Image.Type.Simple;
            messageImage.useSpriteMesh = false;
            messageImage.preserveAspect = true;

            messageImageShadowEffect.effectColor = effectColor;
            messageImageShadowEffect.effectDistance = new Vector2(-1.5f, -1.5f);
            messageImageShadowEffect.useGraphicAlpha = true;

            messageImageOutlineEffect.effectColor = effectColor;
            messageImageOutlineEffect.effectDistance = new Vector2(0.5f, 0.5f);
            messageImageOutlineEffect.useGraphicAlpha = true;

            //Disable message panel
            messagePanel.gameObject.SetActive(false);
        }

        /// <summary>
        /// HUD enabled state.
        /// </summary>
        public void Enabled(bool value)
        {
            rootHUDRect.gameObject.SetActive(value);
        }

        /// <summary>
        /// Display health elements.
        /// </summary>
        public void DisplayHealthElements()
        {
            healthRect.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide health elements.
        /// </summary>
        public void HideHealthElements()
        {
            healthRect.gameObject.SetActive(false);
        }

        /// <summary>
        /// Display weapon elements.
        /// </summary>
        public void DisplayWeaponElements()
        {
            weaponRect.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide weapon elements.
        /// </summary>
        public void HideWeaponElements()
        {
            weaponRect.gameObject.SetActive(false);
        }

        /// <summary>
        /// Display message elements.
        /// </summary>
        public void DisplayMessage(string message, Sprite messageImage = null)
        {
            if(messageRect != null)
            {
                messageRect.gameObject.SetActive(true);
            }

            if(messageTextField != null)
            {
                this.messageTextField.text = message;
            }

            if (messageImage != null)
            {
                this.messageImage.sprite = messageImage;
                this.messageImage.gameObject.SetActive(true);
            }
            else
            {
                this.messageImage.sprite = null;
                this.messageImage.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Hide message elements.
        /// </summary>
        public void HideMessage()
        {
            if (messageRect != null)
            {
                messageRect.gameObject.SetActive(false);
            }

            if (messageTextField != null)
            {
                this.messageTextField.text = "";
            }

            if (messageImage != null)
            {
                this.messageImage.sprite = null;
                this.messageImage.gameObject.SetActive(false);
            }
        }

        public Text CreateGrenadeElement(string name, int count, Sprite grenadeSprite)
        {
            GameObject grenadeObject = GameObject.Instantiate(grenadeTemplate.gameObject, grenadeGroup);
            grenadeObject.name = name;
            Image grenadeImage = grenadeObject.GetComponentInChildren<Image>();
            Text grenadeCounter = grenadeObject.GetComponentInChildren<Text>();
            grenadeImage.sprite = grenadeSprite;
            grenadeCounter.text = count.ToString();
            grenadeObject.SetActive(true);
            return grenadeCounter;
        }

        #region [Getter / Setter]
        public RectTransform GetRootHUDRect()
        {
            return rootHUDRect;
        }

        public void SetRootHUDRect(RectTransform value)
        {
            rootHUDRect = value;
        }

        public RectTransform GetHealthRect()
        {
            return healthRect;
        }

        public void SetHealthRect(RectTransform value)
        {
            healthRect = value;
        }

        public Slider GetHealthSlider()
        {
            return healthSlider;
        }

        public void SetHealthSlider(Slider value)
        {
            healthSlider = value;
        }

        public Text GetHealthCounter()
        {
            return healthCounter;
        }

        public void SetHealthCounter(Text value)
        {
            healthCounter = value;
        }

        public RectTransform GetWeaponRect()
        {
            return weaponRect;
        }

        public void SetWeaponRect(RectTransform value)
        {
            weaponRect = value;
        }

        public Image GetWeaponImage()
        {
            return weaponImage;
        }

        public void SetWeaponImage(Image value)
        {
            weaponImage = value;
        }

        public Text GetWeaponName()
        {
            return weaponName;
        }

        public void SetWeaponName(Text value)
        {
            weaponName = value;
        }

        public Text GetBulletCounter()
        {
            return bulletCounter;
        }

        public void SetBulletCounter(Text value)
        {
            bulletCounter = value;
        }

        public Text GetClipCounter()
        {
            return clipCounter;
        }

        public void SetClipCounter(Text value)
        {
            clipCounter = value;
        }

        public Text GetFireMode()
        {
            return fireMode;
        }

        public void SetFireMode(Text value)
        {
            fireMode = value;
        }

        public RectTransform GetGrenadeGroup()
        {
            return grenadeGroup;
        }

        public void SetGrenadeGroup(RectTransform value)
        {
            grenadeGroup = value;
        }

        public RectTransform GetGrenadeTemplate()
        {
            return grenadeTemplate;
        }

        public void SetGrenadeTemplate(RectTransform value)
        {
            grenadeTemplate = value;
        }

        public RectTransform GetMessageRect()
        {
            return messageRect;
        }

        public void SetMessageRect(RectTransform value)
        {
            messageRect = value;
        }

        public Text GetMessageTextField1()
        {
            return messageTextField;
        }

        public void SetMessageTextField1(Text value)
        {
            messageTextField = value;
        }

        public Text GetMessageTextField()
        {
            return messageTextField;
        }

        public void SetMessageTextField(Text value)
        {
            messageTextField = value;
        }

        #endregion
    }
}