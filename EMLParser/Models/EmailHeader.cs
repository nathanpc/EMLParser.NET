using System;
using System.Collections.Generic;
using System.Text;

namespace EMLParser.Models {
	/// <summary>
	/// Object representation of a single email header.
	/// </summary>
	public class EmailHeader {
		private string _name;
		private string _value;
		private bool valueChanged;
		private Dictionary<string, string> _fields;

		/// <summary>
		/// Creates an empty email header object.
		/// </summary>
		public EmailHeader() {
			valueChanged = false;
			Name = "";
			_value = "";
			Fields = null;
		}

		/// <summary>
		/// Creates an email header object pre-populated with name and value.
		/// </summary>
		/// <param name="name">Header name.</param>
		/// <param name="value">Header value.</param>
		public EmailHeader(string name, string value) : this() {
			Name = name;
			Value = value;
		}

		/// <summary>
		/// Creates an email header object pre-populated with name and value
		/// using a string array.
		/// </summary>
		/// <param name="header">String array containing just the header name
		/// and value.</param>
		public EmailHeader(string[] header) : this() {
			// Check if the string array has the right length.
			if (header.Length > 2)
				throw new Exception("Header string array must have up to 2 items");

			// Populate the object.
			Name = header[0];
			if (header.Length == 2)
				Value = header[1];
		}

		/// <summary>
		/// Appends more data to the value string. Used when parsing multi-line
		/// headers.
		/// </summary>
		/// <param name="value">Value string. This will be trimmed before being
		/// appended.</param>
		public void AppendValue(string value) {
			// Append a space to the value in case it ended with a semi-colon.
			if (Value[Value.Length - 1] == ';')
				Value += " ";

			// Append more to the value.
			Value += value.Trim();
		}

		/// <summary>
		/// Builds up the fields dictionary if needed.
		/// </summary>
		protected void BuildFields() {
			// Build the fields only if the value variable has changed.
			if (!valueChanged)
				return;

			// Clean things up.
			if (_fields == null) {
				_fields = new Dictionary<string, string>();
			} else {
				_fields.Clear();
			}

			// Check if we even have fields to parse.
			if (!Value.Contains("=")) {
				_fields = null;
				return;
			}

			// Split the fields up.
			string[] fields = Value.Split(new char[] { ' ', ';' },
				StringSplitOptions.RemoveEmptyEntries);
			if (fields.Length == 1) {
				_fields = null;
				return;
			}

			// Go through fields parsing them.
			foreach (string strField in fields) {
				// Split the field.
				string[] field = strField.Split(new char[] { '=' },
					2, StringSplitOptions.RemoveEmptyEntries);

				// Check if we have a field that's not a pair.
				if (field.Length == 1) {
					_fields.Add(field[0], null);
					continue;
				}

				// TODO: Properly parse strings.
				// Check if we have a string for the value of a field.
				if (field[1][0] == '"')
					field[1] = field[1].Replace("\"", String.Empty);

				// Add a new field.
				_fields.Add(field[0], field[1]);
			}

			// Reset the value variable changed flag.
			valueChanged = false;
		}

		public override string ToString() {
			return Name + ": " + Value;
		}

		/// <summary>
		/// Header name.
		/// </summary>
		public string Name {
			get { return _name; }
			set { _name = value; }
		}

		/// <summary>
		/// Header plain text value.
		/// </summary>
		public string Value {
			get { return _value; }
			set {
				valueChanged = true;
				_value = value;
			}
		}

		/// <summary>
		/// Header value fields.
		/// </summary>
		public Dictionary<string, string> Fields {
			get { BuildFields(); return _fields; }
			protected set { _fields = value; }
		}
	}
}
