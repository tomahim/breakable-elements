using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubifier : MonoBehaviour
{
    public GameObject TargetCube;

    public Vector3 SectionCount;
    public Material SubCubeMaterial;

    private Vector3 SizeOfOriginalCube;
    private Vector3 SectionSize;
    private Vector3 FillStartPosition;
    private Transform ParentTransform;
    private GameObject SubCube;
    private List<GameObject> SubCubes = new List<GameObject>();
    private List<Rigidbody> rbs = new List<Rigidbody>();

    void Start()
    {
        if (TargetCube == null)
            TargetCube = gameObject;

        SizeOfOriginalCube = TargetCube.transform.lossyScale;
        SectionSize = new Vector3(
            SizeOfOriginalCube.x / SectionCount.x,
            SizeOfOriginalCube.y / SectionCount.y,
            SizeOfOriginalCube.z / SectionCount.z
            );

        FillStartPosition = TargetCube.transform.TransformPoint(new Vector3(-0.5f, 0.5f, -0.5f))
                            + TargetCube.transform.TransformDirection(new Vector3(SectionSize.x, -SectionSize.y, SectionSize.z) / 2.0f);

        ParentTransform = new GameObject(TargetCube.name + "CubeParent").transform;

        InstantDivideIntoCuboids();
        StartCoroutine(SetPhysics());
    }

    void InstantDivideIntoCuboids()
    {
        for (int i = 0; i < SectionCount.x; i++)
        {
            for (int j = 0; j < SectionCount.y; j++)
            {
                for (int k = 0; k < SectionCount.z; k++)
                {
                    SubCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    // SubCube.GetComponent<MeshRenderer>().material = SubCubeMaterial;
                    SubCube.transform.localScale = SectionSize;
                    SubCube.transform.position = FillStartPosition +
                                                   TargetCube.transform.TransformDirection(new Vector3((SectionSize.x) * i, -(SectionSize.y) * j, (SectionSize.z) * k));
                    SubCube.transform.rotation = TargetCube.transform.rotation;

                    SubCube.transform.SetParent(ParentTransform);
                    SubCubes.Add(SubCube);
                }
            }
        }
        foreach (GameObject cube in SubCubes)
        {
            Rigidbody rb = cube.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rbs.Add(rb);
            rb.mass = 0.5f;
        }
    }

    IEnumerator SetPhysics()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
            //rb.mass = 0.5f;
        }
        Destroy(TargetCube);
    }
}