using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Json {

    public class JSONObject : Dictionary<string, object> {}
    public class JSONArray : List<object>  {}
    public class JsonNode {}

    public class JsonSyntaxError : Exception {
        public JsonSyntaxError(string message, int lineno, string source) {

        }
    }



  public class JSON {

    static Regex rx_escapable = new Regex("@[\\\\\\\"\\u0000-\\u001f\\u007f-\\u009f\\u00ad\\u0600-\\u0604\\u070f\\u17b4\\u17b5\\u200c-\\u200f\\u2028-\\u202f\\u2060-\\u206f\\ufeff\\ufff0-\\uffff]");

    private static int at;      // The index of the current character
    private static string ch;   // The current character

    private static Dictionary<string, string> _escapee;
    private static Dictionary<string, string> escapee {
        get {
            if (_escapee == null) {
                _escapee = new Dictionary<string, string>();
                _escapee.Add("b", "\b");
                _escapee.Add("f", "\f");
                _escapee.Add("n", "\n");
                _escapee.Add("r", "\r");
                _escapee.Add("t", "\t");

            }
            return _escapee;
        }
    }

    private static Dictionary<string, string> _meta;
    private static Dictionary<string, string> meta {
        get {
            if (_meta == null) {
                _meta = new Dictionary<string, string>();
                _meta.Add("\b", "b");
                _meta.Add("\f", "f");
                _meta.Add("\n", "n");
                _meta.Add("\r", "r");
                _meta.Add("\t", "t");
                _meta.Add("\\", "\\\\");
            }
            return _meta;
        }
    }

    private static string text;

    private static bool IsNumber(object value) {
        return value is sbyte
            || value is byte
            || value is short
            || value is ushort
            || value is int
            || value is uint
            || value is long
            || value is ulong
            || value is float
            || value is double
            || value is decimal;
    }

    static string gap;
    static string indent;

    public static JSONArray Array(object value) {
        return (JSONArray)value;
    }

    public static JSONObject Object(object value) {
        return (JSONObject)value;
    }

    public static string Stringify(object value, object space=null) {

        // The stringify method takes a value and an optional replacer, and an optional
        // space parameter, and returns a JSON text. The replacer can be a function
        // that can replace values, or an array of strings that will select the keys.
        // A default replacer method can be provided. Use of the space parameter can
        // produce text that is more easily readable.

        int i = 0;
        gap = "";
        indent = "";

        // If the space parameter is a number, make an indent string containing that
        // many spaces.
        if (IsNumber(space)) {
            for (i = 0; i < (int)space; i += 1) {
                indent += ' ';
            }
        }

        // If the space parameter is a string, it will be used as the indent string.
        if (space is String) {
            indent = (string)space;
        }
 
        // Make a fake root object containing our value under the key of ''.
        // Return the result of stringifying the value.
        var root = new JSONObject();
        var iroot = (JSONObject)root;
        iroot.Add("", value);

        return Str("", root);
    }

    public static object Parse(string source) {

        // This is a function that can parse a JSON text, producing a JavaScript
        // data structure. It is a simple, recursive descent parser. It does not use
        // eval or regular expressions, so it can be used as a model for implementing
        // a JSON parser in other languages.

        object result;

        text = source;
        at = 0;
        ch = " ";
        result = Value();
        White();

        if (ch.Length > 0) {
            Error("Syntax error");
        }

        return result;

    }



    static string Quote(string str) {
        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.

        return rx_escapable.IsMatch(str)
            ? rx_escapable.Replace(str, delegate(Match m) {
                return meta.ContainsKey(m.Value) 
                    ? meta[m.Value]
                    : "\\u"+((int)m.Value[0]).ToString("X");
            })
            : "\""+str+"\"";

        
    }

    static string Str(object key, object holder) {
        // Produce a string from holder[key].

      int i = 0;              // The loop counter.
      int length = 0;
      string mind = gap;


      var ihash = holder as JSONObject;
      var ilist= holder as JSONArray;

      var value = ihash == null ? ilist[(int)key] : ihash[(string)key];

      // What happens next depends on the value's type.
      if (value is String) {
        return Quote((string)value);
      } else if (IsNumber(value)) {
        return Convert.ToString(value);
      } else if (value is Boolean) {
        return Convert.ToString(value).ToLower();
      } else if (value is JSONArray) {

        // The value is an array. Stringify every element. Use null as a placeholder
        // for non-JSON values.

        gap += indent;
        var array = value as JSONArray;
        var results = new List<string>();
        length = array.Count;
        for (i=0; i<length; i++) {
            results.Add(Str(i, value));
        }

        // Join all of the elements together, separated with commas, and wrap them in
        // brackets.

       string v = results.Count == 0
            ? "[]"
            : gap.Length > 0
                ? "[\n" + gap + string.Join(",\n"+gap, results.ToArray()) +  "\n" + mind + "]"
                : "[" + string.Join(", ", results.ToArray()) + "]";
        gap = mind;
        return v;

      } else if (value is JSONObject) {

        // Otherwise, iterate through all of the keys in the object.

        gap += indent;
        var dictionary = value as Dictionary< string, object >;
        var results = new List<string>();
        foreach (KeyValuePair< string, object > pair in dictionary) {
            results.Add(Quote(pair.Key) + (
                gap.Length > 0 
                    ? ": "
                    : ":"
            ) + Str(pair.Key, dictionary)) ;        
        }

        // Join all of the member texts together, separated with commas,
        // and wrap them in braces.

        string v = results.Count == 0
            ? "{}"
            : gap.Length > 0
                ? "{\n" + gap + string.Join(",\n" + gap, results.ToArray()) + "\n" + mind + "}"
                : "{" + string.Join(", ", results.ToArray()) + "}";
         gap = mind;
        return v;
      }
      return "";
    }

    static JsonSyntaxError Error(string m) {
        // Call error when something is wrong.
        return new JsonSyntaxError(m, at, text);
    }

    static bool IsChar(string chars) {
        if (ch.Length == 0) return false;
        return chars.IndexOf(ch) != -1;
    }

    static string Next(string c="") {

        // If a c parameter is provided, verify that it matches the current character.
        if (c != ch) {
            Error("Expected '" + c + "' instead of '" + ch + "'");
        }
        // Get the next character. When there are no more characters,
        // return the empty string.
        if (at >= text.Length) return ch = "";
        ch = text.Substring(at, 1);
        at += 1;
        return ch;
    }

    static object Number() {
        // Parse a number value.
        float number = 0.0f;
        string str = "";

        if (ch == "-") {
            str = "-";
            Next("-");
        }
        while (IsChar("0123456789")) {
            str += ch;
            Next();
        }
        if (ch == ".") {
            str += ".";
            while (Next().Length > 0 && IsChar("0123456789")) {
                str += ch;
            }
        }
        if (IsChar("eE")) {
            str += ch;
            Next();
            if (IsChar("-+")) {
                str += ch;
                Next();
            }
            while (IsChar("0123456789")) {
                str += ch;
                Next();
            }

        }
        number = float.Parse(str);
        return number;
    }

    static object String() {
        // Parse a string value.
        int hex;
        int i;
        string str = "";
        int uffff = 0;

        // When parsing for string values, we must look for " and \ characters.
        if (ch == "\"") {
            while (Next().Length > 0) {
                if (ch == "\"") {
                    Next();
                    return str;
                }
                if (ch == "\\") {
                    Next();
                    if (ch == "u") {
                        uffff = 0;
                        for (i = 0; i < 4; i += 1) {
                            hex = "0123456789abcdef".IndexOf(Next());
                            uffff = uffff * 16 + hex;
                        }
                        str += Convert.ToChar(uffff);
                    } else if (escapee.ContainsKey(ch)) {
                        str += escapee[ch];
                    } else {
                        break;
                    }
                } else {
                    str += ch;
                }
            }
        }
        throw Error("Bad String");
    }

    static void White() {
        // Skip whitespace.
        while (IsChar(" \t\n\r")) {
            Next();
        }
    }

    static object Word() {
        // true, false, or null.
        switch(ch) {
        case "t":
            Next("t");
            Next("r");
            Next("u");
            Next("e");
            return true;
        case "f":
            Next("f");
            Next("a");
            Next("l");
            Next("s");
            Next("e");
            return false;
        case "n":
            Next("n");
            Next("u");
            Next("l");
            Next("l");
            return null;
        }
      throw Error("Unexpected '" + ch + "'");

    }

    static JSONArray  Array() {
        // Parse an array value.
        var array = new JSONArray();

        if (ch == "[") {
            Next("[");
            White();
            if (ch == "]") {
                Next("]");
                return array;
            }
            while (ch.Length>0) {
                array.Add(Value());
                White();
                if (ch == "]") {
                    Next("]");
                    return array;
                }
                Next(",");
                White();
            }
        }
        throw Error("Bad array");
    }


    static object Object() {
        // Parse an object value.
        string key = "";
        var obj = new JSONObject();

        if (ch == "{") {
            Next("{");
            White();
            if (ch == "}") {
                Next("}");
                return obj;
            }
            while (ch.Length>0) {
                key = (string)String();
                White();
                Next(":");
                obj.Add(key, Value());
                White();
                if (ch == "}") {
                    Next("}");
                    return obj;
                }
                Next(",");
                White();
            }
        }
        throw Error("Bad Object");
    }

    static object Value() {
        // Parse a JSON value. It could be an object, an array, a string, a number,
        // or a word.

        White();
        switch(ch) {
        case "{":
            return Object();
        case "[":
            return Array();
        case "\"":
            return String();
        case "-":
            return Number();
        default:
            return IsChar("0123456789")
                ? Number()
                : Word();
        }
    }  
  }
}

