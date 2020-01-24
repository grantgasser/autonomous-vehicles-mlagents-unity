using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RollerCameraController : MonoBehaviour
{

    public RollerAgent roller;
    public float imgFrequency = 0.5f;
    public bool record = false;

    private Vector3 offset;

    private int FileCounter = 0;
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - roller.transform.position;
    }

    private void Update()
    {
        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = transform.position = roller.transform.position + offset;

        transform.rotation = Quaternion.LookRotation(roller.rBody.velocity);
        time += Time.deltaTime;
        if ( record && time >= imgFrequency )
        {
            time = 0.0f;
            CamCapture();
        }
    }

    void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        string path = Application.dataPath + "/Camera/" + roller.GetInstanceID() + "_" + FileCounter + ".png";
        File.WriteAllBytes(path, Bytes);
        FileCounter++;
    }
}
