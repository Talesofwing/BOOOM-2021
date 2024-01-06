/*
    ScriptKeywordProcessor.cs
    Author: Kuma
    Created-Date: 2019-01-06
    
    -----Description-----
    Process C# script file keyword.
*/






using UnityEditor;

namespace Kuma.Editor.Utils {

    public class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor {

        private static void OnWillCreateAsset (string path) {
            // Eliminate isn't C# script file.
            path = path.Replace (".meta", "");
            int index = path.LastIndexOf ('.');
            if (index < 0) 
                return;

            string suffix = path.Substring (index);
            if (suffix != ".cs")
                return;

            // File exists
            if (!System.IO.File.Exists(path))
                return;

            // Read file content
            string fileContent = System.IO.File.ReadAllText (path);
            // Replace keyword
            fileContent = fileContent.Replace ("#CREATIONDATE#", System.DateTime.Now.ToString ("yyyy-MM-dd"));
            // Write content to file
            System.IO.File.WriteAllText (path, fileContent);
            // Refresh asset
            AssetDatabase.Refresh ();
        }

    }

}