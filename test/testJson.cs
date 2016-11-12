using UnityEngine;
using System.IO;
using System;
using System.Threading;

public class testJson : MonoBehaviour
{
    // Test Object
    public class TestObject
    {
        public TestObject() { }
        public int dataInt = 32;
        public float datafloat = 32.0f;
        public string dataString = "test data";
        public int[] dataIntArray = { 3, 1, 434, 234, 23, 42, 34, 25, 2, 51, 5 };
        public void Print()
        {
            string stringDataIntArray = "";
            for (int i = 0; i < dataIntArray.Length; i++) stringDataIntArray += " dataIntArray[" + i.ToString() + "]=" + dataIntArray[i].ToString();
            Debug.Log("dataInt=" + dataInt.ToString() + " datafloat=" + datafloat.ToString() + " dataInt=" + dataString.ToString() + " dataString=" + dataInt.ToString() + stringDataIntArray);
        }
    };

    // Use this for initialization
    void Start () {
        // Crate SimpleJSON Object 
        SimpleJSON.JSONClass jsonClass = new SimpleJSON.JSONClass();
        jsonClass.Add("TestKey1", "TestStringValue");
        jsonClass.Add("TestKey2TestString", new SimpleJSON.JSONNode("String"));
        jsonClass.Add("TestKey3TestInt", new SimpleJSON.JSONNode(123));
        jsonClass.Add("TestKey4TestFloat", new SimpleJSON.JSONNode(0.3f));
        jsonClass.Add("TestKey5TestDouble", new SimpleJSON.JSONNode(123.42));
        jsonClass.Add("TestKey6TestBool", new SimpleJSON.JSONNode(true));

        // Crate SimpleJSON Array
        SimpleJSON.JSONArray jsonArray = new SimpleJSON.JSONArray();
        for (int i = 0; i < 10; i++)
        {
            jsonArray.Add( new SimpleJSON.JSONNode("String"));
            jsonArray.Add( new SimpleJSON.JSONNode(123));
            jsonArray.Add( new SimpleJSON.JSONNode(0.3f));
            jsonArray.Add( new SimpleJSON.JSONNode(123.42));
            jsonArray.Add( new SimpleJSON.JSONNode(true));
        }

        jsonClass.Add("TestKey7TestArray", jsonArray);

        // serialize JSONClass to string
        string jsonClassToString = jsonClass.ToString();
        Debug.Log(jsonClassToString);

        // Parse String and deserialize to JSONClass
        SimpleJSON.JSONClass StringTojsonClass = SimpleJSON.JSONNode.Parse(jsonClassToString) as SimpleJSON.JSONClass;
        Debug.Log(StringTojsonClass.ToString());

        // serialize TestObject to JSONNode
        TestObject testObj = new TestObject();
        StringTojsonClass.Add("TestObgect", SimpleJSON.JSONNode.SerializeObjectToJSONNode(testObj));
        Debug.Log(StringTojsonClass["TestObgect"].ToString());
        Debug.Log(UnityEngine.JsonUtility.ToJson(testObj));
        
        // DeSerialize TestObject from JSONNode
        TestObject testObj2 = SimpleJSON.JSONNode.DeserializeObjectFromJSONNode(StringTojsonClass["TestObgect"], typeof(TestObject)) as TestObject;
        testObj2.Print();


        Debug.Log(StringTojsonClass.ToString());
        //  File.WriteAllText(SaveLoadDelete.GetPatch() + "\\Resources\\XML\\ShopItems.json", JSONClass.Parse(Resources.Load<TextAsset>("XML/ShopItems").text).ToString());

    }

    // Update is called once per frame
    void Update () {
	
	}
}
