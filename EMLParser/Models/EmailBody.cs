using System;
using System.Collections.Generic;
using System.Text;

namespace EMLParser.Models {
	/// <summary>
	/// Object representation of an email message body.
	/// </summary>
	public class EmailBody {
		private BodyFormat _format;
		private string _contents;

		/// <summary>
		/// Types of formating the body can have.
		/// </summary>
		public enum BodyFormat {
			PlainText = 0,
			HTML
		};

		/// <summary>
		/// Creates an empty email body object.
		/// </summary>
		public EmailBody() {
			// Set some defaults.
			Format = BodyFormat.PlainText;
		}

		/// <summary>
		/// Creates a fully populated email body object.
		/// </summary>
		/// <param name="format">Format of the message.</param>
		/// <param name="contents">Contents of the message.</param>
		public EmailBody(BodyFormat format, string contents) {
			Format = format;
			Contents = contents;
		}

		/// <summary>
		/// Message format.
		/// </summary>
		public BodyFormat Format {
			get { return _format; }
			set { _format = value; }
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
