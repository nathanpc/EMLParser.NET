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

			// Get the contents of the included example EML file and parse it.
			string contents = File.ReadAllText(emlPath);
			EmailMessage msg = new EmailMessage(contents);

			// Print the parsed headers.
			Console.WriteLine("=============== BEGIN HEADERS ===============");
			foreach (KeyValuePair<string, string> header in msg.Headers) {
				Console.WriteLine("[ '{0}', '{1}' ]", header.Key, header.Value);
			}
			Console.WriteLine("=============== END HEADERS ===============");

			// Conviniently wait for the user to look at things.
			Console.Write("Press any key to continue...");
			Console.Read();
		}
	}
}
