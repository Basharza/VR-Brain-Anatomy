using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;
using Plane = UnityEngine.Plane;
using UnityEngine.XR;

[RequireComponent(typeof(Camera))]
public class RayMarching : MonoBehaviour
{
    [SerializeField]
    [Header("Render in a lower resolution to increase performance.")]
    private int downscale = 1;
    [SerializeField]
    private LayerMask volumeLayer;

    [SerializeField]
    private float _depthBlend = 20;

    [SerializeField]
    private Shader compositeShader;

    [SerializeField]
    private Shader renderFrontDepthShader;
    [SerializeField]
    private Shader renderBackDepthShader;

    [SerializeField]
    private Shader renderFrontPosShader;
    [SerializeField]
    private Shader renderBackPosShader;
    [SerializeField]
    private Shader rayMarchShader;

    [SerializeField]
    [Header("Remove all the darker colors")]
    private bool increaseVisiblity = false;

    [SerializeField]
    private Texture2D _noiseTex;

    [Header("Drag all the textures in here")]
    [SerializeField]
    private Texture2D[] slices;
    [SerializeField]
    [Range(0, 2)]
    private float opacity = 1;
    [Header("Volume texture size. These must be a power of 2")]
    [SerializeField]
    private int volumeWidth = 512;
    [SerializeField]
    private int volumeHeight = 512;
    [SerializeField]
    private int volumeDepth = 512;
    [Header("Clipping planes percentage")]
    [SerializeField]
    private Vector4 clipDimensions = new Vector4(100, 100, 100, 0);

    private Material _rayMarchMaterial;
    private Material _compositeMaterial;
    private Camera _ppCamera;
    private Camera _ppCamera2;
    private Texture3D _volumeBuffer;
    private Texture3D _volumeBufferDATA;
    [SerializeField]
    private Transform slicePlaneTransform;
    [SerializeField]
    private LayerMask sliceInteractionLayer;
    [SerializeField]
    private float slicespeed = 0.1f;
    [SerializeField]
    private float SliceThreshold = 0.5f;
    [Header("Clipping direction (1 for bottom to top, -1 for top to bottom)")]
    [SerializeField]
    private int clippingDirection = 1;
    [SerializeField]
    private GameObject PlaneSpawn;

    [SerializeField]
    private GameObject[] instances;
    [SerializeField]
    private AnimationCurve curve;
    
    public Camera _camera2;
    public List<Transform> clipPlanes = new List<Transform>();
    private void Awake()
    {
        _rayMarchMaterial = new Material(rayMarchShader);
        _compositeMaterial = new Material(compositeShader);

        gameObject.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        if(_camera2 != null)
        _camera2.depthTextureMode |= DepthTextureMode.DepthNormals;
        if (gameObject.name.Equals("Camera"))
        {
            GeneratePlaneBOTTOM();
            GeneratePlaneBOTTOMDIAGONAL();
        }
      
    }
 
    private void Start()
    {

        GenerateVolumeTexture();
        if (gameObject.name.Equals("Camera2"))
        {
            GameObject BoTTOMDIAGONALPlane = GameObject.Find("Plane - BOTTOMDIAGONAL");
            clipPlanes.Insert(2, BoTTOMDIAGONALPlane.transform);

            GameObject BoTTOMPlane = GameObject.Find("Plane - BOTTOM");
            clipPlanes.Insert(1, BoTTOMPlane.transform);
        }
    }

    private void OnDestroy()
    {
        if (_volumeBuffer != null)
        {
            Destroy(_volumeBuffer);
        }
    }


    
   //ublic List<Transform> clipPlanes = new List<Transform>();
    [SerializeField]
    private Transform cubeTarget;
    [SerializeField]
    private Texture2D Mask;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
     
        _rayMarchMaterial.SetTexture("_VolumeTex", _volumeBuffer);
      //  _rayMarchMaterial.SetTexture("_2DMaskTexture", Mask);
        //  _rayMarchMaterial.SetVector("_SlicePlanePosition", slicePlaneTransform.position);
        // _rayMarchMaterial.SetVector("_SlicePlaneNormal", slicePlaneTransform.up);
        // _rayMarchMaterial.SetFloat("_SliceThreshold", SliceThreshold);

        var width = source.width / downscale;
        var height = source.height / downscale;

        if (_ppCamera == null  )
        {
            var go = new GameObject("PPCamera");
            _ppCamera = go.AddComponent<Camera>();
            _ppCamera.enabled = false;
            _ppCamera.renderingPath = RenderingPath.VertexLit;
            _ppCamera.useOcclusionCulling = false;
        }

       

        _ppCamera.CopyFrom(GetComponent<Camera>());
        _ppCamera.clearFlags = CameraClearFlags.SolidColor;

        _ppCamera.backgroundColor = Color.white;
        _ppCamera.cullingMask = volumeLayer;
        

        var frontPos = RenderTexture.GetTemporary((int)width, (int)height, 0, RenderTextureFormat.ARGBFloat);
        var backPos = RenderTexture.GetTemporary((int)width, (int)height, 0, RenderTextureFormat.ARGBFloat);
        var volumeTarget = RenderTexture.GetTemporary((int)width, (int)height, 0);

