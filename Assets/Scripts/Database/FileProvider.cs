using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.IO;

namespace FootTactic
{
	public class FileProvider : IDatabaseProvider
	{
		        
		public IEnumerator OpenTactic(string filePath,  Action<string> OnSuccess, Action<string> OnError){

            try
            {
                StreamReader reader = new StreamReader(filePath);
                string contents = reader.ReadToEnd();
                reader.Close();

                OnSuccess(contents);
            }
            catch(Exception ex)
            {
                OnError("[FT] Error openning file " + filePath + ": " + ex.Message);
            }
            yield return null;
            
        }

        public void SaveTactic(
            string filePath,
            string contents,
            Action<string> OnSuccess,
            Action<string> OnError
            )
        {
            try
            {
                File.WriteAllText(filePath, contents);
                OnSuccess(filePath);
            }
            catch(Exception ex)
            {
                OnError(ex.Message);
            }
        }

    }
}