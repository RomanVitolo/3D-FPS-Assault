// Assets/Editor/AssemblyDefinitionGenerator.cs
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class AssemblyDefinitionGenerator : EditorWindow
{
    // Carpeta raíz seleccionada por el usuario
    private DefaultAsset targetFolder;

    // Plataformas disponibles y su estado (toggle)
    private Dictionary<string, bool> platformToggles = new Dictionary<string, bool>
    {
        { "Editor", true },
        { "Standalone", true },
        { "iOS", false },
        { "Android", false }
    };

    [MenuItem("Window/Assembly Definition Generator")]
    public static void ShowWindow()
        => GetWindow<AssemblyDefinitionGenerator>("AsmDef Generator");

    void OnGUI()
    {
        GUILayout.Label("1) Drag & Drop a Folder here", EditorStyles.boldLabel);
        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Root Folder",
            targetFolder,
            typeof(DefaultAsset),
            allowSceneObjects: false);

        GUILayout.Space(10);
        GUILayout.Label("2) Select Platforms to Include", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        foreach (var key in platformToggles.Keys.ToList())
        {
            platformToggles[key] = EditorGUILayout.Toggle(key, platformToggles[key]);
        }
        EditorGUI.indentLevel--;

        GUILayout.Space(10);
        if (GUILayout.Button("Generate Assembly Definitions"))
            GenerateAsmDefsForSelected();

        GUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "If no folder is selected, it will scan entire Assets/ by default.",
            MessageType.Info);
    }

    void GenerateAsmDefsForSelected()
    {
        string rootPath = "Assets";
        if (targetFolder != null)
        {
            rootPath = AssetDatabase.GetAssetPath(targetFolder);
            if (!AssetDatabase.IsValidFolder(rootPath))
            {
                Debug.LogError($"[AsmDefGen] Selected asset is not a folder: {rootPath}");
                return;
            }
        }

        // Generar lista de plataformas activas
        var includePlatforms = platformToggles
            .Where(kv => kv.Value)
            .Select(kv => kv.Key)
            .ToArray();

        // 1. Encontrar todos los scripts bajo rootPath
        string[] guids = AssetDatabase.FindAssets("t:Script", new[] { rootPath });
        var folderMap = new Dictionary<string, List<string>>();
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            if (!path.EndsWith(".cs")) continue;
            var folder = Path.GetDirectoryName(path);
            if (!folderMap.ContainsKey(folder))
                folderMap[folder] = new List<string>();
            folderMap[folder].Add(path);
        }

        // 2. Crear/registrar .asmdef en cada carpeta
        var asmDefs = new Dictionary<string, string>(); // nombre → ruta asmdef
        foreach (var kv in folderMap)
        {
            var folder = kv.Key;
            if (folder.Contains("/Packages/")) continue;

            var existing = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset", new[] { folder });
            if (existing.Length > 0)
            {
                var asmPath = AssetDatabase.GUIDToAssetPath(existing[0]);
                var name = Path.GetFileNameWithoutExtension(asmPath);
                asmDefs[name] = asmPath;
                continue;
            }

            var asmName = Path.GetFileName(folder);
            var asmPathNew = Path.Combine(folder, asmName + ".asmdef");
            var dataNew = new AssemblyDefData
            {
                name = asmName,
                references = new string[] { },
                includePlatforms = includePlatforms,
                optionalUnityReferences = new string[] { }
            };
            File.WriteAllText(asmPathNew, dataNew.ToJson());
            AssetDatabase.ImportAsset(asmPathNew);
            asmDefs[asmName] = asmPathNew;
            Debug.Log($"[AsmDefGen] Created {asmName}.asmdef in {folder}");
        }

        // 3. Resolver referencias
        foreach (var kv in asmDefs)
        {
            var asmName = kv.Key;
            var asmPath = kv.Value;
            var folder = Path.GetDirectoryName(asmPath);

            var deps = new HashSet<string>();
            foreach (var script in folderMap.GetValueOrDefault(folder, new List<string>()))
            {
                var allDeps = AssetDatabase.GetDependencies(script, true);
                foreach (var d in allDeps.Where(d => d.EndsWith(".cs")))
                {
                    var depFolder = Path.GetDirectoryName(d);
                    var owner = asmDefs.FirstOrDefault(x => Path.GetDirectoryName(x.Value) == depFolder).Key;
                    if (!string.IsNullOrEmpty(owner) && owner != asmName)
                        deps.Add(owner);
                }
            }

            var text = File.ReadAllText(asmPath);
            var data = AssemblyDefData.FromJson(text);
            if (deps.Count == 0)
            {
                deps.UnionWith(asmDefs.Keys.Where(name => name != asmName));
            }
            data.references = deps.ToArray();
            File.WriteAllText(asmPath, data.ToJson());
            AssetDatabase.ImportAsset(asmPath);
            Debug.Log($"[AsmDefGen] Updated refs for {asmName}: {string.Join(", ", deps)}");
        }

        AssetDatabase.Refresh();
        Debug.Log("[AsmDefGen] Done!");
    }

    // Helper para (de)serializar el JSON del .asmdef
    class AssemblyDefData
    {
        public string name;
        public string[] references;
        public string[] includePlatforms;
        public string[] optionalUnityReferences;

        // ← Agrega esto:
        public bool autoReferenced = true;

        public string ToJson() => JsonUtility.ToJson(this, prettyPrint: true);
        public static AssemblyDefData FromJson(string json) => JsonUtility.FromJson<AssemblyDefData>(json);
    }
}
