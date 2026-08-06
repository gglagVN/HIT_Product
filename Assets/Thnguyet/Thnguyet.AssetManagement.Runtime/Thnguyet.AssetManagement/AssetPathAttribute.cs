using System;
using UnityEngine;

namespace Thnguyet.AssetManagement
{
	public class AssetPathAttribute : PropertyAttribute
	{
		public readonly Type assetType;

		public readonly bool displayPreview;

		public AssetPathAttribute(Type assetType, bool displayPreview = false)
		{
			this.assetType = assetType;
			this.displayPreview = displayPreview;
		}
	}
}
