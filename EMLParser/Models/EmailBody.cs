using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EMLParser.Models {
	/// <summary>
	/// Object representation of an email message body.
	/// </summary>
	public class EmailBody {
		private List<EmailHeader> _headers;
		private string _contents;
		private string _contentType;
		private string _transferEncoding;

		/// <summary>
		/// Creates an empty email body object.
		/// </summary>
		public EmailBody() {
			// Set some things up.
			Headers = new List<EmailHeader>();
			Contents = "";
			ContentType = null;
			TransferEncoding = null;
		}

		/// <summary>
		/// Creates a fully populated email body object.
		/// </summary>
		/// <param name="headers">Body headers.</param>
		/// <param name="contents">Contents of the message.</param>
		public EmailBody(List<EmailHeader> headers, string contents) {
			Headers = headers;
			Contents = contents;
		}

		/// <summary>
		/// Appends a line to the message body.
		/// </summary>
		/// <param name="line">Line to be appended without any newline
		/// characters.</param>
		public void AppendLine(string line) {
			if (line == null) {
				Contents += Environment.NewLine;
				return;
			}

			Contents += line + Environment.NewLine;
		}

		/// <summary>
		/// Gets the content type MIME string and puts it into the ContentType
		/// property.
		/// </summary>
		protected void GetContentType() {
			// Do nothing if the ContentType property has already been set.
			if (_contentType != null)
				return;

			// Go through the headers looking for the Content-Type.
			foreach (EmailHeader header in Headers) {
				// Is this it?
				if (header.Name != "Content-Type")
					continue;

				// Get the MIME string.
				ContentType = header.Value.Split(new char[] { ';' }, 2,
					StringSplitOptions.RemoveEmptyEntries)[0].Trim();
				return;
			}
		}

		/// <summary>
		/// Gets the transfer encoding scheme and puts it into the TransferEncoding
		/// property.
		/// </summary>
		protected void GetTransferEncoding() {
			// Do nothing if the TransferEncoding property has already been set.
			if (_transferEncoding != null)
				return;

			// Go through the headers looking for the Content-Transfer-Encoding.
			foreach (EmailHeader header in Headers) {
				// Is this it?
				if (header.Name != "Content-Transfer-Encoding")
					continue;

				// Get the scheme name.
				TransferEncoding = header.Value;
				return;
			}
		}

		/// <summary>
		/// Cleans up the contents of a body string.
		/// </summary>
		/// <param name="contents">Body contents to unescape.</param>
		/// <returns>Unescaped and clean body contents.</returns>
		protected string UnescapeBody(string contents) {
			string unescaped;

			// Do nothing if the transfer encoding scheme wasn't set.
			if (TransferEncoding == null)
				return contents;
			
			// Do we even need to do anything?
			if (!String.Equals(TransferEncoding, "Quoted-Printable",
					StringComparison.OrdinalIgnoreCase)) {
				return contents;
			}

			// Remove all of the escaped line endings.
			unescaped = contents.Replace("=" + Environment.NewLine, "");

			// TODO: Actually parse the escape characters.
			// Ignore all escaped characters.
			unescaped = Regex.Replace(unescaped, @"=[A-F0-9]{2}", "");

			return unescaped;
		}

		public override string ToString() {
			return Contents;
		}

		/// <summary>
		/// Message headers.
		/// </summary>
		public List<EmailHeader> Headers {
			get { return _headers; }
			set { _headers = value; }
		}

		/// <summary>
		/// Message content.
		/// </summary>
		public string Contents {
			get { return _contents; }
			set { _contents = UnescapeBody(value); }
		}

		/// <summary>
		/// Message content type MIME string.
		/// </summary>
		public string ContentType {
			get { GetContentType(); return _contentType; }
			set { _contentType = value; }
		}

		/// <summary>
		/// Transfer encoding scheme used in the body.
		/// </summary>
		protected string TransferEncoding {
			get { GetTransferEncoding(); return _transferEncoding; }
			set { _transferEncoding = value; }
		}
	}
}
