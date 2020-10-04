using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
using input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    private List<DetectedPlane> m_NewDetectedPlanes = new List<DetectedPlane>();
    public GameObject GridPrefab;
    public GameObject Portal;
    public GameObject ARCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check ARCore session status
        if(Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        //The following function will fill m_newDetectedPlanes with the planes that ARCore detected in the current frame
        Session.GetTrackables<DetectedPlane>(m_NewDetectedPlanes, TrackableQueryFilter.New);
        
        //Instantiate a grid for each detected plane in m_NewDetectedPlane
        for(int i = 0; i < m_NewDetectedPlanes.Count; ++i)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);

            //This function will set the position of the grid and modify the vertices of the attached mesh
            grid.GetComponent<GridVisualizer>().Initialize(m_NewDetectedPlanes[i]);
        }

        //Check if the user touches the screen 
        Touch touch;
        if(Input.touchCount<1 || (touch=Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        //Lets now check if the user touched any of the tracked planes
        TrackableHit hit;
        if(Frame.Raycast(touch.position.x,touch.position.y,TrackableHitFlags.PlaneWithinPolygon,out hit))
        {
            //Lets now place the portal on top of the tracked plane that we touched

            //Enable the portal
            Portal.SetActive(true);

            //Create a new anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //Set the position of the portal to be the same as the hit position
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //We want the portal to face the camera
            Vector3 cameraPosition = ARCamera.transform.position;

            //The portal should only rotate around y-axis
            cameraPosition.y = hit.Pose.position.y;

            //Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            //ARCore will keep understanding the world and update the anchors accordingly hence we need to attach our portal to the anchor
            Portal.transform.parent = anchor.transform;
        }
    }
}
