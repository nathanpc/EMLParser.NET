using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EMLParser.Models;

namespace EMLExample {
	/// <summary>
	/// A simple example program to test if our EML parser is actually working.
	/// </summary>
	class Program {
		/// <summary>
		/// Program's main entry point.
		/// </summary>
		/// <param name="args">Command-line arguments.</param>
		static void Main(string[] args) {
			string emlPath = @"../../../example.eml";

			// Simple header message.
			Console.WriteLine("EMLParser Example");
			Console.WriteLine();

			// Check if the user passed an EML file to be parsed.
			if (args.Length > 1)
				emlPath = args[1];

			// Parse our example EML file.
			EmailMessage msg = EMLParser.EMLParser.ParseFile(emlPath);

			// Print the parsed headers.
			Console.WriteLine("=============== BEGIN HEADERS ===============");
			foreach (EmailHeader header in msg.Headers) {
				// Print the header.
				Console.WriteLine("'{0}' = '{1}'", header.Name, header.Value);
				if (header.Fields == null)
					continue;

				// Print the header fields.
				Console.WriteLine("{");
				foreach (KeyValuePair<string, string> field in header.Fields) {
					Console.WriteLine("    '{0}' = '{1}'", field.Key, field.Value);
				}
				Console.WriteLine("}");
			}
			Console.WriteLine("=============== END HEADERS ===============");

			// Print bodies.
			foreach (EmailBody body in msg.Bodies) {
				Console.WriteLine("========== BEGIN BODY ({0}) ==========",
					body.ContentType);
				Console.Write(body.Contents);
				Console.WriteLine("========== END BODY ({0}) ==========",
					body.ContentType);
			}

			// Conviniently wait for the user to look at things.
			Console.Write("Press any key to continue...");
			Console.Read();
		}
	}
}
