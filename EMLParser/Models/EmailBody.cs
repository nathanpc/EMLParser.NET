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

		/// <summary>
		/// Creates an empty email body object.
		/// </summary>
		public EmailBody() {
			// Set some things up.
			Headers = new List<EmailHeader>();
			Contents = "";
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
	}
}
