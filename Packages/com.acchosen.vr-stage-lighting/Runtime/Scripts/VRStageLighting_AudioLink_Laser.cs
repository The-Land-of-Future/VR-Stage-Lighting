using UnityEngine;
//using UnityEngine.UI;

#if UDONSHARP
using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;
#endif

#if UDONSHARP
using PropertyToIdClass = VRC.SDKBase.VRCShader;
#else
using PropertyToIdClass = UnityEngine.Shader;
#endif

#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEditor;

#if UDONSHARP
using UdonSharpEditor;
using VRC.Udon.Common;
using VRC.Udon.Common.Interfaces;
#endif

#endif
namespace VRSL
{
#if UDONSHARP
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VRStageLighting_AudioLink_Laser : UdonSharpBehaviour
#else
    public class VRStageLighting_AudioLink_Laser : MonoBehaviour
#endif
    {
        // ReSharper disable InconsistentNaming
        private int Link = PropertyToIdClass.PropertyToID("_EnableAudioLink");
        private int EnableColorChord = PropertyToIdClass.PropertyToID("_EnableColorChord");
        private int Delay1 = PropertyToIdClass.PropertyToID("_Delay");
        private int Multiplier = PropertyToIdClass.PropertyToID("_BandMultiplier");
        private int Band1 = PropertyToIdClass.PropertyToID("_Band");
        private int TextureColorSampleX = PropertyToIdClass.PropertyToID("_TextureColorSampleX");
        private int TextureColorSampleY = PropertyToIdClass.PropertyToID("_TextureColorSampleY");
        private int EnableColorTextureSample = PropertyToIdClass.PropertyToID("_EnableColorTextureSample");
        private int UseTraditionalSampling = PropertyToIdClass.PropertyToID("_UseTraditionalSampling");
        private int EnableThemeColorSampling = PropertyToIdClass.PropertyToID("_EnableThemeColorSampling");
        private int ColorTarget = PropertyToIdClass.PropertyToID("_ThemeColorTarget");
        private int Emission = PropertyToIdClass.PropertyToID("_Emission");
        private int UseAnimatedEmission = PropertyToIdClass.PropertyToID("_UseAnimatedEmission");
        private int VertexConeWidth = PropertyToIdClass.PropertyToID("_VertexConeWidth");
        private int Intensity = PropertyToIdClass.PropertyToID("_GlobalIntensity");
        private int FinalIntensity1 = PropertyToIdClass.PropertyToID("_FinalIntensity");
        private int VertexConeLength = PropertyToIdClass.PropertyToID("_VertexConeLength");
        private int ZConeFlatness = PropertyToIdClass.PropertyToID("_ZConeFlatness");
        private int XRotation = PropertyToIdClass.PropertyToID("_XRotation");
        private int YRotation = PropertyToIdClass.PropertyToID("_YRotation");
        private int ZRotation = PropertyToIdClass.PropertyToID("_ZRotation");
        private int Count = PropertyToIdClass.PropertyToID("_LaserCount");
        private int Thickness = PropertyToIdClass.PropertyToID("_LaserThickness");
        private int Scroll = PropertyToIdClass.PropertyToID("_Scroll");
        // ReSharper restore InconsistentNaming

        //////////////////Public Variables////////////////////

        [Header("Audio Link Settings")]
        [SerializeField,
         Tooltip("Enable or disable Audio Link Reaction for this fixture.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(EnableAudioLink))
#endif
        ]
        private bool enableAudioLink;


        //[Tooltip("The Audio Link Script to react to.")]
        //public AudioLink audioLink;


        [SerializeField,
         Tooltip("The frequency band of the spectrum to react to.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(Band))
#endif
        ]
        private AudioLinkBandState band;


        [SerializeField, Range(0, 31),
         Tooltip("The level of delay to add to the reaction.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(Delay))
#endif
        ]
        private int delay;


        [SerializeField, Range(1.0f, 15.0f),
         Tooltip("Multiplier for the sensativity of the reaction.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(BandMultiplier))
#endif
        ]
        private float bandMultiplier = 1.0f;


        [SerializeField,
         Tooltip("Enable Color Chord tinting of the light emission.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(ColorChord))
#endif
        ]
        private bool enableColorChord;

        [Header("General Settings")]
        [SerializeField, Range(0, 1),
         Tooltip("Sets the overall intensity of the shader. Good for animating or scripting effects related to intensity. Its max value is controlled by Final Intensity.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(GlobalIntensity))
#endif
        ]
        private float globalIntensity = 1; 

        [SerializeField, Range(0, 1),
         Tooltip("Sets the maximum brightness value of Global Intensity. Good for personalized settings of the max brightness of the shader by other users via UI.")
#if UDONSHARP
        ,FieldChangeCallback(nameof(FinalIntensity))
#endif
        ]
        private float finalIntensity = 1;


        [SerializeField, ColorUsage(false, false),
         Tooltip("The main color of the light.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(LightColorTint))
#endif
        ]
        private Color lightColorTint = Color.white * 2.0f;
        [SerializeField, 
#if UDONSHARP
         FieldChangeCallback(nameof(LightColorTintAnimated)),
#endif
         Tooltip("Should the shader use the Animated Light Color?")
        ]
        private bool lightColorTintAnimated = false;


        [SerializeField,
         Tooltip("Check this box if you wish to sample separate texture for the color. The color will be influenced by the intensity of the original emission color! The texture is set in the shader itself.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ColorTextureSampling))
#endif
        ]
        private bool enableColorTextureSampling;

        [SerializeField,
         Tooltip("Check this box if you wish to use traditional color sampling instead of white to black conversion")
#if UDONSHARP
            ,FieldChangeCallback(nameof(TraditionalColorTextureSampling))
#endif
        ]
        private bool traditionalColorTextureSampling;

        [SerializeField,
         Tooltip("The UV coordinates to sample the color from on the texture.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(TextureSamplingCoordinates))
#endif
        ]
        private Vector2 textureSamplingCoordinates = new Vector2(0.5f, 0.5f);

        
        [SerializeField,
         Tooltip("Check this box if you wish to enable AudioLink Theme colors.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ThemeColorSampling))
#endif
        ]
        private bool enableThemeColorSampling;


        [SerializeField, Range(1, 4),
         Tooltip("Theme Color to Sample from.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ThemeColorTarget))
#endif
        ]
        private int themeColorTarget = 1;

        [SerializeField, Range(-3.75f, 20.0f),
         Tooltip("Controls the radius of the laser cone.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeWidth))
#endif
        ]
        private float coneWidth = 2.5f;

        [SerializeField, Range(-0.5f, 5.0f),
         Tooltip("Controls the length of the laser cone")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeLength))
#endif
        ]
        private float coneLength = 8.5f; 

        [SerializeField, Range(0.0f, 1.999f),
         Tooltip("Controls how flat or round the cone is.")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeFlatness))
#endif
        ]
        private float coneFlatness = 0.0f;

        [SerializeField, Range(-90.0f, 90.0f),
         Tooltip("X rotation coffset for cone")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeXRotation))
#endif
        ]
        private float coneXRotation = 0.0f; 

        [SerializeField, Range(-90.0f, 90.0f),
         Tooltip("Y rotation offset for cone")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeYRotation))
#endif
        ]
        private float coneYRotation = 0.0f;

        [SerializeField, Range(-90.0f, 90.0f),
         Tooltip("Z rotation offset for cone")
#if UDONSHARP
            ,FieldChangeCallback(nameof(ConeZRotation))
#endif
        ]
        private float coneZRotation = 0.0f;  

        [SerializeField, Range(4.0f, 68f),
         Tooltip("Number of laser beams in cone")
#if UDONSHARP
            ,FieldChangeCallback(nameof(LaserCount))
#endif
        ]
        private int laserCount = 14;  

        [SerializeField, Range(0.003f, 0.25f),
         Tooltip("Controls how thick/thin the lasers are")
#if UDONSHARP
            ,FieldChangeCallback(nameof(LaserThickness))
#endif
        ]
        private float laserThickness = 0.125f; 

        [SerializeField, Range(-1.0f, 1.0f),
         Tooltip("Controls the speed of laser scroll animation. Negative goes left, positive goes right, 0 means no scroll")
#if UDONSHARP
            ,FieldChangeCallback(nameof(LaserScroll))
#endif
        ]
        private float laserScroll = 0.0f; 


        [Header("Mesh Settings")]
        [Tooltip ("The meshes used to make up the light. You need atleast 1 mesh in this group for the script to work properly.")]
        public MeshRenderer[] objRenderers;


        private float previousConeWidth, previousConeLength, previousGlobalIntensity, previousFinalIntensity, previousConeFlatness, previousConeXRotation, previousConeYRotation, previousConeZRotation, previousLaserThickness, previousLaserScroll;
        private int previousLaserCount;
        private Color previousColorTint;
        private bool previousColorTintAnimated;
        MaterialPropertyBlock props;

        [HideInInspector]
        public bool foldout;



        ////////////////////////////////////////////////////////////////////////////////

        public bool EnableAudioLink
        {
            get
            {
                return enableAudioLink;
            }
            set
            {
                enableAudioLink = value;
                _UpdateInstancedProperties();
            }
        }

        public bool ColorChord
        {
            get
            {
                return enableColorChord;
            }
            set
            {
                enableColorChord = value;
                _UpdateInstancedProperties();
            }
        }

        public AudioLinkBandState Band
        {
            get
            {
                return band;
            }
            set
            {
                band = value;
                _UpdateInstancedProperties();
            }
        }
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
                _UpdateInstancedProperties();
            }
        }
        public float BandMultiplier
        {
            get
            {
                return bandMultiplier;
            }
            set
            {
                bandMultiplier = value;
                _UpdateInstancedProperties();
            }
        }
            public Color LightColorTint
        {
            get
            {
                return lightColorTint;
            }
            set
            {
                previousColorTint = lightColorTint;
                lightColorTint = value;
                _UpdateInstancedProperties();
            }
        }
        public bool LightColorTintAnimated
        {
            get
            {
                return lightColorTintAnimated;
            }
            set
            {
                previousColorTintAnimated = lightColorTintAnimated;
                lightColorTintAnimated = value;
                _UpdateInstancedProperties();
            }
        }
        public float ConeWidth
        {
            get
            {
                return coneWidth;
            }
            set
            {
                previousConeWidth = coneWidth;
                coneWidth = value;
                _UpdateInstancedProperties();
            }
        }
        public float ConeLength
        {
            get
            {
                return coneLength;
            }
            set
            {
                previousConeLength = coneLength;
                coneLength = value;
                _UpdateInstancedProperties();
            }
        }
        public float GlobalIntensity
        {
            get
            {
                return globalIntensity;
            }
            set
            {
                previousGlobalIntensity = globalIntensity;
                globalIntensity = value;
                _UpdateInstancedProperties();
            }
        }
        public float FinalIntensity
        {
            get
            {
                return finalIntensity;
            }
            set
            {
                previousFinalIntensity = finalIntensity;
                finalIntensity = value;
                _UpdateInstancedProperties();
            }
        }
        
        public float ConeFlatness
        {
            get
            {
                return coneFlatness;
            }
            set
            {
                previousConeFlatness = coneFlatness;
                coneFlatness = value;
                _UpdateInstancedProperties();
            }
        }
        public float ConeXRotation
        {
            get
            {
                return coneXRotation;
            }
            set
            {
                previousConeXRotation = coneXRotation;
                coneXRotation = value;
                _UpdateInstancedProperties();
            }
        }
        public float ConeYRotation
        {
            get
            {
                return coneYRotation;
            }
            set
            {
                previousConeYRotation = coneYRotation;
                coneYRotation = value;
                _UpdateInstancedProperties();
            }
        }
        public float ConeZRotation
        {
            get
            {
                return coneZRotation;
            }
            set
            {
                previousConeZRotation = coneZRotation;
                coneZRotation = value;
                _UpdateInstancedProperties();
            }
        }
        public int LaserCount
        {
            get
            {
                return laserCount;
            }
            set
            {
                previousLaserCount = laserCount;
                laserCount = value;
                _UpdateInstancedProperties();
            }
        }
        public float LaserThickness
        {
            get
            {
                return laserThickness;
            }
            set
            {
                previousLaserThickness = laserThickness;
                laserThickness = value;
                _UpdateInstancedProperties();
            }
        }
        public float LaserScroll
        {
            get
            {
                return laserScroll;
            }
            set
            {
                previousLaserScroll = laserScroll;
                laserScroll = value;
                _UpdateInstancedProperties();
            }
        }
        public bool ColorTextureSampling
        {
            get
            {
                return enableColorTextureSampling;
            }
            set
            {
                enableColorTextureSampling = value;
                _UpdateInstancedProperties();
            }
        }
        public bool TraditionalColorTextureSampling
        {
            get
            {
                return traditionalColorTextureSampling;
            }
            set
            {
                traditionalColorTextureSampling = value;
                _UpdateInstancedProperties();
            }
        }
        public Vector2 TextureSamplingCoordinates
        {
            get
            {
                return textureSamplingCoordinates;
            }
            set
            {
                textureSamplingCoordinates = value;
                _UpdateInstancedProperties();
            }
        }

        public bool ThemeColorSampling
            {
                get
                {
                    return enableThemeColorSampling;
                }
                set
                {
                    enableThemeColorSampling = value;
                    _UpdateInstancedProperties();
                }
            }
            public int ThemeColorTarget
            {
                get
                {
                    return themeColorTarget;
                }
                set
                {
                    themeColorTarget = value;
                    _UpdateInstancedProperties();
                }

            }

        ///////////////////////////////////////////////////////////

        void OnEnable() 
        {
            Init(true);
        }

        void Start()
        {
            Link = PropertyToIdClass.PropertyToID("_EnableAudioLink");
            EnableColorChord = PropertyToIdClass.PropertyToID("_EnableColorChord");
            Delay1 = PropertyToIdClass.PropertyToID("_Delay");
            Multiplier = PropertyToIdClass.PropertyToID("_BandMultiplier");
            Band1 = PropertyToIdClass.PropertyToID("_Band");
            TextureColorSampleX = PropertyToIdClass.PropertyToID("_TextureColorSampleX");
            TextureColorSampleY = PropertyToIdClass.PropertyToID("_TextureColorSampleY");
            EnableColorTextureSample = PropertyToIdClass.PropertyToID("_EnableColorTextureSample");
            UseTraditionalSampling = PropertyToIdClass.PropertyToID("_UseTraditionalSampling");
            EnableThemeColorSampling = PropertyToIdClass.PropertyToID("_EnableThemeColorSampling");
            ColorTarget = PropertyToIdClass.PropertyToID("_ThemeColorTarget");
            Emission = PropertyToIdClass.PropertyToID("_Emission");
            UseAnimatedEmission = PropertyToIdClass.PropertyToID("_UseAnimatedEmission");
            VertexConeWidth = PropertyToIdClass.PropertyToID("_VertexConeWidth");
            Intensity = PropertyToIdClass.PropertyToID("_GlobalIntensity");
            FinalIntensity1 = PropertyToIdClass.PropertyToID("_FinalIntensity");
            VertexConeLength = PropertyToIdClass.PropertyToID("_VertexConeLength");
            ZConeFlatness = PropertyToIdClass.PropertyToID("_ZConeFlatness");
            XRotation = PropertyToIdClass.PropertyToID("_XRotation");
            YRotation = PropertyToIdClass.PropertyToID("_YRotation");
            ZRotation = PropertyToIdClass.PropertyToID("_ZRotation");
            Count = PropertyToIdClass.PropertyToID("_LaserCount");
            Thickness = PropertyToIdClass.PropertyToID("_LaserThickness");
            Scroll = PropertyToIdClass.PropertyToID("_Scroll");
            Init(true);
        }

        public void _SetProps()
        {
            props = new MaterialPropertyBlock();
        }
        public void _UpdateInstancedProperties()
        {
            if(props == null)
            {
                if(objRenderers.Length > 0 && objRenderers[0] != null)
                {
                    _SetProps();
                }
                else
                {
                    Debug.Log("Please add atleast one fixture renderer.");
                    return;
                }
            }
            //AudioLink Stuff
            props.SetFloat(Link, enableAudioLink ? 1.0f : 0.0f);
            props.SetInt(EnableColorChord, enableColorChord ? 1 : 0);
            //props.SetFloat("_NumBands", spectrumBands.Length);
            props.SetFloat(Delay1, delay);
            props.SetFloat(Multiplier, bandMultiplier);
            int b = (int) band;
            float ba = 1.0f * b;
            props.SetFloat(Band1, ba);
            //Color Texture Sampling
            props.SetFloat(TextureColorSampleX, textureSamplingCoordinates.x);
            props.SetFloat(TextureColorSampleY, textureSamplingCoordinates.y);
            props.SetInt(EnableColorTextureSample, enableColorTextureSampling ? 1 : 0);
            props.SetInt(UseTraditionalSampling, traditionalColorTextureSampling ? 1 : 0);
            props.SetInt(EnableThemeColorSampling, enableThemeColorSampling ? 1 : 0);
            props.SetInt(ColorTarget, themeColorTarget);
            //General Light Stuff
            props.SetColor(Emission, lightColorTint);
            props.SetInt(UseAnimatedEmission, lightColorTintAnimated?1:0);
            props.SetFloat(VertexConeWidth, coneWidth);
            props.SetFloat(Intensity, globalIntensity);
            props.SetFloat(FinalIntensity1, finalIntensity);
            props.SetFloat(VertexConeLength, coneLength);
            props.SetFloat(ZConeFlatness, coneFlatness);
            props.SetFloat(XRotation, coneXRotation);
            props.SetFloat(YRotation, coneYRotation);
            props.SetFloat(ZRotation, coneZRotation);
            props.SetInt(Count, laserCount);
            props.SetFloat(Thickness, laserThickness);
            props.SetFloat(Scroll, laserScroll);
            // for(int i = 0; i < objRenderers.Length; i++)
            // {
            //     objRenderers[i].SetPropertyBlock(props);
            // }
            switch(objRenderers.Length)
            {
                case 1:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    break;
                case 2:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    break;
                case 3:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    break;
                case 4:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    if(objRenderers[3])
                        objRenderers[3].SetPropertyBlock(props);
                    break;
                case 5:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    if(objRenderers[3])
                        objRenderers[3].SetPropertyBlock(props);
                    if(objRenderers[4])
                        objRenderers[4].SetPropertyBlock(props);
                    break;
                default:
                    Debug.Log("Too many mesh renderers for this fixture!");
                    break;   
            }  
        }

        public void _UpdateInstancedPropertiesSansAudioLink()
        {
            if(props == null)
            {
                if(objRenderers.Length > 0 && objRenderers[0] != null)
                {
                    _SetProps();
                }
            }
            //AudioLink Stuff
            props.SetFloat(Link, 0.0f);
            props.SetInt(EnableColorChord, 0);
            //props.SetFloat("_NumBands", spectrumBands.Length);
            props.SetFloat(Delay1, delay);
            props.SetFloat(Multiplier, bandMultiplier);
            int b = (int) band;
            float ba = 1.0f * b;
            props.SetFloat(Band1, ba);
            //Color Texture Sampling
            props.SetFloat(TextureColorSampleX, textureSamplingCoordinates.x);
            props.SetFloat(TextureColorSampleY, textureSamplingCoordinates.y);
            props.SetInt(EnableColorTextureSample, enableColorTextureSampling ? 1 : 0);
            props.SetInt(UseTraditionalSampling, traditionalColorTextureSampling ? 1 : 0);
            props.SetInt(EnableThemeColorSampling, enableThemeColorSampling ? 1 : 0);
            props.SetInt(ColorTarget, themeColorTarget);
            //General Light Stuff
            props.SetColor(Emission, lightColorTint);
            props.SetInt(UseAnimatedEmission, lightColorTintAnimated?1:0);
            props.SetFloat(VertexConeWidth, coneWidth);
            props.SetFloat(Intensity, globalIntensity);
            props.SetFloat(FinalIntensity1, finalIntensity);
            props.SetFloat(VertexConeLength, coneLength);
            props.SetFloat(ZConeFlatness, coneFlatness);
            props.SetFloat(XRotation, coneXRotation);
            props.SetFloat(YRotation, coneYRotation);
            props.SetFloat(ZRotation, coneZRotation);
            props.SetInt(Count, laserCount);
            props.SetFloat(Thickness, laserThickness);
            props.SetFloat(Scroll, laserScroll);
            // for(int i = 0; i < objRenderers.Length; i++)
            // {
            //     objRenderers[i].SetPropertyBlock(props);
            // }
            switch(objRenderers.Length)
            {
                case 1:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    break;
                case 2:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    break;
                case 3:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    break;
                case 4:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    if(objRenderers[3])
                        objRenderers[3].SetPropertyBlock(props);
                    break;
                case 5:
                    if(objRenderers[0])
                        objRenderers[0].SetPropertyBlock(props);
                    if(objRenderers[1])
                        objRenderers[1].SetPropertyBlock(props);
                    if(objRenderers[2])
                        objRenderers[2].SetPropertyBlock(props);
                    if(objRenderers[3])
                        objRenderers[3].SetPropertyBlock(props);
                    if(objRenderers[4])
                        objRenderers[4].SetPropertyBlock(props);
                    break;
                default:
                    Debug.Log("Too many mesh renderers for this fixture!");
                    break;  
            }  
        }
        void Init(bool withAL)
        {
            if(objRenderers[0] == null)
            {
                return;
            }
            if(objRenderers.Length > 0)
            {
                _SetProps();
                previousColorTint = lightColorTint;
                previousConeWidth = coneWidth;
                previousConeLength = coneLength;
                previousGlobalIntensity = globalIntensity;
                previousFinalIntensity = finalIntensity;
                if(withAL)
                {
                    _UpdateInstancedProperties();
                }
                else
                {
                    _UpdateInstancedPropertiesSansAudioLink();
                }
            }
            else
            {
                Debug.Log("Please add atleast one fixture renderer.");
                //enableInstancing = false;
            }
        }

        #if !COMPILER_UDONSHARP && UNITY_EDITOR
                void OnValidate()
            {
                Event e = Event.current;
        
                if (e != null)
                {
                    if (e.type == EventType.ExecuteCommand && e.commandName == "Duplicate")
                    {
                        Init(false);
                    }
                }
            }
        #endif

    }
}
