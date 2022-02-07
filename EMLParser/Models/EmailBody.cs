using System;
using System.Collections.Generic;
using System.Text;

namespace EMLParser.Models {
	/// <summary>
	/// Object representation of an email message body.
	/// </summary>
	public class EmailBody {
		private List<EmailHeader> _headers;
		private string _contents;
		private string _contentType;

		/// <summary>
		/// Creates an empty email body object.
		/// </summary>
		public EmailBody() {
			// Set some things up.
			Headers = new List<EmailHeader>();
			Contents = "";
			ContentType = null;
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
			set { _contents = value; }
		}

		/// <summary>
		/// Message content type MIME string.
		/// </summary>
		public string ContentType {
			get { GetContentType(); return _contentType; }
			set { _contentType = value; }
		}
	}
}
