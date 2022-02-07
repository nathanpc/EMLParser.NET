using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EMLParser.Models;

namespace EMLParser {
	/// <summary>
	/// A simple way to parse your EML files.
	/// </summary>
	public class EMLParser {
		/// <summary>
		/// Parses an EML file.
		/// </summary>
		/// <param name="fileName">EML file path.</param>
		/// <returns>Parsed email message.</returns>
		public static EmailMessage ParseFile(string fileName) {
			// Get the contents of the EML file and parse it.
			return new EmailMessage(File.ReadAllText(fileName));
		}
	}
}
