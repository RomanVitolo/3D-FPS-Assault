#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RV_Unity_MultipleBuilds.Editor
{
    public class MultiBuildTool : EditorWindow
    {
        [MenuItem("RV - Template Tool/Multiple Builds Tool")]
        public static void OnShowTools()
        {
            GetWindow<MultiBuildTool>("Multiple Builds");
        }

        Dictionary<BuildTarget, bool> TargetsToBuild = new Dictionary<BuildTarget, bool>();
        List<BuildTarget> AvailableTargets = new List<BuildTarget>();

        private BuildTargetGroup GetTargetGroupForTarget(BuildTarget target) => target switch
        {
            BuildTarget.StandaloneOSX => BuildTargetGroup.Standalone,
            BuildTarget.StandaloneWindows => BuildTargetGroup.Standalone,
            BuildTarget.iOS => BuildTargetGroup.iOS,
            BuildTarget.Android => BuildTargetGroup.Android,
            BuildTarget.StandaloneWindows64 => BuildTargetGroup.Standalone,
            BuildTarget.WebGL => BuildTargetGroup.WebGL,
            BuildTarget.StandaloneLinux64 => BuildTargetGroup.Standalone,
            _ => BuildTargetGroup.Unknown
        };

        private void OnEnable()
        {
            AvailableTargets.Clear();
            var buildTargets = System.Enum.GetValues(typeof(BuildTarget));
            foreach (var buildTargetValue in buildTargets)
            {
                BuildTarget target = (BuildTarget)buildTargetValue;

                // Skip if unsupported
                if (!BuildPipeline.IsBuildTargetSupported(GetTargetGroupForTarget(target), target))
                    continue;

                AvailableTargets.Add(target);

                // Add the target if not in the build list
                if (!TargetsToBuild.ContainsKey(target))
                    TargetsToBuild[target] = false;
            }

            // Check if any targets have gone away
            if (TargetsToBuild.Count > AvailableTargets.Count)
            {
                // Build the list of removed targets
                List<BuildTarget> targetsToRemove = new List<BuildTarget>();
                foreach (var target in TargetsToBuild.Keys)
                {
                    if (!AvailableTargets.Contains(target))
                        targetsToRemove.Add(target);
                }

                // Cleanup the removed targets
                foreach (var target in targetsToRemove)
                    TargetsToBuild.Remove(target);
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Platforms to Build", EditorStyles.boldLabel);

            // Display the build targets
            int numEnabled = 0;
            foreach (var target in AvailableTargets)
            {
                TargetsToBuild[target] = EditorGUILayout.Toggle(target.ToString(), TargetsToBuild[target]);

                if (TargetsToBuild[target])
                    numEnabled++;
            }

            if (numEnabled > 0)
            {
                // Attempt to build?
                string prompt = numEnabled == 1 ? "Build 1 Platform" : $"Build {numEnabled} Platforms";
                if (GUILayout.Button(prompt))
                {
                    List<BuildTarget> selectedTargets = new List<BuildTarget>();
                    foreach (var target in AvailableTargets)
                    {
                        if (TargetsToBuild[target])
                            selectedTargets.Add(target);
                    }

                    EditorCoroutineUtility.StartCoroutine(PerformBuild(selectedTargets), this);
                }
            }
        }

        IEnumerator PerformBuild(List<BuildTarget> targetsToBuild)
        {
            // Show the progress display
            int buildAllProgressID = Progress.Start("Build All", "Building all selected platforms", Progress.Options.Sticky);
            Progress.ShowDetails();
            yield return new EditorWaitForSeconds(1f);

            BuildTarget originalTarget = EditorUserBuildSettings.activeBuildTarget;
            BuildTargetGroup originalTargetGroup = GetTargetGroupForTarget(originalTarget);

            // Build each target
            for (int targetIndex = 0; targetIndex < targetsToBuild.Count; ++targetIndex)
            {
                var buildTarget = targetsToBuild[targetIndex];
                BuildTargetGroup buildTargetGroup = GetTargetGroupForTarget(buildTarget);

                Progress.Report(buildAllProgressID, targetIndex + 1, targetsToBuild.Count);
                int buildTaskProgressID = Progress.Start($"Build {buildTarget.ToString()}", null, Progress.Options.Sticky, buildAllProgressID);
                yield return new EditorWaitForSeconds(1f);

                // Switch to the target platform
                if (EditorUserBuildSettings.activeBuildTarget != buildTarget)
                {
                    bool switchResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(buildTargetGroup, buildTarget);
                    while (!switchResult)
                    {
                        yield return null;
                        switchResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(buildTargetGroup, buildTarget);
                    }
                }

                // Perform the build
                if (!BuildIndividualTarget(buildTarget))
                {
                    Progress.Finish(buildTaskProgressID, Progress.Status.Failed);
                    Progress.Finish(buildAllProgressID, Progress.Status.Failed);

                    // Switch back to the original target if the build fails
                    if (EditorUserBuildSettings.activeBuildTarget != originalTarget)
                    {
                        bool switchBackResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(originalTargetGroup, originalTarget);
                        while (!switchBackResult)
                        {
                            yield return null;
                            switchBackResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(originalTargetGroup, originalTarget);
                        }
                    }

                    yield break;
                }

                Progress.Finish(buildTaskProgressID, Progress.Status.Succeeded);
                yield return new EditorWaitForSeconds(1f);
            }

            Progress.Finish(buildAllProgressID, Progress.Status.Succeeded);

            // Switch back to the original target after all builds are done
            if (EditorUserBuildSettings.activeBuildTarget != originalTarget)
            {
                bool switchBackResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(originalTargetGroup, originalTarget);
                while (!switchBackResult)
                {
                    yield return null;
                    switchBackResult = EditorUserBuildSettings.SwitchActiveBuildTargetAsync(originalTargetGroup, originalTarget);
                }
            }

            yield return null;
        }

        bool BuildIndividualTarget(BuildTarget target)
        {
            BuildPlayerOptions options = new BuildPlayerOptions();

            // Get the list of scenes from the Editor Build Settings
            List<string> scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled) // Ensure only enabled scenes are included
                    scenes.Add(scene.path);
            }

            // Configure the build
            options.scenes = scenes.ToArray();
            options.target = target;
            options.targetGroup = GetTargetGroupForTarget(target);

            // Set the location path name
            string buildPath = System.IO.Path.Combine("Builds", target.ToString());
            if (target == BuildTarget.Android)
            {
                string apkName = PlayerSettings.productName + ".apk";
                options.locationPathName = System.IO.Path.Combine(buildPath, apkName);
            }
            else if (target == BuildTarget.StandaloneWindows64)
            {
                options.locationPathName = System.IO.Path.Combine(buildPath, PlayerSettings.productName + ".exe");
            }
            else if (target == BuildTarget.StandaloneLinux64)
            {
                options.locationPathName = System.IO.Path.Combine(buildPath, PlayerSettings.productName + ".x86_64");
            }
            else
            {
                options.locationPathName = System.IO.Path.Combine(buildPath, PlayerSettings.productName);
            }

            // Set build options
            if (BuildPipeline.BuildCanBeAppended(target, options.locationPathName) == CanAppendBuild.Yes)
                options.options = BuildOptions.AcceptExternalModificationsToPlayer;
            else
                options.options = BuildOptions.None;

            // Start the build
            BuildReport report = BuildPipeline.BuildPlayer(options);

            // Check if the build was successful
            if (report.summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build for {target.ToString()} completed in {report.summary.totalTime.Seconds} seconds");
                return true;
            }

            Debug.LogError($"Build for {target.ToString()} failed");
            return false;
        }
    }
}
#endif