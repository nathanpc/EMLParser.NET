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

		/// <summary>
		/// Creates an empty email header object.
		/// </summary>
		public EmailHeader() {
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
			if (header.Length != 2)
				throw new Exception("Header string array must only have 2 items");

			// Populate the object.
			Name = header[0];
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
			set { _value = value; }
		}
	}
}
