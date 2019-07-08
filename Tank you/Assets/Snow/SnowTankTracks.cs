using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTankTracks : MonoBehaviour
{
    private RenderTexture _splatmap;
    private Material _snowMaterial; 
    private Material _drawMaterial;
    private RaycastHit _hit;
    private TankManagerV2 tankManager;

    public GameObject _terrain;
    public Shader _drawShader;

    [Range(0, 10)]
    public float _brushSize;

    [Range(0, 1)]
    public float _brushStrength;

    void Start()
    {
        _drawMaterial = new Material(_drawShader);
        _snowMaterial = GameObject.Find("Snow").GetComponent<MeshRenderer>().material;
        _snowMaterial.SetTexture("_Splat", _splatmap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));

        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    void Update()
    {
        foreach(GameObject tank in tankManager.getAllTanks())
        {
            if (Physics.Raycast(tank.transform.position + Vector3.up, -Vector3.up, out _hit))
            {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_hit.textureCoord.x, _hit.textureCoord.y, 0, 0));
                _drawMaterial.SetFloat("_Strength", _brushStrength);
                _drawMaterial.SetFloat("_Size", _brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatmap.width, _splatmap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatmap, temp);
                Graphics.Blit(temp, _splatmap, _drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }
}
