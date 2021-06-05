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
                    Vector3 position = new Vector3(
                        (transform.position.x - (shapeSize.x / 2)) + newCubeSize.x / 2 + x * newCubeSize.x,
                        (transform.position.y - (shapeSize.y / 2)) + newCubeSize.y / 2 + y * newCubeSize.y,
                        (transform.position.z - (shapeSize.z / 2)) + newCubeSize.z / 2 + z * newCubeSize.z
                    );
                    cube.transform.position = position;
                    cube.transform.localScale = newCubeSize;
                    cube.transform.SetParent(transform);
                }
            }
        }

        renderer.enabled = false;
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
