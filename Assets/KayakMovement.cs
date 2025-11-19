using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayakMovement : MonoBehaviour
{

    [Header("Paddle References")]
    public Transform leftPaddle;
    public Transform rightPaddle;
    public PaddleWaterDetector leftDetector;
    public PaddleWaterDetector rightDetector;

    [Header("Kayak")]
    public Rigidbody kayakRigidbody;

    [Header("Settings")]
    public float baseStrokeForce = 5f;     // baseline thrust
    public float depthMultiplier = 4f;     // deeper = more powerful
    public float velocityMultiplier = 1.2f; // faster blade = stronger stroke
    public float turnForce = 3f;            // rotational force
    public float sideDistanceMultiplier = 1.5f; // turning stronger if paddle further from center
    public float minSpeed = 0.05f;          // ignore tiny motions
    public float damping = 0.97f;


    // Previous frame paddle positions
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;



    // Start is called before the first frame update
    void Start()
    {
        if (leftPaddle) lastLeftPos = leftPaddle.position;
        if (rightPaddle) lastRightPos = rightPaddle.position;
    }

    void FixedUpdate()
    {
        if (!kayakRigidbody) return;

        bool leftExists = leftPaddle != null;
        bool rightExists = rightPaddle != null;

        if (leftPaddle)
            HandlePaddle(leftPaddle, ref lastLeftPos, leftDetector, isLeft: true);

        if (rightPaddle)
            HandlePaddle(rightPaddle, ref lastRightPos, rightDetector, isLeft: false);

        // Water drag
        kayakRigidbody.velocity *= damping;
        kayakRigidbody.angularVelocity *= damping;
    }

    // works based on paddle depth in water
    private void HandlePaddle(Transform paddle, ref Vector3 lastPos, PaddleWaterDetector detector, bool isLeft)
    {
        // if paddle isn't in water (or doesn't exist), do nothing
        if (!detector || !detector.IsInWater()) return;

        // Paddle velocity
        Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
        lastPos = paddle.position;

        float paddleSpeed = paddleVel.magnitude;
        // if paddle speed is too slow, do nothing
        if (paddleSpeed < minSpeed) return;

        // Backward stroke
        float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);
        if (strokePower <= 0f) return;

        // ---------- DEPTH-BASED STROKE FORCE ----------
        float depthFactor = detector.depth * depthMultiplier; // deeper = more powerful
        float velocityFactor = paddleSpeed * velocityMultiplier;

        float totalPower = baseStrokeForce * depthFactor * velocityFactor;

        Debug.Log($"{(isLeft ? "LEFT" : "RIGHT")} Depth: {detector.depth:F3}");
        Debug.DrawRay(
            paddle.position,
            Vector3.up * detector.depth,
            isLeft ? Color.cyan : Color.magenta,
            0.25f
        );

        // Forward thrust
        Vector3 force = kayakRigidbody.transform.forward * (strokePower * totalPower);
        kayakRigidbody.AddForce(force, ForceMode.Acceleration);

        Debug.Log($"{(isLeft ? "LEFT" : "RIGHT")} Force: {force.magnitude:F2}");

        // ---------- ADVANCED TURNING ----------
        // How far paddle is from kayak center?
        Vector3 localPos = kayakRigidbody.transform.InverseTransformPoint(paddle.position);
        float sideDistance = Mathf.Abs(localPos.x); // farther = more turning

        float sideFactor = sideDistance * sideDistanceMultiplier;

        // Decide turn direction
        float torqueDir = isLeft ? +1f : -1f;

        float torqueAmount = strokePower * totalPower * sideFactor * 0.5f; // scaled down for stability

        kayakRigidbody.AddTorque(Vector3.up * torqueAmount * torqueDir, ForceMode.Acceleration);

        // Debug
        Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.cyan : Color.magenta, 0.05f);
        Debug.DrawRay(kayakRigidbody.position, force.normalized * 3f, Color.red, 0.1f);
    }

    // works only when paddle is in water
/*    private void HandleSinglePaddle(Transform paddle, ref Vector3 lastPos, bool isLeft, PaddleWaterDetector waterDetector)
    {
        if (!waterDetector.IsInWater())
        {
            return;
        }

        // Paddle velocity
        Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
        lastPos = paddle.position;

        float speed = paddleVel.magnitude;
        if (speed < minSpeed) return;

        // Backward stroke
        float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);
        if (strokePower <= 0f) return;

        // Forward propulsion
        Vector3 forwardForce = kayakRigidbody.transform.forward * (strokePower * strokeForce);
        kayakRigidbody.AddForce(forwardForce, ForceMode.Acceleration);

        // Turning torque
        float torqueDirection = isLeft ? +1f : -1f;
        float torqueAmount = strokePower * turnForce;
        kayakRigidbody.AddTorque(Vector3.up * torqueAmount * torqueDirection, ForceMode.Acceleration);

        // Debug visuals
        Debug.DrawRay(kayakRigidbody.position, forwardForce.normalized * 3f, Color.red, 0.1f);
        Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.cyan : Color.magenta, 0.1f);
    }*/

      // works when paddle is out of air and kayak moves
    /*    private void HandleSinglePaddle(Transform paddle, ref Vector3 lastPos, bool isLeft)
        {
            // Paddle velocity
            Vector3 paddleVel = (paddle.position - lastPos) / Time.fixedDeltaTime;
            lastPos = paddle.position;

            float speed = paddleVel.magnitude;
            if (speed < minSpeed) return; // too small, ignore

            // Stroke power based on backward movement (toward kayak rear)
            float strokePower = Vector3.Dot(paddleVel, -kayakRigidbody.transform.forward);
            if (strokePower <= 0f) return; // only couunt backward strokes

            // --- Forward Push ---
            Vector3 forwardForce = kayakRigidbody.transform.forward * (strokePower * strokeForce);
            kayakRigidbody.AddForce(forwardForce, ForceMode.Acceleration);

            // --- Arc Turning (Left paddle pushes right, right paddle pushes left) ---
            float torqueDirection = isLeft ? +1f : -1f;
            float torqueAmount = strokePower * turnForce;

            kayakRigidbody.AddTorque(Vector3.up * torqueAmount * torqueDirection, ForceMode.Acceleration);

            // Debug lines
            Debug.DrawRay(kayakRigidbody.position, forwardForce.normalized * 3f, Color.red, 0.1f);
            Debug.DrawRay(paddle.position, paddleVel, isLeft ? Color.cyan : Color.magenta, 0.1f);
        }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
