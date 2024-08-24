using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    [SerializeField] public float meshRefreshRate = 0.1f;
    [SerializeField] private Material material;
    [SerializeField] private float meshDestroyDelay = 2f;
    [SerializeField] private string shaderVariableRef;
    [SerializeField] private float shaderVariableRate = 0.1f;
    [SerializeField] private float shaderVariableRefreshRate = 0.05f;


    public void DisplayMeshTrail(float trailTime)
    {
        StartCoroutine(Trail(trailTime));
    }

    IEnumerator Trail(float trailTime)
    {
        List<SkinnedMeshRenderer> skinnedMeshRenderers = new List<SkinnedMeshRenderer>();
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        GameObject empty = new GameObject("PlayerTrail");


        while (trailTime > 0f)
        {
            trailTime -= meshRefreshRate;

            // Get all SkinnedMeshRenderers if not already retrieved
            if (skinnedMeshRenderers.Count == 0)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            }

            // Get all MeshRenderers if not already retrieved
            if (meshRenderers.Count == 0)
            {
                meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
            }

            // Create trail for SkinnedMeshRenderers
            for (int i = 0; i < skinnedMeshRenderers.Count; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(transform.position, transform.rotation);
                gObj.transform.parent = empty.transform;

                MeshRenderer meshRenderer = gObj.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                meshFilter.mesh = mesh;
                meshRenderer.material = material;
                StartCoroutine(AnimateMaterialFade(meshRenderer.material, 0, shaderVariableRate, shaderVariableRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            // Create trail for MeshRenderers
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(meshRenderers[i].transform.position, meshRenderers[i].transform.rotation);
                gObj.transform.parent = empty.transform;

                MeshRenderer meshRenderer = gObj.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = gObj.AddComponent<MeshFilter>();

                meshFilter.mesh = meshRenderers[i].GetComponent<MeshFilter>().mesh;
                meshRenderer.material = material;
                StartCoroutine(AnimateMaterialFade(meshRenderer.material, 0, shaderVariableRate, shaderVariableRefreshRate));
                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

    }

    IEnumerator AnimateMaterialFade(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVariableRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVariableRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
