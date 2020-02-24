using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class recorder : MonoBehaviour
{

    public VehicleAgentCont vehicle;
    public string runId;
    public float imgFrequency = 0.5f;
    public bool record = false;

    private int FileCounter = 0;
    private float time = 0.0f;

    private StreamWriter streamWriter;

    // Start is called before the first frame update
    void Start()
    {
        SetUpTargetFile();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
    }

    private void LateUpdate()
    {
        time += Time.deltaTime;
        if (record && time >= imgFrequency)
        {
            time = 0.0f;
            CamCapture();
            WriteTargetData();
        }
    }

    void SetUpTargetFile()
    {
        string path = Application.dataPath + "/recording_data/" + runId + "_" + "targets" + ".csv";
        File.AppendAllText(path, "runId,iteration,angle\n"); 
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

        string path = Application.dataPath + "/recording_data/" + runId + "_" + FileCounter + ".png";
        File.WriteAllBytes(path, Bytes);
        FileCounter++;
    }

    void WriteTargetData()
    {
        var target = vehicle.currentAngle;

        string path = Application.dataPath + "/recording_data/" + runId + "_" + "targets" + ".csv";
        string text = runId + "," + FileCounter.ToString() + "," + target.ToString() + "\n";
        File.AppendAllText(path, text);
    }
}
