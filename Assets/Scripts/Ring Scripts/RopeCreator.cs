using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    public GameObject ropeSegmentPrefab; // Assign your rope segment prefab in the inspector
    public Transform targetMarker; // Assign the target marker in the inspector
    public float segmentLength = 0.4f; // The length of one segment

    private GameObject previousSegment; // To keep track of the last created segment

    void Start()
    {
        GenerateRope();
    }

    void GenerateRope()
    {
        Vector3 directionToTarget = (targetMarker.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetMarker.position);
        int numberOfSegments = Mathf.FloorToInt(distanceToTarget / segmentLength);
        float actualSpacing = distanceToTarget / numberOfSegments; // Adjust spacing to fit exactly

        for (int i = 0; i < numberOfSegments; i++)
        {
            // Calculate the position for this segment
            Vector3 position = transform.position + directionToTarget * (i * actualSpacing + segmentLength / 2);

            // Instantiate the segment at the calculated position
            GameObject segment = Instantiate(ropeSegmentPrefab, position, Quaternion.identity, transform);

            // Add a SpringJoint if this is not the first segment
            if (previousSegment != null)
            {
                SpringJoint joint = segment.AddComponent<SpringJoint>();
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
                // Set other SpringJoint properties as needed
            }

            // Update the previousSegment reference
            previousSegment = segment;
        }

        // Optionally, make the first and last segments kinematic to anchor the rope
        transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
        previousSegment.GetComponent<Rigidbody>().isKinematic = true;
    }
}
