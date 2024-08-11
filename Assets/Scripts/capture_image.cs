using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

//用于捕获场景的截图并将其保存为PNG格式
public class capture_image : MonoBehaviour
{
    // Start is called before the first frame update

    public int minSpawnCount;//Sets the number of generated objects
    public int maxSpawnCount;
    public GameObject[] fruitPrefabs;
    private GameObject[] spawnedObjects;
    private static List<GameObject> spawnedObjectsGlobal = new List<GameObject>();
    public KeyCode spawnKey = KeyCode.Space; // Trigger the generated key
    public float minSpawnHeight = 1.0f; // Minimum value of random height
    public float maxSpawnHeight = 3.0f; // The maximum of the random height
    private int positioncount = 0;
    // Defines a counter to name an image each time it is saved
    private int captureCounter = 0;
    public int numberOfPhotos = 10; // The number of photos taken

    // Define up to 32 colors and assign one color to each instance
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

    private Material instanceMat; // Materials are used for instance rendering
    //Main camera
    public Camera main_camera;
    //depth camera
    public GameObject depth_camera;
    public Material mMat;
    private RenderTexture currentRT;
    
    void Start()
    {
        instanceMat = new Material(Shader.Find("Unlit/Color"));
        main_camera = Camera.main;

        depth_camera = GameObject.FindGameObjectWithTag("depth");
      
    }


    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            //Execute the photo coroutine
            StartCoroutine(TakePhotos());
        }
    }
    IEnumerator TakePhotos()
    {
        for (int photoIndex = 0; photoIndex < numberOfPhotos; photoIndex++)
        {
            StartCoroutine(spawn()); 
            yield return new WaitForSeconds(4f); // wait for 4s
            StartCoroutine(CaptureProcess());                                  // 
            yield return new WaitForSeconds(3f); //wait for 3s
            ClearFruits(); //clear fruits
            yield return new WaitForSeconds(1f); // wait for 1s
        }
    }
    //深度图像的方法

    private StringBuilder positionsString = new StringBuilder();
    public int SpawnNumber=5;
    private int SpawnHeight;

    //创建水果模型
    /* IEnumerator spawn()
     {
         int actualSpawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
         for (int i = 0; i < actualSpawnCount; i++)
         {
             // Random generating height
             float randomHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
             // Randomly generated position
             Vector3 randomPosition = new Vector3(
                 Random.Range(-0.8f, 0.8f), // X-axis range
                 randomHeight, // Random height
                 Random.Range(-0.8f, 0.8f) // Z-axis range
             );
             GameObject selectedFruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

             // Generate a new object at a random location
             GameObject spawnedObject = Instantiate(selectedFruitPrefab, randomPosition, Quaternion.Euler(-90, 0, 0));// Quaternion.Euler(-90, 0, 0)
             spawnedObjectsGlobal.Add(spawnedObject);
         }
         yield return new WaitForSeconds(3f); // 等待3秒

         System.Text.StringBuilder positionsString = new System.Text.StringBuilder();
         foreach (var obj in spawnedObjectsGlobal)
         {
             positionsString.Append(obj.name + "-");
             positionsString.Append("[" + obj.transform.position + "]");
             positionsString.Append(", ");
         }
         Debug.Log(positionsString.ToString());
         WritePositionsToFile(positionsString.ToString());
     }*/
    IEnumerator spawn()
    {
        if (fruitPrefabs.Length <= SpawnNumber)
        {
            // 首先保证每种水果至少被生成一次
            for (int i = 0; i < fruitPrefabs.Length; i++)
            {
                SpawnFruit(fruitPrefabs[i]);
            }

            for (int i = 0; i < SpawnNumber - fruitPrefabs.Length; i++)
            {
                GameObject selectedFruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
                SpawnFruit(selectedFruitPrefab);
            }
        }
        else
        {
            for (int i = 0; i < SpawnNumber; i++)
            {
                GameObject selectedFruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
                SpawnFruit(selectedFruitPrefab);
            }
        }

        // 如果还需要生成更多的水果，则随机生成剩余的

        yield return new WaitForSeconds(3f); // 等待3秒
    }

    void SpawnFruit(GameObject fruitPrefab)
    {
        SpawnHeight = Random.Range(1, 3);
        // Randomly generated position
        Vector3 randomPosition = new Vector3(
                 Random.Range(-0.8f, 0.8f), // X-axis range
                 SpawnHeight, // Random height
                 Random.Range(-0.8f, 0.8f) // Z-axis range
             );
        float randomXRotation = Random.Range(0, 360);
        float randomYRotation = Random.Range(0, 360);
        float randomZRotation = Random.Range(0, 360);
        // Generate a new object at a random location
        GameObject spawnedObject = Instantiate(fruitPrefab, randomPosition, Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation));
        spawnedObjectsGlobal.Add(spawnedObject);
    }
    void WritePositionsToFile(string positions)
    {
        // Specify folder path
        string folderPath = Application.dataPath + "/position";
        // If the folder does not exist, create the folder
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, "position_" + captureCounter + ".txt");

        // Writes location information to a text file
        File.WriteAllText(filePath, positions);

        // Print a message on the console indicating that the file has been created
        Debug.Log("Positions file created at: " + filePath);
        positioncount++;

    }

    void ClearFruits()
    {
        // Destroy all fruits on the field
        foreach (GameObject spawnedObject in spawnedObjectsGlobal)
        {
            Destroy(spawnedObject);
        }
        // Clear the list after destroying all objects to avoid holding references to destroyed objects
        spawnedObjectsGlobal.Clear();
    }



    //拍摄脚本
    IEnumerator CaptureProcess()
    {
        // Capture common screenshots
        ScreenCapture.CaptureScreenshot("C:/Users/bai/Desktop/柏/SAT301/Result/image/image_" + captureCounter + ".png");
        Debug.Log("Screenshot image_" + captureCounter + ".png" + " has been saved.");
        // Wait 0.1 seconds for the normal screenshot operation to complete
        yield return new WaitForSeconds(0.1f);
        // Capture semantic tag
        CaptureLanguageLabel();
        yield return new WaitForSeconds(1f);
        //Capture instance tag
        CaptureInstanceLabel();
        yield return new WaitForSeconds(1f);
        //Capture depth information
        capture_depth depth=depth_camera.GetComponent<capture_depth>();
        depth.CaptureDepth();
        captureCounter++;
    }
    //实例标签
    // 在这个方法中，为spawnedObjects中的每个物体分配颜色
    void CaptureInstanceLabel()
    {
        Material[] originalMats = new Material[spawnedObjectsGlobal.Count];
        for (int i = 0; i < spawnedObjectsGlobal.Count; i++)
        {
            // Gets the renderer component of the object
            originalMats[i] = spawnedObjectsGlobal[i].GetComponent<Renderer>().material;
            // Sets the color of the object
            Material newMatInstance = new Material(instanceMat);
            newMatInstance.color = instanceColors[i];
            spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
        }
        ScreenCapture.CaptureScreenshot("C:/Users/bai/Desktop/柏/SAT301/Result/instance/instance_label_" + captureCounter + ".png");
        Debug.Log("Instance label image_" + captureCounter + ".png" + " has been saved.");
        // Use coroutines to delay recovery of the original material
        StartCoroutine(RestoreMaterials(originalMats));
    }

    // 拍摄语义标签的方法
    void CaptureLanguageLabel()
    {
        Material[] originalMats = new Material[spawnedObjectsGlobal.Count];
        for (int i = 0; i < spawnedObjectsGlobal.Count; i++)
        {
            // Save the original material
            originalMats[i] = spawnedObjectsGlobal[i].GetComponent<Renderer>().material;
            Material newMatInstance = new Material(instanceMat);
            switch (spawnedObjectsGlobal[i].tag)
            {
                case "Apple":
                    newMatInstance.color = Color.red;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;
                case "Banana":
                    newMatInstance.color = Color.yellow;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;
                case "Orange":
                    newMatInstance.color = Color.cyan;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;
                case "Tomato":
                    newMatInstance.color = Color.blue;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;
                case "Melon":
                    newMatInstance.color = Color.green;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;
                case "blueberry":
                    newMatInstance.color = Color.white;
                    spawnedObjectsGlobal[i].GetComponent<Renderer>().material = newMatInstance;
                    break;


            }
        }
        ScreenCapture.CaptureScreenshot("C:/Users/bai/Desktop/柏/SAT301/Result/language/language_label_" + captureCounter + ".png");
        Debug.Log("Language label image_" + captureCounter + ".png" + " has been saved.");
        StartCoroutine(RestoreMaterials(originalMats));
    }


    //用于恢复材质的协程

    IEnumerator RestoreMaterials(Material[] originalMats)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < originalMats.Length; i++)
        {
            if (i < spawnedObjectsGlobal.Count && originalMats[i] != null)
            {
                spawnedObjectsGlobal[i].GetComponent<Renderer>().material = originalMats[i];
            }
        }
    }


}
