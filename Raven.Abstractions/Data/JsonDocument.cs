//-----------------------------------------------------------------------
// <copyright file="JsonDocument.cs" company="Hibernating Rhinos LTD">
//     Copyright (c) Hibernating Rhinos LTD. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Raven.Json.Linq;

namespace Raven.Abstractions.Data
{
	/// <summary>
	/// A document representation:
	/// * Data / Projection
	/// * Etag
	/// * Metadata
	/// </summary>
	public class JsonDocument : IJsonDocumentMetadata
	{
		/// <summary>
		/// Create a new instance of JsonDocument
		/// </summary>
		public JsonDocument()
		{
		}

		private RavenJObject dataAsJson;
		private RavenJObject metadata;

		/// <summary>
		/// 	Gets or sets the document data as json.
		/// </summary>
		/// <value>The data as json.</value>
		public RavenJObject DataAsJson
		{
			get { return dataAsJson ?? (dataAsJson = new RavenJObject()); }
			set { dataAsJson = value; }
		}

		/// <summary>
		/// 	Gets or sets the metadata for the document
		/// </summary>
		/// <value>The metadata.</value>
		public RavenJObject Metadata
		{
			get { return metadata ?? (metadata = new RavenJObject(StringComparer.InvariantCultureIgnoreCase)); }
			set { metadata = value; }
		}

		/// <summary>
		/// 	Gets or sets the key for the document
		/// </summary>
		/// <value>The key.</value>
		public string Key { get; set; }

		/// <summary>
		/// 	Gets or sets a value indicating whether this document is non authoritative (modified by uncommitted transaction).
		/// </summary>
		public bool? NonAuthoritativeInformation { get; set; }

		/// <summary>
		/// Gets or sets the etag.
		/// </summary>
		/// <value>The etag.</value>
		public Guid? Etag { get; set; }

		/// <summary>
		/// 	Gets or sets the last modified date for the document
		/// </summary>
		/// <value>The last modified.</value>
		public DateTime? LastModified { get; set; }

		/// <summary>
		/// 	Translate the json document to a <see cref = "RavenJObject" />
		/// </summary>
		/// <returns></returns>
		public RavenJObject ToJson()
		{
			var doc = (RavenJObject)DataAsJson.CloneToken();
			var metadata = (RavenJObject)Metadata.CloneToken();

			if (LastModified != null)
				metadata[Constants.LastModified] = LastModified.Value;
			if(Etag != null)
				metadata["@etag"] = Etag.Value.ToString();
			if (NonAuthoritativeInformation != null)
				metadata["Non-Authoritative-Information"] = NonAuthoritativeInformation.Value;

			doc["@metadata"] = metadata;

			return doc;
		}
	}
}