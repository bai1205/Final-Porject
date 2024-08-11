using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instance : MonoBehaviour
{
    // Start is called before the first frame update
    private int captureCounter = 0;

    // �������32����ɫ��Ϊÿ��ʵ������һ����ɫ
    private Color[] instanceColors = {
        Color.red, Color.green, Color.blue, Color.yellow, Color.magenta,
        Color.cyan, Color.white, Color.gray, Color.black, new Color(0.5f, 0.2f, 0.8f),
        new Color(0.2f, 0.5f, 0.2f), new Color(0.6f, 0.3f, 0.2f), new Color(0.9f, 0.6f, 0.1f),
        new Color(0.4f, 0.4f, 0.7f), new Color(0.8f, 0.5f, 0.3f), new Color(0.3f, 0.8f, 0.4f),
        new Color(0.7f, 0.2f, 0.6f), new Color(0.5f, 0.5f, 0.1f), new Color(0.1f, 0.7f, 0.9f),
        new Color(0.9f, 0.1f, 0.3f), new Color(0.3f, 0.3f, 0.3f), new Color(0.6f, 0.6f, 0.6f),
        new Color(0.1f, 0.1f, 0.5f), new Color(0.8f, 0.8f, 0.8f), new Color(0.2f, 0.9f, 0.2f),
        new Color(0.7f, 0.7f, 0.7f), new Color(0.4f, 0.1f, 0.4f), new Color(0.5f, 0.9f, 0.5f),
        new Color(0.6f, 0.4f, 0.8f), new Color(0.9f, 0.5f, 0.6f), new Color(0.3f, 0.6f, 0.3f),
        new Color(0.7f, 0.9f, 0.7f)
    };

    private Material instanceMat; // ��������ʵ����Ⱦ

    // Start�������ڽű���������ʱ����һ��
    void Start()
    {
        // ����һ���򵥵Ĳ�������ʵ����Ⱦ
        instanceMat = new Material(Shader.Find("Unlit/Color"));
    }

    // Update������ÿһ֡���ᱻ����
    void Update()
    {
        // ����Ұ��¿ո��ʱ
        if (Input.GetKeyDown(KeyCode.W))
        {
            //ִ������Э��
            StartCoroutine(CaptureProcess());
        }
    }

    //����Э��
    IEnumerator CaptureProcess()
    {
        // ������ͨ��ͼ
        ScreenCapture.CaptureScreenshot("Assets/images/image_" + captureCounter + ".png");
        Debug.Log("Screenshot image_" + captureCounter + ".png" + " has been saved.");

        // �ȴ�0.1����ȷ����ͨ��ͼ�������
        yield return new WaitForSeconds(0.1f);

        // ����ʵ����ǩ
        CaptureInstanceLabel();

        captureCounter++;
    }

    // ����ʵ����ǩ�ķ���
    void CaptureInstanceLabel()
    {
        GameObject[] Instances = GameObject.FindGameObjectsWithTag("Apple");
        // ���浱ǰÿ��ƻ��ʵ���Ĳ���
        Material[] originalMats = new Material[Instances.Length];

        for (int i = 0; i < Instances.Length; i++)
        {
            // ����ԭʼ����
            originalMats[i] = Instances[i].GetComponent<Renderer>().material;

            // ����һ���µĲ���ʵ����������ʵ����ɫ
            Material newMatInstance = new Material(instanceMat);
            newMatInstance.color = instanceColors[i];
            Instances[i].GetComponent<Renderer>().material = newMatInstance;
        }

        // ����ʵ����ͼ
        ScreenCapture.CaptureScreenshot("Assets/images/instance_label_" + captureCounter + ".png");
        Debug.Log("Instance label image_" + captureCounter + ".png" + " has been saved.");

        // ʹ��Э�����ӳٻָ�ԭʼ����
        StartCoroutine(RestoreMaterials(originalMats, Instances));
    }

    //���ڻָ����ʵ�Э��
    IEnumerator RestoreMaterials(Material[] originalMats, GameObject[] Instance)
    {
        // �ȴ�0.1����ȷ����ͼ�������
        yield return new WaitForSeconds(0.1f);

        // �ָ�ԭʼ����
        for (int i = 0; i < originalMats.Length; i++)
        {
            if (i < Instance.Length && originalMats[i] != null)
            {
                Instance[i].GetComponent<Renderer>().material = originalMats[i];
            }
        }
    }
}
