using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube LastCube { get; private set; }

    [SerializeField]
    public float moveSpeed = 1f;

    private void OnEnable()
    {
        if(LastCube == null)
        {
            LastCube = GameObject.Find("Start").GetComponent<MovingCube>();
        }

        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop()
    {
        moveSpeed = 0;
        float hangover = transform.position.z - LastCube.transform.position.z;
        Debug.Log(hangover);

        float direction = hangover > 0 ? 1f : -1f;
        SplitCubeOnZ(hangover, direction);
    }

    private void SplitCubeOnZ(float hangover, float direction)
    {
        float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = LastCube.transform.position.z + (hangover / 2f);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);
    }

    private void SpawnDropCube(float fallingBlockZPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockZPosition);

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        Destroy(cube, 1.5f);
    }

    private void FixedUpdate()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
}
