using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject Sponza;
    private Material[] SponzaMaterials;
    private Material portalPlaneMaterial;

    // Start is called before the first frame update
    void Start()
    {
        SponzaMaterials = Sponza.GetComponent<Renderer>().sharedMaterials;
        portalPlaneMaterial = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        Vector3 campPsitionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        if (campPsitionInPortalSpace.y <= 0.0f)
        {
            //Disable Stencil Test
            for (int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }
            portalPlaneMaterial.SetInt("_CullMode",(int)CullMode.Front);
        }
        else if(campPsitionInPortalSpace.y < 0.5f)
        {
            //Disable Stencil Test
            for(int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
            portalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Off);
        }
        else
        {
            //Enable Stencil Test
            for (int i = 0; i < SponzaMaterials.Length; ++i)
            {
                SponzaMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
            portalPlaneMaterial.SetInt("_CullMode", (int)CullMode.Back);
        }
    }
}
