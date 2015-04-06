using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public static class CommandLine
{
    public static void Build()
    {
        //TODO: Many many things need clearing up here

        var args = Environment.GetCommandLineArgs();

        var buildPath = "";
        var devBuild = false;
        var targets = new BuildTarget[0];

        for (var i = 0; i < args.Length; ++i)
        {
            if (args[i] == "--BuildPath")
            {
                buildPath = args[i + 1];
            }

            if (args[i] == "--MakeDevBuild")
            {
                devBuild = true;
            }

            if (args[i] == "--Targets")
            {
                targets = args[i + 1].Split(',').Select(str => Enum.Parse(typeof(BuildTarget), str, true)).OfType<BuildTarget>().ToArray();
            }
        }

        string[] scenes =
        {
            "Assets/Scenes/Main.unity",
        };

        var buildOptions = BuildOptions.None;

        if (devBuild)
        {
            buildOptions |= (BuildOptions.Development | BuildOptions.AllowDebugging);
        }

        var error = string.Empty;
        foreach (var buildTarget in targets)
        {
            var buildError = BuildPipeline.BuildPlayer(scenes, buildPath, buildTarget, buildOptions);
            if (!string.IsNullOrEmpty(buildError))
            {
                error += "Error building " + buildTarget + ": " + buildError;
            }
        }

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception("Build failed: " + error);
        }
    }
}
