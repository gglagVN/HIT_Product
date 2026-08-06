using UnityEngine;
using UnityEngine.UI;

namespace Thnguyet.UIBehavior
{
	/// To mau dinh cua mot Graphic uGUI theo Gradient, doc truc ngang hoac doc.
	/// Chi chay tren Graphic co dung IMeshModifier (Image, RawImage, UI.Text); TextMeshPro KHONG dung duoc.
	[AddComponentMenu("UI/Effects/Gradient")]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	public class UIGradient : BaseMeshEffect
	{
		public enum Type
		{
			Horizontal,
			Vertical
		}

		public enum Blend
		{
			Override,
			Add,
			Multiply
		}

		[SerializeField]
		private Type _gradientType;

		[SerializeField]
		private Blend _blendMode = Blend.Multiply;

		[Range(-1f, 1f)]
		[SerializeField]
		private float _offset;

		[SerializeField]
		private Gradient _effectGradient = new Gradient
		{
			colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(Color.white, 0f),
				new GradientColorKey(Color.black, 1f)
			}
		};

		public Blend BlendMode
		{
			get
			{
				return _blendMode;
			}
			set
			{
				_blendMode = value;
				SetDirty();
			}
		}

		public Gradient EffectGradient
		{
			get
			{
				return _effectGradient;
			}
			set
			{
				_effectGradient = value;
				SetDirty();
			}
		}

		public Type GradientType
		{
			get
			{
				return _gradientType;
			}
			set
			{
				_gradientType = value;
				SetDirty();
			}
		}

		public float Offset
		{
			get
			{
				return _offset;
			}
			set
			{
				_offset = Mathf.Clamp(value, -1f, 1f);
				SetDirty();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (IsTextMeshPro(base.graphic))
			{
				Debug.LogWarning("[UIGradient] Component nay khong co tac dung tren TextMeshPro: TMP khong xu ly BaseMeshEffect/IMeshModifier. Hay dung TMP_Text.colorGradient (VertexGradient) hoac gan mot TMP_ColorGradient asset thay cho UIGradient.", this);
			}
		}

		/// Tim min/max toa do tren truc da chon roi to lai mau tung dinh theo gradient.
		public override void ModifyMesh(VertexHelper helper)
		{
			if (!IsActive() || _effectGradient == null || helper == null)
			{
				return;
			}
			int vertexCount = helper.currentVertCount;
			if (vertexCount == 0)
			{
				return;
			}
			UIVertex vertex = default(UIVertex);
			float min = float.MaxValue;
			float max = float.MinValue;
			for (int i = 0; i < vertexCount; i++)
			{
				helper.PopulateUIVertex(ref vertex, i);
				float coordinate = GetCoordinate(vertex.position);
				if (coordinate < min)
				{
					min = coordinate;
				}
				if (coordinate > max)
				{
					max = coordinate;
				}
			}
			float size = max - min;
			if (size <= Mathf.Epsilon)
			{
				return;
			}
			float inverseSize = 1f / size;
			for (int j = 0; j < vertexCount; j++)
			{
				helper.PopulateUIVertex(ref vertex, j);
				float time = (GetCoordinate(vertex.position) - min) * inverseSize - _offset;
				vertex.color = BlendColor(vertex.color, _effectGradient.Evaluate(Mathf.Clamp01(time)));
				helper.SetUIVertex(vertex, j);
			}
		}

		/// Toa do dung de noi suy gradient: x khi Horizontal, y khi Vertical.
		private float GetCoordinate(Vector3 position)
		{
			if (_gradientType != Type.Horizontal)
			{
				return position.y;
			}
			return position.x;
		}

		private Color BlendColor(Color colorA, Color colorB)
		{
			switch (_blendMode)
			{
			case Blend.Add:
				return colorA + colorB;
			case Blend.Multiply:
				return colorA * colorB;
			default:
				return colorB;
			}
		}

		/// Yeu cau Graphic dung lai mesh de Inspector cap nhat ngay khi doi thuoc tinh.
		private void SetDirty()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

		/// Nhan dien TMP_Text ma khong tao phu thuoc bien dich vao TextMeshPro.
		private static bool IsTextMeshPro(Graphic target)
		{
			if (target == null)
			{
				return false;
			}
			for (System.Type type = target.GetType(); type != null; type = type.BaseType)
			{
				if (type.Name == "TMP_Text")
				{
					return true;
				}
			}
			return false;
		}

#if UNITY_EDITOR
		protected override void OnValidate()
		{
			base.OnValidate();
			SetDirty();
		}
#endif

		public UIGradient()
		{
		}
	}
}
