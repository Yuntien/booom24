using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Conversa.Runtime
{
	[MovedFrom(true, null, "Assembly-CSharp")]
	[Serializable]
	public class StickyNoteData
	{
		public const string DefaultTitle = "New title";
		public const string DefaultContent = "New content";
		
		[FormerlySerializedAs("id")]
		[SerializeField] private string guid;
		[SerializeField] private Rect rect;
		[SerializeField] private string title;
		[SerializeField] private string contents;

		// Properties
		
		public string Guid => guid;
		public Rect Rect { get => rect; set => rect = value; }
		public string Title { get => title; set => title = value; }
		public string Contents { get => contents; set => contents = value; }

		// Methods
		
		public StickyNoteData(Vector2 position)
		{
			guid = System.Guid.NewGuid().ToString();
			Rect = new Rect(position, new Vector2(100, 100));
			Title = DefaultTitle;
			Contents = DefaultContent;
		}
	}
}