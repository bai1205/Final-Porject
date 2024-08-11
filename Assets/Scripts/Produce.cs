using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Produce : MonoBehaviour
{
    // Start is called before the first frame update
    public int spawncount;
    void Start()
    {

    }

    // Update is called once per frame
    public GameObject objectToSpawn; // Ҫ���ɵ�����
    public KeyCode spawnKey = KeyCode.Space; // �������ɵİ���
    public float minSpawnHeight = 1.0f; // ����߶ȵ���Сֵ
    public float maxSpawnHeight = 3.0f; // ����߶ȵ����ֵ

    void Update()
    {
        // ����Ƿ���ָ������
        if (Input.GetKeyDown(spawnKey))
        {
            for (int i = 0; i < spawncount; i++)
            {
                // ������ɸ߶�
                float randomHeight = Random.Range(minSpawnHeight, maxSpawnHeight);

                // �������λ��
                Vector3 randomPosition = new Vector3(
                    Random.Range(-1.0f, 1.0f), // X�᷶Χ
                    randomHeight, // ����߶�
                    Random.Range(-1.0f, 1.0f) // Z�᷶Χ
                );
                Quaternion randomRotation = Quaternion.Euler(
                    Random.Range(0, 360), // X����ת
                    Random.Range(0, 360), // Y����ת
                    Random.Range(0, 360)  // Z����ת
                );

                // �����λ������������
                GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, randomRotation);
            }
        }
    }
}
