using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeDivider : MonoBehaviour
{
    Transform transform;
    Renderer renderer;
    Vector3 shapeSize;

    public bool cubeApproximation = true;
    public float nbDivision = 2;

    public float xDivision = 2;
    public float yDivision = 2;
    public float zDivision = 2;

    void Start()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<Renderer>();
        shapeSize = renderer.bounds.size;

        Vector3 newCubeSize = cubeApproximation ? computeCubeSize() : computeRectangleSize();
        Vector3 startPosition = transform.TransformPoint(new Vector3(-0.5f, 0.5f, -0.5f))
                            + transform.TransformDirection(new Vector3(newCubeSize.x, -newCubeSize.y, newCubeSize.z) / 2.0f);
        int numCube = 1;
        for (float x = 0; x < xDivision; x++)
        {
            for (float y = 0; y < yDivision; y++)
            {
                for (float z = 0; z < zDivision; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "cube " + numCube;
                    numCube++;
                    cube.transform.localScale = newCubeSize;
                    cube.transform.position = (
                        startPosition + transform.TransformDirection(
                            new Vector3((newCubeSize.x) * x, -(newCubeSize.y) * y, (newCubeSize.z) * z)
                        )
                    );
                    cube.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
                    // cube.transform.SetParent(transform);
                    Rigidbody rb = cube.AddComponent<Rigidbody>();
                    rb.mass = 0.2f;
                }
            }
        }
        gameObject.SetActive(false);
        // renderer.enabled = false;
    }

    private Vector3 computeCubeSize()
    {
        float smallestSide = Mathf.Min(new float[] { shapeSize.x, shapeSize.y, shapeSize.z });
        float cubeSize = smallestSide / nbDivision;
        Vector3 newCubeSize = new Vector3(cubeSize, cubeSize, cubeSize);
        xDivision = shapeSize.x / newCubeSize.x;
        yDivision = shapeSize.y / newCubeSize.y;
        zDivision = shapeSize.z / newCubeSize.z;
        return newCubeSize;
    }

    private Vector3 computeRectangleSize()
    {
        return new Vector3(shapeSize.x / xDivision, shapeSize.y / yDivision, shapeSize.z / zDivision);
    }
}