        RenderTexture.active = volumeTarget;
        GL.Clear(false, true, Color.clear);
        RenderTexture.active = null;

        // need to set this vector because unity bakes object that are non uniformily scaled
        //TODO:FIX
        //Shader.SetGlobalVector("_VolumeScale", cubeTarget.transform.localScale);

        // Render depths
        _ppCamera.targetTexture = frontPos;
        _ppCamera.RenderWithShader(renderFrontPosShader, "RenderType");
        _ppCamera.targetTexture = backPos;
        _ppCamera.RenderWithShader(renderBackPosShader, "RenderType");

       

        // Render volume
        _rayMarchMaterial.SetTexture("_FrontTex", frontPos);
        _rayMarchMaterial.SetTexture("_BackTex", backPos);
        _rayMarchMaterial.SetFloat("_FadeAmount", _depthBlend);

        for (int i = 0; i < clipPlanes.Count; i++)
        {
            if (cubeTarget != null && clipPlanes[i] != null && clipPlanes[i].gameObject.activeSelf)
            {
                var p = new Plane(
                    cubeTarget.InverseTransformDirection(clipPlanes[i].transform.up),
                    cubeTarget.InverseTransformPoint(clipPlanes[i].position));

                string planeVarName = "_ClipPlane" + i.ToString();
               // _rayMarchMaterial.SetVector(planeVarName, new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance));
                _rayMarchMaterial.SetVector(planeVarName, new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance));
            }
        }

        _rayMarchMaterial.SetTexture("_NoiseTex", _noiseTex);
        _rayMarchMaterial.SetFloat("_Opacity", opacity); // Blending strength 
        _rayMarchMaterial.SetVector("_ClipDims", clipDimensions / 100f); // Clip box
        _rayMarchMaterial.SetMatrix("_ObjectToView", GetComponent<Camera>().worldToCameraMatrix * cubeTarget.localToWorldMatrix);

        Graphics.Blit(null, volumeTarget, _rayMarchMaterial);

        _ppCamera.Render();
       

        //Composite
        _compositeMaterial.SetTexture("_BlendTex", volumeTarget);
        Graphics.Blit(source, destination, _compositeMaterial);

        RenderTexture.ReleaseTemporary(volumeTarget);
        RenderTexture.ReleaseTemporary(frontPos);
        RenderTexture.ReleaseTemporary(backPos);
    }

    private void GenerateVolumeTexture()
    {
        // sort
        //System.Array.Sort(slices, (x, y) => x.name.CompareTo(y.name));

        // use a bunch of memory!
        _volumeBuffer = new Texture3D(volumeWidth, volumeHeight, volumeDepth, TextureFormat.ARGB32, false);

        var w = _volumeBuffer.width;
        var h = _volumeBuffer.height;
        var d = _volumeBuffer.depth;

        // skip some slices if we can't fit it all in
        var countOffset = (slices.Length - 1) / (float)d;

        var volumeColors = new Color[w * h * d];

        var sliceCount = 0;
        var sliceCountFloat = 0f;
        for (int z = 0; z < d; z++)
        {
            sliceCountFloat += countOffset;
            sliceCount = Mathf.FloorToInt(sliceCountFloat);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    var idx = x + (y * w) + (z * (w * h));
                    volumeColors[idx] = slices[sliceCount].GetPixelBilinear(x / (float)w, y / (float)h);
                    if (increaseVisiblity)
                    {
                        volumeColors[idx].a *= volumeColors[idx].r;
                    }
                }
            }
        }

        _volumeBuffer.SetPixels(volumeColors);
        _volumeBuffer.Apply();

        _rayMarchMaterial.SetTexture("_VolumeTex", _volumeBuffer);
    }


  
  
    public void GeneratePlaneBOTTOM()
    {
        if (clipPlanes.Count == 4)
            return;
        GameObject clone = Instantiate(PlaneSpawn);
        clone.transform.localPosition = new Vector3(26.1f, -0.869f, -8.8f);
        clone.name = "Plane - BOTTOM";
        // setPlanes(clone.transform);
        clipPlanes.Insert(1, clone.transform);
        for (int i = 0; i < clipPlanes.Count; i++)
        {
            instances[i].SetActive(true);


        }
    }
    public void GeneratePlaneBOTTOMDIAGONAL()
    {
        if (clipPlanes.Count == 4)
            return;
        GameObject clone = Instantiate(PlaneSpawn);
         clone.transform.localPosition = new Vector3(26.1f, -50f, -8.8f);
        clone.name = "Plane - BOTTOMDIAGONAL";

        // clone.transform.transform.localEulerAngles = new Vector3(40f, clone.transform.position.y, clone.transform.position.z);
        // setPlanes(clone.transform);
        clipPlanes.Insert(3, clone.transform);
        for (int i = 0; i < clipPlanes.Count; i++)
        {
            instances[i].SetActive(true);


        }
    }
    public List<Transform> GetPlanes()
    {
        return clipPlanes;
    }
    public GameObject[] GetInstances() {
        return instances;
    }
    public Transform getTarget()
    {
        return cubeTarget;
    }
    public Material getmat()
    {
        return _rayMarchMaterial;
    }
}
