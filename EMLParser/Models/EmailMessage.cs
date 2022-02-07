using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EMLParser.Models {
	/// <summary>
	/// Email message header item.
	/// </summary>
	public class EmailMessage {
		protected string boundary = "";
		private List<KeyValuePair<string, string>> _headers;
		private List<EmailBody> _bodies;

		/// <summary>
		/// Creates an empty email message object.
		/// </summary>
		public EmailMessage() {
			// Initializes some of our internal variables.
			Headers = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Creates an email message object from the contents of an EML file.
		/// </summary>
		/// <param name="emlFileContents">Contents of an EML file.</param>
		public EmailMessage(string emlFileContents) : this() {
			ParseEML(emlFileContents);
		}

		protected void ParseEML(string eml) {
			StringReader reader = new StringReader(eml);
			string[] header;

			// Parse the headers first.
			while ((header = ParseHeader(reader)) != null) {
				// Add the parsed header to the headers list.
				Headers.Add(new KeyValuePair<string, string>(header[0], header[1]));
			}

			// TODO: Check for a boundary before parsing the bodies.
		}

		/// <summary>
		/// Parses a header using an <see cref="StringReader"/>.
		/// </summary>
		/// <param name="reader">Reader of the EML contents.</param>
		/// <returns>Parsed header as a <c>string[]</c> where item 0 is the name
		/// of the header and item 1 is its value. Will return <c>null</c> if it
		/// has finished parsing the headers.</returns>
		private string[] ParseHeader(StringReader reader) {
			string line;
			string[] header = new string[2];

			// Get the next line and check if we haven't finished parsing headers.
			line = reader.ReadLine();
			if (line.Length == 0)
				return null;

			// Separate the name and value fields.
			header = line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
			header[1] = header[1].Trim();

			// Check if this header continues down the line.
			if (!IsNextCharWhitespace(reader))
				return header;

			// Parse the multi-line part of this header value.
			while ((line = reader.ReadLine()) != null) {
				// Append a space to the value in case it ended with a semi-colon.
				if (header[1][header[1].Length - 1] == ';')
					header[1] += " ";

				// Append more to the value.
				header[1] += line.Trim();

				// Check if we've finished parsing the multi-line header.
				if (!IsNextCharWhitespace(reader))
					break;
			}

			// Looks like we've finished parsing.
			return header;
		}

		/// <summary>
		/// Checks if the next character in the <see cref="StringReader"/> is
		/// whitespace without advancing the pointer.
		/// </summary>
		/// <param name="reader">Reader being used.</param>
		/// <returns>True if the next character to be read is whitespace.</returns>
		protected bool IsNextCharWhitespace(StringReader reader) {
			int c = reader.Peek();
			return (c == (int)' ') || (c == (int)'\t');
		}

		/// <summary>
		/// Message headers.
		/// </summary>
		public List<KeyValuePair<string, string>> Headers {
			get { return _headers; }
			set { _headers = value; }
		}

		/// <summary>
		/// Message bodies.
		/// </summary>
		public List<EmailBody> Bodies {
			get { return _bodies; }
			set { _bodies = value; }
		}
	}
}
