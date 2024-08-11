using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

//����һ��Unity�ű������ڲ��񳡾��е����ͼ�񲢱���ΪEXR��ʽ

public class capture_depth : MonoBehaviour
{
    // ����һ��������������ÿ�α���ͼ��ʱ��ͼ������
    private int captureCounter = 0;
    // �����Ĳ��ʱ�����������OnRenderImage�д���ͼ��
    public Material mMat;
    // ������������
    private Camera mCamera;
    // ���浱ǰ�����Ⱦ���������
    private RenderTexture currentRT;

    //����
    public int numberOfPhotos;
    void Start()
    {
        // ��ȡ��Ϸ�����ϵ�������
    }

    void Update()
    {

    }
    
    public void CaptureDepth()
    {
        mCamera = gameObject.GetComponent<Camera>();
        // �������ģʽΪ���ģʽ
        mCamera.depthTextureMode = DepthTextureMode.Depth;
        // Ϊ�������һ���µ�Ŀ����Ⱦ������С����������ؿ��һ�£�24λ��ȣ�ʹ��ARGBFloat��ʽ
        mCamera.targetTexture = new RenderTexture(mCamera.pixelWidth, mCamera.pixelHeight, 24, RenderTextureFormat.ARGBFloat);
        // ���浱ǰ�����Ⱦ��������
        currentRT = RenderTexture.active;

        // �������Ŀ����������Ϊ��ǰ�����Ⱦ����
        RenderTexture.active = mCamera.targetTexture;

        // ��Ⱦ�������ͼ
        mCamera.Render();

        // ����һ���µ�2D������С�������Ŀ������һ��
        Texture2D image = new Texture2D(mCamera.targetTexture.width, mCamera.targetTexture.height, TextureFormat.RGBAFloat, false);
        // �������Ŀ�������ж�ȡ���ص�2D������
        image.ReadPixels(new Rect(0, 0, mCamera.targetTexture.width, mCamera.targetTexture.height), 0, 0);
        image.Apply();

        // �ָ�֮ǰ�Ļ��Ⱦ����
        RenderTexture.active = currentRT;

        // ��2D�������ΪEXR��ʽ
        byte[] bytes = image.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
        // ����EXR�ļ���ָ��·��
        File.WriteAllBytes(Application.dataPath + "/images/depth/depth_" + captureCounter + ".exr", bytes);
        // �ڿ���̨��ӡ��Ϣ����ʾͼ���ѱ�����
        Debug.Log("Screenshot depth_" + captureCounter + " has been saved.");
        
        // ����ͼ�������
        captureCounter++;
    }

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // ���������mMat���ʣ�ʹ�øò��ʴ���ͼ��
            if (null != mMat)
            {
                Graphics.Blit(source, destination, mMat);
            }
            // ����ֱ�ӽ�Դ�����Ƶ�Ŀ������
            else
            {
                Graphics.Blit(source, destination);
            }
        }
}
