using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FootTactic
{
	public interface IDatabaseProvider
	{
		
		IEnumerator OpenTactic(string filePath, Action<string> onSuccess, Action<string> onError);

        void SaveTactic(string filePath, string contents, Action<string> onSuccess, Action<string> onError);

    }
}