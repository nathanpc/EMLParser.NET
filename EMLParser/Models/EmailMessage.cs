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
		private List<EmailHeader> _headers;
		private List<EmailBody> _bodies;

		/// <summary>
		/// Creates an empty email message object.
		/// </summary>
		public EmailMessage() {
			// Initializes some of our internal variables.
			Headers = new List<EmailHeader>();
		}

		/// <summary>
		/// Creates an email message object from the contents of an EML file.
		/// </summary>
		/// <param name="emlFileContents">Contents of an EML file.</param>
		public EmailMessage(string emlFileContents) : this() {
			ParseEML(emlFileContents);
		}

		/// <summary>
		/// Actually parses the EML file contents and populates this object.
		/// </summary>
		/// <param name="eml">EML file contents.</param>
		protected void ParseEML(string eml) {
			StringReader reader = new StringReader(eml);
			EmailHeader header;
			string line;

			// Parse the headers first.
			while ((header = ParseHeader(reader)) != null) {
				// Add the parsed header to the headers list.
				Headers.Add(header);

				// Check if we have a Content-Type header.
				if (header.Name == "Content-Type") {
					// Check if we actually have a type of content we can parse.
					if (header.Fields == null) {
						throw new Exception("Currently we only support " +
							"'multipart/alternative' EML files");
					}

					// Get the boundary string.
					boundary = header.Fields["boundary"];
				}
			}

			// Read the empty line after the headers.
			line = reader.ReadLine();
			if (line.Length > 0)
				throw new Exception("Header-Body separator line has contents");

			// TODO: Check for a boundary before parsing the bodies.
		}

		/// <summary>
		/// Parses a header using an <see cref="StringReader"/>.
		/// </summary>
		/// <param name="reader">Reader of the EML contents.</param>
		/// <returns>Parsed header as a <c>string[]</c> where item 0 is the name
		/// of the header and item 1 is its value. Will return <c>null</c> if it
		/// has finished parsing the headers.</returns>
		private EmailHeader ParseHeader(StringReader reader) {
			string line;
			EmailHeader header;

			// Get the next line and check if we haven't finished parsing headers.
			line = reader.ReadLine();
			if (line.Length == 0)
				return null;

			// Separate the name and value fields.
			header = new EmailHeader(
				line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries));
			header.Value = header.Value.Trim();

			// Check if this header continues down the line.
			if (!IsNextCharWhitespace(reader))
				return header;

			// Parse the multi-line part of this header value.
			while ((line = reader.ReadLine()) != null) {
				// Append the value to the existing header value.
				header.AppendValue(line);

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
		public List<EmailHeader> Headers {
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
