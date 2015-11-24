using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
/**

    json2.cs

        JSON.stringify(value, replacer, space)
            value       any JavaScript value, usually an object or array.

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 
                            ? '0' + n 
                            : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date 
                    ? 'Date(' + this[key] + ')' 
                    : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


*/

namespace json {

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
            return true;
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

    /*
    static JSONArray  Array() {
        // Parse an array value.
        JSONArray array = new JSONArray();

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
    }*/

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

