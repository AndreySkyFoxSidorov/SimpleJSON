using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
    public class JSONNode
    {
        public enum JSONTypeTag
        {
            Array = 1,
            Class = 2,
            Value = 3,
            IntValue = 4,
            DoubleValue = 5,
            BoolValue = 6,
            FloatValue = 7,
        }

        public virtual void Add(string aKey, JSONNode aItem) { }
        public virtual JSONNode this[int aIndex] { get { return null; } set { } }
        public virtual JSONNode this[string aKey] { get { return null; } set { } }
        public virtual JSONTypeTag ValueTag { get { return JSONTypeTag.Value; } set { } }
        public virtual int Count { get { return 0; } }

        private string m_Data;
        public virtual string Value
        {
            get
            {
                return m_Data;
            }
            set
            {
                m_Data = value;
            }
        }
        public JSONNode()
        {
            ValueTag = JSONTypeTag.Value;
            m_Data = "";
        }
        public JSONNode(string aData)
        {
            ValueTag = JSONTypeTag.Value;
            m_Data = aData;
        }
        public JSONNode(float aData)
        {
            ValueTag = JSONTypeTag.FloatValue;
            AsFloat = aData;
        }
        public JSONNode(double aData)
        {
            ValueTag = JSONTypeTag.DoubleValue;
            AsDouble = aData;
        }
        public JSONNode(bool aData)
        {
            ValueTag = JSONTypeTag.BoolValue;
            AsBool = aData;
        }
        public JSONNode(int aData)
        {
            ValueTag = JSONTypeTag.IntValue;
            AsInt = aData;
        }

        public virtual void Add(JSONNode aItem)
        {
            Add("", aItem);
        }

        public virtual JSONNode Remove(string aKey)
        {
            return null;
        }
        public virtual JSONNode Remove(int aIndex)
        {
            return null;
        }
        public virtual JSONNode Remove(JSONNode aNode)
        {
            return aNode;
        }

        public virtual IEnumerable<JSONNode> Childs { get { yield break; } }
        public IEnumerable<JSONNode> DeepChilds
        {
            get
            {
                foreach (var C in Childs)
                    foreach (var D in C.DeepChilds)
                        yield return D;
            }
        }

        public override string ToString()
        {
            if (ValueTag == JSONTypeTag.Value)
            {
                return "\"" + Value + "\"";
            }
            else
            {
                return Value;
            }
        }

        public virtual string ToString(string aPrefix)
        {
            if (ValueTag == JSONTypeTag.Value)
            {
                return "\"" + Value + "\"";
            }
            else
            {
                return Value;
            }
        }

        public virtual int AsInt
        {
            get
            {
                int v = 0;
                if (int.TryParse(Value, out v))
                {
                    return v;
                }
                return 0;
            }
            set
            {
                ValueTag = JSONTypeTag.IntValue;
                Value = value.ToString();
            }
        }
        public virtual float AsFloat
        {
            get
            {
                float v = 0.0f;
                if (float.TryParse(Value, out v))
                {
                    return v;
                }
                return 0.0f;
            }
            set
            {
                ValueTag = JSONTypeTag.FloatValue;
                Value = value.ToString();
            }
        }
        public virtual double AsDouble
        {
            get
            {
                double v = 0.0;
                if (double.TryParse(Value, out v))
                {
                    return v;
                }
                return 0.0;
            }
            set
            {
                ValueTag = JSONTypeTag.DoubleValue;
                Value = value.ToString();
            }
        }
        public virtual bool AsBool
        {
            get
            {
                bool v = false;
                if (bool.TryParse(Value, out v))
                {
                    return v;
                }
                return !string.IsNullOrEmpty(Value);
            }
            set
            {
                ValueTag = JSONTypeTag.BoolValue;
                Value = (value) ? "true" : "false";
            }
        }
        public virtual JSONArray AsArray
        {
            get
            {
                return this as JSONArray;
            }
        }
        public virtual JSONClass AsObject
        {
            get
            {
                return this as JSONClass;
            }
        }

        public static implicit operator JSONNode(string s)
        {
            return new JSONNode(s);
        }

        public static implicit operator JSONNode(int s)
        {
            return new JSONNode(s);
        }

        public static implicit operator JSONNode(float s)
        {
            return new JSONNode(s);
        }

        public static implicit operator JSONNode(double s)
        {
            return new JSONNode(s);
        }

        public static implicit operator JSONNode(bool s)
        {
            return new JSONNode(s);
        }

        public static implicit operator string(JSONNode d)
        {
            return (d == null) ? null : d.Value;
        }

        public static implicit operator int(JSONNode d)
        {
            return (d == null) ? 0 : d.AsInt;
        }

        public static implicit operator float(JSONNode d)
        {
            return (d == null) ? 0.0f : d.AsFloat;
        }

        public static implicit operator double(JSONNode d)
        {
            return (d == null) ? 0.0 : d.AsDouble;
        }

        public static implicit operator bool(JSONNode d)
        {
            return (d == null) ? false : d.AsBool;
        }

        public static bool operator ==(JSONNode a, object b)
        {
            if (b == null && a is JSONCreator)
            {
                return true;
            }
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONNode a, object b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static string SerializeObject(object pObject)
        {
            return UnityEngine.JsonUtility.ToJson(pObject);
        }

        public static JSONNode SerializeObjectToJSONNode(object pObject)
        {
            return Parse(UnityEngine.JsonUtility.ToJson(pObject));
        }

        public static object DeserializeObject(string dataString, System.Type UserData)
        {
            if (dataString != null)
            {
                return UnityEngine.JsonUtility.FromJson(dataString, UserData);
            }
            else
            {
                return null;
            }
        }

        public static object DeserializeObjectFromJSONNode(JSONNode data, System.Type UserData)
        {
            if (data != null)
            {
                return UnityEngine.JsonUtility.FromJson(data.ToString(), UserData);
            }
            else
            {
                return null;
            }
        }


        public static string FormatJson(string UnFormatJson, string INDENT_STRING = "\t")
        {
            var indent = 0;
            var QuoteMode = false;
            var sb = new StringBuilder();
            for (var i = 0; i < UnFormatJson.Length; i++)
            {
                var ch = UnFormatJson[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!QuoteMode)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!QuoteMode)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && UnFormatJson[--index] == '\\')
                        {
                            escaped = !escaped;
                        }
                        if (!escaped)
                        {
                            QuoteMode ^= true;
                        }
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!QuoteMode)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!QuoteMode)
                        {
                            sb.Append(" ");
                        }
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }


        public static JSONNode Parse(string aJSONstring)
        {
            if (aJSONstring == null)
            {
                throw new Exception("JSON Parse: data is null.");
            }
            string aJSON = aJSONstring.Replace("\r", "");
            aJSON = aJSONstring.Replace("\n", "");
            Stack<JSONNode> stack = new Stack<JSONNode>();
            JSONNode ctx = null;
            int i = 0;
            string Token = "";
            string TokenName = "";
            bool QuoteMode = false;
            while (i < aJSON.Length)
            {
                switch (aJSON[i])
                {
                    case '{':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        stack.Push(new JSONClass());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();
                            if (ctx is JSONArray)
                            {
                                ctx.Add(stack.Peek());
                            }
                            else if (TokenName != "")
                            {
                                ctx.Add(TokenName, stack.Peek());
                            }
                        }
                        TokenName = "";
                        Token = "";
                        ctx = stack.Peek();
                        break;

                    case '[':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }

                        stack.Push(new JSONArray());
                        if (ctx != null)
                        {
                            TokenName = TokenName.Trim();
                            if (ctx is JSONArray)
                            {
                                ctx.Add(stack.Peek());
                            }
                            else if (TokenName != "")
                            {
                                ctx.Add(TokenName, stack.Peek());
                            }
                        }
                        TokenName = "";
                        Token = "";
                        ctx = stack.Peek();
                        break;

                    case '}':
                    case ']':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        if (stack.Count == 0)
                        {
                            throw new Exception("JSON Parse: Too many closing brackets");
                        }

                        stack.Pop();
                        if (Token != "")
                        {
                            TokenName = TokenName.Trim();
                            if (ctx is JSONArray)
                            {
                                ctx.Add(ParseHelper(Token, aJSON, i));
                            }
                            else if (TokenName != "")
                            {
                                ctx.Add(TokenName, ParseHelper(Token, aJSON, i));
                            }
                        }
                        TokenName = "";
                        Token = "";
                        if (stack.Count > 0)
                        {
                            ctx = stack.Peek();
                        }
                        break;

                    case ':':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        TokenName = Token;
                        Token = "";
                        break;

                    case '"':
                        QuoteMode ^= true;
                        break;

                    case ',':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                            break;
                        }
                        if (Token != "")
                        {
                            if (ctx is JSONArray)
                            {
                                ctx.Add(ParseHelper(Token, aJSON, i));
                            }
                            else if (TokenName != "")
                            {
                                ctx.Add(TokenName, ParseHelper(Token, aJSON, i));
                            }
                        }
                        TokenName = "";
                        Token = "";
                        break;

                    case '\r':
                    case '\n':
                        break;

                    case ' ':
                    case '\t':
                        if (QuoteMode)
                        {
                            Token += aJSON[i];
                        }
                        break;

                    case '\\':
                        ++i;
                        if (QuoteMode)
                        {
                            char C = aJSON[i];
                            switch (C)
                            {
                                case 't':
                                    Token += '\t';
                                    break;
                                case 'r':
                                    Token += '\r';
                                    break;
                                case 'n':
                                    Token += '\n';
                                    break;
                                case 'b':
                                    Token += '\b';
                                    break;
                                case 'f':
                                    Token += '\f';
                                    break;
                                case 'u':
                                    {
                                        string s = aJSON.Substring(i + 1, 4);
                                        Token += (char)int.Parse(s, System.Globalization.NumberStyles.AllowHexSpecifier);
                                        i += 4;
                                        break;
                                    }
                                default:
                                    Token += C;
                                    break;
                            }
                        }
                        break;

                    default:
                        Token += aJSON[i];
                        break;
                }
                ++i;
            }
            if (QuoteMode)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }
            return ctx;
        }

        private static JSONNode ParseHelper(string aJSONstring, string aJSON, int i)
        {
            int b = i;
            while (aJSON[b] == '\t' || aJSON[b] == '\n' || aJSON[b] == '\r' || aJSON[b] == ' ' || aJSON[b] == '\0')
            {
                if (b > 3)
                {
                    b--;
                }
                else
                {
                    break;
                }
                if (aJSON[b] == '\"')
                {
                    break;
                }
            }

            if (aJSON[b] == '\"' || aJSON[b - 1] == '\"' || aJSON[b - 2] == '\"')
            {
                return (new JSONNode(aJSONstring));
            }
            else
            {
                if (aJSONstring.Contains("."))
                {
                    return (new JSONNode(float.Parse(aJSONstring)));
                }
                else if (aJSONstring.Contains("true") || aJSONstring.Contains("false"))
                {
                    return (new JSONNode(bool.Parse(aJSONstring)));
                }
                else
                {
                    try
                    {
                        return (new JSONNode(int.Parse(aJSONstring)));
                    }
                    catch
                    {
                        return (new JSONNode(aJSONstring));
                    }

                }
            }

            return (new JSONNode(aJSONstring));
        }

    } // End of JSONNode

    public class JSONArray : JSONNode, IEnumerable
    {
        private List<JSONNode> m_List = new List<JSONNode>();
        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                {
                    return new JSONCreator(this);
                }
                return m_List[aIndex];
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                {
                    m_List.Add(value);
                }
                else
                { m_List[aIndex] = value; }
            }
        }
        public override JSONNode this[string aKey]
        {
            get { return new JSONCreator(this); }
            set { m_List.Add(value); }
        }
        public override int Count
        {
            get { return m_List.Count; }
        }
        public override void Add(string aKey, JSONNode aItem)
        {
            m_List.Add(aItem);
        }
        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
            {
                return null;
            }
            JSONNode tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }
        public override JSONNode Remove(JSONNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }
        public override IEnumerable<JSONNode> Childs
        {
            get
            {
                foreach (JSONNode N in m_List)
                    yield return N;
            }
        }
        public IEnumerator GetEnumerator()
        {
            foreach (JSONNode N in m_List)
                yield return N;
        }
        public override string ToString()
        {
            string result = "[";
            foreach (JSONNode N in m_List)
            {
                if (result.Length > 2)
                {
                    result += ",";
                }
                result += N.ToString();
            }
            result += "]";
            return result;
        }
        public override string ToString(string aPrefix)
        {
            string result = "[";
            foreach (JSONNode N in m_List)
            {
                if (result.Length > 3)
                {
                    result += ",";
                }
                result += "\n" + aPrefix + "   ";
                result += N.ToString(aPrefix + "   ");
            }
            result += "\n" + aPrefix + "]";
            return result;
        }
    } // End of JSONArray

    public class JSONClass : JSONNode, IEnumerable
    {
        public KeyValuePair<string, JSONNode> GetRootNode()
        {
            if (m_Dict.Keys.Count > 1)
            {
                throw new Exception("JSONClass contains more than one class description");
            }
            foreach (KeyValuePair<string, JSONNode> kvp in m_Dict)
            {
                return kvp;
            }
            throw new Exception("JSONClass is empty");
        }

        private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();
        public override JSONNode this[string aKey]
        {
            get
            {
                if (m_Dict.ContainsKey(aKey))
                {
                    return m_Dict[aKey];
                }
                else
                { return new JSONCreator(this, aKey); }
            }
            set
            {
                if (m_Dict.ContainsKey(aKey))
                {
                    m_Dict[aKey] = value;
                }
                else
                { m_Dict.Add(aKey, value); }
            }
        }
        public override JSONNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                {
                    return null;
                }
                return m_Dict.ElementAt(aIndex).Value;
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_Dict.Count)
                {
                    return;
                }
                string key = m_Dict.ElementAt(aIndex).Key;
                m_Dict[key] = value;
            }
        }
        public override int Count
        {
            get { return m_Dict.Count; }
        }


        public override void Add(string aKey, JSONNode aItem)
        {
            if (!string.IsNullOrEmpty(aKey))
            {
                if (m_Dict.ContainsKey(aKey))
                {
                    m_Dict[aKey] = aItem;
                }
                else
                {
                    m_Dict.Add(aKey, aItem);
                }
            }
            else
            {
                m_Dict.Add(Guid.NewGuid().ToString(), aItem);
            }
        }

        public override JSONNode Remove(string aKey)
        {
            if (!m_Dict.ContainsKey(aKey))
            {
                return null;
            }
            JSONNode tmp = m_Dict[aKey];
            m_Dict.Remove(aKey);
            return tmp;
        }
        public override JSONNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_Dict.Count)
            {
                return null;
            }
            var item = m_Dict.ElementAt(aIndex);
            m_Dict.Remove(item.Key);
            return item.Value;
        }
        public override JSONNode Remove(JSONNode aNode)
        {
            try
            {
                var item = m_Dict.Where(k => k.Value == aNode).First();
                m_Dict.Remove(item.Key);
                return aNode;
            }
            catch
            {
                return null;
            }
        }

        public override IEnumerable<JSONNode> Childs
        {
            get
            {
                foreach (KeyValuePair<string, JSONNode> N in m_Dict)
                    yield return N.Value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<string, JSONNode> N in m_Dict)
                yield return N;
        }

        public override string ToString()
        {
            string result = "{";
            foreach (KeyValuePair<string, JSONNode> N in m_Dict)
            {
                if (result.Length > 2)
                {
                    result += ",";
                }
                if (N.Value.ValueTag == JSONTypeTag.Value)
                {
                    result += "\"" + N.Key + "\":" + N.Value.ToString();
                }
                else
                {
                    result += "\"" + N.Key + "\":" + N.Value.ToString().Replace("\"", "");
                }
            }
            result += "}";
            return result;
        }

        public override string ToString(string aPrefix)
        {
            string result = "{";
            foreach (KeyValuePair<string, JSONNode> N in m_Dict)
            {
                if (result.Length > 3)
                {
                    result += ",";
                }
                result += "\n" + aPrefix + "   ";
                if (N.Value.ValueTag == JSONTypeTag.Value)
                {
                    result += "\"" + N.Key + "\":" + N.Value.ToString();
                }
                else
                {
                    result += "\"" + N.Key + "\":" + N.Value.ToString(aPrefix + "   ").Replace("\"", "");
                }
            }
            result += "\n" + aPrefix + "}";
            return result;
        }
    } // End of JSONClass


    internal class JSONCreator : JSONNode
    {
        private JSONNode m_Node = null;
        private string m_Key = null;

        public JSONCreator(JSONNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }
        public JSONCreator(JSONNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }

        private void Set(JSONNode aVal)
        {
            if (m_Key == null)
            {
                m_Node.Add(aVal);
            }
            else
            {
                m_Node.Add(m_Key, aVal);
            }
            m_Node = null; // Be GC friendly.
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                return new JSONCreator(this);
            }
            set
            {
                var tmp = new JSONArray();
                tmp.Add(value);
                Set(tmp);
            }
        }

        public override JSONNode this[string aKey]
        {
            get
            {
                return new JSONCreator(this, aKey);
            }
            set
            {
                var tmp = new JSONClass();
                tmp.Add(aKey, value);
                Set(tmp);
            }
        }
        public override void Add(JSONNode aItem)
        {
            var tmp = new JSONArray();
            tmp.Add(aItem);
            Set(tmp);
        }
        public override void Add(string aKey, JSONNode aItem)
        {
            var tmp = new JSONClass();
            tmp.Add(aKey, aItem);
            Set(tmp);
        }
        public static bool operator ==(JSONCreator a, object b)
        {
            if (b == null)
            {
                return true;
            }
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONCreator a, object b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            return System.Object.ReferenceEquals(this, obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "";
        }
        public override string ToString(string aPrefix)
        {
            return "";
        }

        public override int AsInt
        {
            get
            {
                JSONNode tmp = new JSONNode(0);
                Set(tmp);
                return 0;
            }
            set
            {
                JSONNode tmp = new JSONNode(value);
                Set(tmp);
            }
        }
        public override float AsFloat
        {
            get
            {
                JSONNode tmp = new JSONNode(0.0f);
                Set(tmp);
                return 0.0f;
            }
            set
            {
                JSONNode tmp = new JSONNode(value);
                Set(tmp);
            }
        }
        public override double AsDouble
        {
            get
            {
                JSONNode tmp = new JSONNode(0.0);
                Set(tmp);
                return 0.0;
            }
            set
            {
                JSONNode tmp = new JSONNode(value);
                Set(tmp);
            }
        }
        public override bool AsBool
        {
            get
            {
                JSONNode tmp = new JSONNode(false);
                Set(tmp);
                return false;
            }
            set
            {
                JSONNode tmp = new JSONNode(value);
                Set(tmp);
            }
        }
        public override JSONArray AsArray
        {
            get
            {
                JSONArray tmp = new JSONArray();
                Set(tmp);
                return tmp;
            }
        }
        public override JSONClass AsObject
        {
            get
            {
                JSONClass tmp = new JSONClass();
                Set(tmp);
                return tmp;
            }
        }
    } // End of JSONCreator

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}