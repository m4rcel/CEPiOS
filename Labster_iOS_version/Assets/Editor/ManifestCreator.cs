using UnityEngine;
using System;
using System.IO;

public class ManifestCreator {
	
	public static void CreateManifest(string path){
		var splitString = path.Split("." [0]);
		var fileName = splitString[0]+".xml";
		var sr = File.CreateText(fileName);		
			sr.WriteLine ("<Version Value='" + ProduceVersion(DateTime.Now) + "'/>");
			sr.Close();
	}
	
	public static string ProduceVersion(DateTime date){
		var year = date.Year;
		var day = ("00" + date.DayOfYear).Substring(date.DayOfYear.ToString().Length-1);
		var hour = date.Hour;
		var minute = date.Minute;
	
		var outString = (year.ToString().Substring(2,2))+(day)+((hour*60)+minute).ToString();
		
		Debug.Log(string.Format("Scene Exported, Version : {0}", outString));
		return outString;
	}
}
