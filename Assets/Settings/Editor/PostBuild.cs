//Script copiant les fichiers du dossier ExternalResources une fois le build realise

using System.IO;
using System.Security.Cryptography;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PostBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    //private const string externalResourcesEditor = "Assets\\ExternalResources";
    //private const string externalResourcesBuild = "ExternalResources";

    public void OnPostprocessBuild(BuildReport report)
    {
        //Debug.Log("Build complete, starting postbuild process...");
        //string unityProject = Path.GetFullPath(Directory.GetCurrentDirectory());
        //string buildFolder = Path.GetFullPath(report.summary.outputPath + "/../");

        //On copie les ressources externes du projet vers le build
        //Debug.Log($"Copying files from {externalResourcesEditor}...");
        //CopyFolderContent(new DirectoryInfo(Path.Combine(unityProject, externalResourcesEditor)), new DirectoryInfo(Path.Combine(buildFolder, externalResourcesBuild)));
    }

    public void CopyFolderContent(DirectoryInfo source, DirectoryInfo target)
    {
        if (source.Exists)
        {
            Directory.CreateDirectory(target.FullName);

            FileInfo[] existingFiles = target.GetFiles();

            //Copie les fichiers de la source vers la destination
            foreach (FileInfo sourceFileInformations in source.GetFiles())
            {
                //On verifie l'extension : si c'est un fichier generer par unity, on l'ignore
                if (sourceFileInformations.Extension == ".meta") continue;

                //On signal que le fichier etait deja present
                for(int i = 0; i < existingFiles.Length; i++)
                {
                    if (existingFiles[i] != null && existingFiles[i].Name == sourceFileInformations.Name) existingFiles[i] = null;
                }

                //On verifie si le fichier a ete modifie. Si ce n'est pas le cas, on l'ignore
                FileInfo destinationFileInformations = new FileInfo(Path.Combine(target.FullName, sourceFileInformations.Name));
                if (sourceFileInformations.LastWriteTimeUtc == destinationFileInformations.LastWriteTimeUtc) continue;

                //On copie finalement le fichier
                sourceFileInformations.CopyTo(destinationFileInformations.FullName, true);
            }

            //On supprime les fichiers qui n'ont plus leur place dans target
            foreach(FileInfo fileInfo in existingFiles)
            {
                if (fileInfo != null) File.Delete(fileInfo.FullName);
            }

            DirectoryInfo[] existingDirectories = target.GetDirectories();

            //Copie le contenu de chaque dossier de la source vers la destination
            foreach (DirectoryInfo sourceSubDirectoryInformations in source.GetDirectories())
            {
                DirectoryInfo targetSubDirectory = target.CreateSubdirectory(sourceSubDirectoryInformations.Name);

                //On signal que le dossier etait deja present
                for (int i = 0; i < existingDirectories.Length; i++)
                {
                    if (existingDirectories[i] != null && existingDirectories[i].Name == sourceSubDirectoryInformations.Name) existingDirectories[i] = null;
                }

                CopyFolderContent(sourceSubDirectoryInformations, targetSubDirectory);
            }

            //On supprime les dossiers qui n'ont plus leur place dans target
            foreach (DirectoryInfo directoryInfo in existingDirectories)
            {
                if (directoryInfo != null) Directory.Delete(directoryInfo.FullName, true);
            }
        }
        else
        {
            Debug.LogError($"Source path {source.FullName} doesn't exist.");
        }
    }
}