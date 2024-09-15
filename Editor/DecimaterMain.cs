using UnityEngine;
using UnityEditor;

public class DecimaterMain : EditorWindow
{
    private GameObject selectedGameObject;
    private Mesh originalMesh;
    private Mesh decimatedMesh;
    private MeshPreviewer meshPreviewer;
    private MeshInfoDisplay meshInfoDisplay;
    private float decimateLevel = 1.0f;

    private Material[] originalMaterials;
    private int[] originalSubmeshCount;
    private Material previewMaterial;
    private Shader previewShader;

    [MenuItem("Decimater/MeshDecimater")]
    public static void ShowWindow()
    {
        GetWindow<DecimaterMain>("Decimater for Unity");
    }

    private void OnEnable()
    {
        LoadShaders();
        previewMaterial = CreatePreviewMaterial();

        meshPreviewer = new MeshPreviewer(previewMaterial);
        meshInfoDisplay = new MeshInfoDisplay();
    }

    private void LoadShaders()
    {
        previewShader = Shader.Find("Refiaa/Wireframe");
    }

    private void OnSelectionChange()
    {
        UpdateSelection(Selection.activeGameObject);
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a GameObject", EditorStyles.boldLabel);

        GameObject newSelectedGameObject = (GameObject)EditorGUILayout.ObjectField("GameObject", selectedGameObject, typeof(GameObject), true);
        if (newSelectedGameObject != selectedGameObject)
        {
            UpdateSelection(newSelectedGameObject);
        }

        if (selectedGameObject == null)
        {
            EditorGUILayout.HelpBox("No GameObject selected. Please select a GameObject with a MeshFilter or SkinnedMeshRenderer component.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);
        GUILayout.Label("Mesh Preview", EditorStyles.boldLabel);
        Rect previewRect = GUILayoutUtility.GetRect(400, 400);
        meshPreviewer.PreviewMesh(selectedGameObject, previewRect);

        GUILayout.Space(10);
        GUILayout.Label("Decimate Level", EditorStyles.boldLabel);
        decimateLevel = EditorGUILayout.Slider("Decimate Level", decimateLevel, 0.1f, 1.0f);

        GUILayout.Space(10);
        if (GUILayout.Button("Apply Decimation"))
        {
            ApplyDecimation();
        }
        
        if (GUILayout.Button("Revert"))
        {
            RevertDecimation();
        }

        GUILayout.Space(10);
        meshInfoDisplay.DisplayMeshInfo(GetCurrentMesh());
    }

    private void UpdateSelection(GameObject newSelectedGameObject)
    {
        if (newSelectedGameObject != null)
        {
            MeshFilter meshFilter = newSelectedGameObject.GetComponent<MeshFilter>();
            SkinnedMeshRenderer skinnedMeshRenderer = newSelectedGameObject.GetComponent<SkinnedMeshRenderer>();

            if (meshFilter != null)
            {
                selectedGameObject = newSelectedGameObject;
                originalMesh = meshFilter.sharedMesh;
                EnableReadWrite(originalMesh);
                decimatedMesh = Instantiate(originalMesh);
                meshInfoDisplay.SetOriginalMesh(originalMesh);
                
                Renderer renderer = newSelectedGameObject.GetComponent<Renderer>();
                originalMaterials = renderer.sharedMaterials;
                originalSubmeshCount = new int[originalMesh.subMeshCount];
                for (int i = 0; i < originalMesh.subMeshCount; i++)
                {
                    originalSubmeshCount[i] = originalMesh.GetTriangles(i).Length / 3;
                }

                Selection.activeObject = originalMesh;
                Debug.Log($"Selected Mesh: {AssetDatabase.GetAssetPath(originalMesh)}");
                Repaint();
            }
            else if (skinnedMeshRenderer != null)
            {
                selectedGameObject = newSelectedGameObject;
                originalMesh = skinnedMeshRenderer.sharedMesh;
                EnableReadWrite(originalMesh);
                decimatedMesh = Instantiate(originalMesh);
                meshInfoDisplay.SetOriginalMesh(originalMesh);
                
                originalMaterials = skinnedMeshRenderer.sharedMaterials;
                originalSubmeshCount = new int[originalMesh.subMeshCount];
                for (int i = 0; i < originalMesh.subMeshCount; i++)
                {
                    originalSubmeshCount[i] = originalMesh.GetTriangles(i).Length / 3;
                }

                Selection.activeObject = originalMesh;
                Debug.Log($"Selected Mesh: {AssetDatabase.GetAssetPath(originalMesh)}");
                Repaint();
            }
        }
    }

    private void ApplyDecimation()
    {
        bool isSkinnedMeshRenderer = selectedGameObject.GetComponent<SkinnedMeshRenderer>() != null;

        EnableReadWrite(decimatedMesh);
        MeshDecimaterUtility.DecimateMesh(originalMesh, decimatedMesh, decimateLevel, isSkinnedMeshRenderer, originalSubmeshCount);

        if (selectedGameObject.GetComponent<MeshFilter>() != null)
        {
            MeshFilter meshFilter = selectedGameObject.GetComponent<MeshFilter>();
            meshFilter.sharedMesh = decimatedMesh;
            MeshRenderer meshRenderer = selectedGameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterials = originalMaterials;
        }
        else if (isSkinnedMeshRenderer)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = selectedGameObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = decimatedMesh;
            skinnedMeshRenderer.sharedMaterials = originalMaterials;
        }

        meshPreviewer.UpdatePreviewMesh(selectedGameObject);
    }

    private void RevertDecimation()
    {
        if (selectedGameObject.GetComponent<MeshFilter>() != null)
        {
            MeshFilter meshFilter = selectedGameObject.GetComponent<MeshFilter>();
            meshFilter.sharedMesh = originalMesh;
            MeshRenderer meshRenderer = selectedGameObject.GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterials = originalMaterials;
        }
        else if (selectedGameObject.GetComponent<SkinnedMeshRenderer>() != null)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = selectedGameObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sharedMesh = originalMesh;
            skinnedMeshRenderer.sharedMaterials = originalMaterials;
        }

        decimateLevel = 1.0f;
        meshPreviewer.UpdatePreviewMesh(selectedGameObject);
    }

    private Mesh GetCurrentMesh()
    {
        if (selectedGameObject != null)
        {
            if (selectedGameObject.GetComponent<MeshFilter>() != null)
            {
                return selectedGameObject.GetComponent<MeshFilter>().sharedMesh;
            }
            else if (selectedGameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                return selectedGameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
        }
        return null;
    }

    private void EnableReadWrite(Mesh mesh)
    {
        string path = AssetDatabase.GetAssetPath(mesh);
        if (string.IsNullOrEmpty(path)) return;

        ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
        if (modelImporter != null)
        {
            modelImporter.isReadable = true;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
    }

    private Material CreatePreviewMaterial()
    {
        if (previewShader == null)
        {
            Debug.LogError("Preview shader not found!");
            return null;
        }

        Material material = new Material(previewShader)
        {
            name = "Preview Material"
        };

        material.SetFloat("_Glossiness", 0.0f);
        material.SetFloat("_Metallic", 0.0f);
        material.SetColor("_Color", Color.white);
        material.SetColor("_EmissionColor", Color.white);

        material.EnableKeyword("_EMISSION");

        material.SetColor("_WireColor", Color.black);

        return material;
    }
}
