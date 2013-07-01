using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public class ImportSettings : AssetPostprocessor{
	
        void OnPreprocessModel(){
            var modelImporter = assetImporter as ModelImporter;
            if (modelImporter == null) return;
//            modelImporter.globalScale = 0.1f;
            modelImporter.importMaterials = false;
//            modelImporter.meshCompression = ModelImporterMeshCompression.High;
//            modelImporter.swapUVChannels = false;
            modelImporter.generateSecondaryUV = true;
			modelImporter.animationType = ModelImporterAnimationType.Legacy;
        }
    }
}
