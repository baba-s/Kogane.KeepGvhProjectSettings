using System.IO;
using UnityEditor;
using UnityEditor.Compilation;

namespace Kogane.Internal
{
    /// <summary>
    /// GvhProjectSettings.xml が勝手に変更される不具合が GitHub の Issue で報告されているが
    /// まだ対応されていないため、暫定で対応するエディタ拡張
    /// https://github.com/googlesamples/unity-jar-resolver/issues/524
    /// </summary>
    [InitializeOnLoad]
    internal static class KeepGvhProjectSettings
    {
        //================================================================================
        // 関数(static)
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        static KeepGvhProjectSettings()
        {
            CompilationPipeline.compilationStarted          += _ => Restore();
            CompilationPipeline.compilationFinished         += _ => Restore();
            CompilationPipeline.assemblyCompilationFinished += ( _, _ ) => Restore();
            EditorApplication.delayCall                     += () => Restore();
            EditorFocusWatcher.OnFocused                    += () => Restore();

            Restore();

            static void Restore()
            {
                var sourcePath = "ProjectSettings/GvhProjectSettings.xml.source";

                if ( !File.Exists( sourcePath ) ) return;

                try
                {
                    File.Copy
                    (
                        sourceFileName: sourcePath,
                        destFileName: "ProjectSettings/GvhProjectSettings.xml",
                        overwrite: true
                    );
                }
                // 以下の例外を握りつぶす
                // IOException: The process cannot access the file 'XXXX\ProjectSettings\GvhProjectSettings.xml' because it is being used by another process.
                catch ( IOException )
                {
                }
            }
        }
    }
}