using UnityEngine;
using System.Collections.Generic;


// This script converts the Json response into classes understandable by Unity
public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.result;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> result;
    }
}