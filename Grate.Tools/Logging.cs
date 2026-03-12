using System;
using System.Diagnostics;
using System.Reflection;
using BepInEx.Logging;
using Grate.GUI;
using UnityEngine;

namespace Grate.Tools;

public static class Logging
{
	private static ManualLogSource logger;

	public static int DebuggerLines = 20;

	public static void Init()
	{
		logger = Logger.CreateLogSource("Grate");
	}

	public static void Exception(Exception e)
	{
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		logger.LogWarning((object)("(" + method.ReflectedType.Name + "." + method.Name + "()) " + string.Join(" ", e.Message, e.StackTrace)));
	}

	public static void Fatal(params object[] content)
	{
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		logger.LogFatal((object)("(" + method.ReflectedType.Name + "." + method.Name + "()) " + string.Join(" ", content)));
	}

	public static void Warning(params object[] content)
	{
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		logger.LogWarning((object)("(" + method.ReflectedType.Name + "." + method.Name + "()) " + string.Join(" ", content)));
	}

	public static void Info(params object[] content)
	{
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		logger.LogInfo((object)("(" + method.ReflectedType.Name + "." + method.Name + "()) " + string.Join(" ", content)));
	}

	public static void Debug(params object[] content)
	{
		MethodBase method = new StackTrace().GetFrame(1).GetMethod();
		logger.LogDebug((object)("(" + method.ReflectedType.Name + "." + method.Name + "()) " + string.Join("  ", content)));
	}

	public static void Debugger(params object[] content)
	{
		Debug(content);
		if (MenuController.Debugger && Object.op_Implicit((Object)(object)Plugin.DebugText))
		{
			Plugin.DebugText.text = PrependTextToLog(Plugin.DebugText.text, string.Join(" ", content));
		}
	}

	public static string PrependTextToLog(string log, string text)
	{
		log = text + "\n" + log;
		string[] array = log.Split('\n');
		if (array.Length > DebuggerLines)
		{
			log = string.Join("\n", array, 0, DebuggerLines);
		}
		return log;
	}
}
